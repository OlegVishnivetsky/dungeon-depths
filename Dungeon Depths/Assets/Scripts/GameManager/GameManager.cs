using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    [SerializeField] private List<DungeonLevelSO> dungeonLevelList;
    [SerializeField] private int currentDungeonLevelListIndex = 0;

    [Space(10), SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private CanvasGroup canvasGroup;

    private Room currentRoom;
    private Room previousRoom;

    private PlayerDetailsSO playerDetails;
    private Player player;

    [HideInInspector] public GameState gameState;
    [HideInInspector] public GameState previousGameState;

    private InstantiatedRoom bossRoom;

    private int playerScore;
    private int scoreMultiplier;

    protected override void Awake()
    {
        base.Awake();

        playerDetails = GameResources.Instance.currentPlayerSO.playerDetails;
        InstantiatePlayer();
    }

    private void OnEnable()
    {
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;
        StaticEventHandler.OnRoomEnemiesDefeated += StaticEventHandler_OnRoomEnemiesDefeated;
        StaticEventHandler.OnPointsScored += StaticEventHandler_OnPointsScored;
        StaticEventHandler.OnMultiplier += StaticEventHandler_OnMultiplier;

        player.destroyedEvent.OnDestroyed += DestroyedEvent_OnDestroyed;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;
        StaticEventHandler.OnRoomEnemiesDefeated -= StaticEventHandler_OnRoomEnemiesDefeated;
        StaticEventHandler.OnPointsScored -= StaticEventHandler_OnPointsScored;
        StaticEventHandler.OnMultiplier -= StaticEventHandler_OnMultiplier;

        player.destroyedEvent.OnDestroyed -= DestroyedEvent_OnDestroyed;
    }

    private void Start()
    {
        playerScore = 0;
        scoreMultiplier = 1;

        previousGameState = GameState.GameStarted;
        gameState = GameState.GameStarted;

    }

    private void Update()
    {
        HandleGameState();
    }

    public Player GetPlayer()
    {
        return player;
    }

    public Sprite GetPlayerMinimapIcon()
    {
        return playerDetails.playerMinimapIcon;
    }

    public Room GetCurrentRoom()
    {
        return currentRoom;
    }

    public DungeonLevelSO GetCurrentDungeonLevel()
    {
        return dungeonLevelList[currentDungeonLevelListIndex];
    }

    public void SetCurrentRoom(Room room)
    {
        currentRoom = room;
    }

    private void HandleGameState()
    {
        switch (gameState)
        {
            case GameState.GameStarted:
                PlayDungeonLevel(currentDungeonLevelListIndex);
                gameState = GameState.PlayingLevel;
                RoomEnemiesDefeated();
                break;
            case GameState.LevelCompleted:
                StartCoroutine(LevelCompleted());
                break;
            case GameState.GameWon:
                if (previousGameState != GameState.GameWon)
                {
                    StartCoroutine(GameWon());
                }
                break;
            case GameState.GameLost:
                if (previousGameState != GameState.GameWon)
                {
                    StartCoroutine(GameLost());
                }
                break;
            case GameState.RestartGame:
                RestartGame();
                break;
        }
    }

    private IEnumerator LevelCompleted()
    {
        gameState = GameState.PlayingLevel;

        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(Fade(0, 1, 2, new Color(0, 0, 0, 0.4f)));

        yield return StartCoroutine(DisplayMessageRoutine($"CONGRATS {GameResources.Instance.currentPlayerSO.playerName}!\n\n" +
            $"LEVEL COMPLETED", Color.white, 5));
        yield return StartCoroutine(DisplayMessageRoutine($"PRESS RETURN KEY\n\nTO GO TO THE NEXT LEVEL!", Color.white, 5));

        yield return StartCoroutine(Fade(1, 0, 2, new Color(0, 0, 0, 0.4f)));

        while (Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }

        yield return null;

        currentDungeonLevelListIndex++;
        PlayDungeonLevel(currentDungeonLevelListIndex);
    }

    private IEnumerator GameWon()
    {
        previousGameState = GameState.GameWon;
        GetPlayer().playerControl.DisablePlayerMovement();

        yield return StartCoroutine(Fade(0, 1, 2, Color.black));

        yield return StartCoroutine(DisplayMessageRoutine($"WELL DONE {GameResources.Instance.currentPlayerSO.playerName} " +
            $"YOU HAME DEFEATED THE DUNGEON", Color.white, 4));

        yield return StartCoroutine(DisplayMessageRoutine($"YOU SCORED {playerScore.ToString("###,###0")}", Color.white, 4));

        yield return StartCoroutine(DisplayMessageRoutine($"PRESS RETURN KEY RO RESTART", Color.white, 0));

        gameState = GameState.RestartGame;
    }

    private IEnumerator GameLost()
    {
        previousGameState = GameState.GameLost;
        gameState = GameState.None;
        GetPlayer().playerControl.DisablePlayerMovement();

        StartCoroutine(Fade(0, 1, 2, Color.black));

        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
        }

        yield return StartCoroutine(DisplayMessageRoutine($"YOU DIED", Color.white, 2));

        yield return StartCoroutine(DisplayMessageRoutine($"YOU SCORED {playerScore.ToString("###,###0")}", Color.white, 2));

        yield return StartCoroutine(DisplayMessageRoutine($"PRESS RETURN KEY RO RESTART", Color.white, 0));

        gameState = GameState.RestartGame;
    }

    private IEnumerator BossStage()
    {
        bossRoom.gameObject.SetActive(true);
        bossRoom.UnlockDoors();

        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(Fade(0, 1, 2, new Color(0, 0, 0, 0.4f)));

        yield return StartCoroutine(DisplayMessageRoutine($"WELL DONE {GameResources.Instance.currentPlayerSO.playerName} YOU HAVE REACHED THE FINAL STAGE!\n\n" +
            $"DEFEAT THE BOSS TO GO TO ANOTHER LEVEL", Color.white, 5));

        yield return StartCoroutine(Fade(1, 0, 2, new Color(0, 0, 0, 0.4f)));
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    private void PlayDungeonLevel(int dungeonLevelListIndex)
    {
        bool dungeonBuiltSucessfully = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);

        if (!dungeonBuiltSucessfully)
        {
            Debug.LogError("Couldn't build dungeon from specified rooms and node graphs");
        }

        StaticEventHandler.InvokeRoomChangedEvent(currentRoom);

        player.gameObject.transform.position = new Vector3((currentRoom.lowerBounds.x + currentRoom.upperBounds.x) / 2f,
            (currentRoom.lowerBounds.y + currentRoom.upperBounds.y) / 2f, 0f);

        StartCoroutine(DisplayLevelDungeonText());

        player.gameObject.transform.position = HelperUtilities.GetSpawnPositionNearestToPlayer(player.gameObject.transform.position);
    }

    private void InstantiatePlayer()
    {
        GameObject playerObject = Instantiate(playerDetails.playerPrefab);

        player = playerObject.GetComponent<Player>();
        player.Initialize(playerDetails);
    }

    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedArgs)
    {
        SetCurrentRoom(roomChangedArgs.room);
    }

    private void StaticEventHandler_OnPointsScored(PointsScoredArgs pointsScoredArgs)
    {
        playerScore += pointsScoredArgs.points * scoreMultiplier;

        StaticEventHandler.InvokeScoreChangedEvent(playerScore, scoreMultiplier);
    }

    private void StaticEventHandler_OnMultiplier(MultiplierArgs multiplierArgs)
    {
        if (multiplierArgs.multiplier && scoreMultiplier < 30)
        {
            scoreMultiplier++;
        }
        else if (!multiplierArgs.multiplier && scoreMultiplier > 1)
        {
            scoreMultiplier--;
        }

        StaticEventHandler.InvokeScoreChangedEvent(playerScore, scoreMultiplier);
    }

    private void StaticEventHandler_OnRoomEnemiesDefeated(RoomEnemiesDefeatedArgs roomEnemiesDefeatedArgs)
    {
        RoomEnemiesDefeated();
    }

    private void RoomEnemiesDefeated()
    {
        bool isDungeonClearOfRegularEnemies = true;

        bossRoom = null;

        foreach (KeyValuePair<string, Room> keyValuePair in DungeonBuilder.Instance.dungeonBuilderRoomDictionary)
        {
            Room room = keyValuePair.Value;

            if (room.roomNodeType.isBossRoom)
            {
                bossRoom = room.instantiatedRoom;
                continue;
            }

            if (!room.isClearedOfEnemies)
            {
                isDungeonClearOfRegularEnemies = false;
                break;
            }
        }

        if ((isDungeonClearOfRegularEnemies && bossRoom == null) || (isDungeonClearOfRegularEnemies && bossRoom.room.isClearedOfEnemies))
        {
            if (currentDungeonLevelListIndex < dungeonLevelList.Count - 1)
            {
                gameState = GameState.LevelCompleted;
            }
            else
            {
                gameState = GameState.GameWon;
            }
        }
        else if (isDungeonClearOfRegularEnemies)
        {
            gameState = GameState.BossStage;

            StartCoroutine(BossStage());
        }
    }
    private void DestroyedEvent_OnDestroyed(DestroyedEvent destroyedEvent, DestroyedEventArgs destroyedEventArgs)
    {
        previousGameState = gameState;
        gameState = GameState.GameLost;
    }

    private IEnumerator DisplayLevelDungeonText()
    {
        StartCoroutine(Fade(0, 1, 0, Color.black));
        GetPlayer().playerControl.DisablePlayerMovement();

        string text = $"LEVEL {currentDungeonLevelListIndex}\n\n{dungeonLevelList[currentDungeonLevelListIndex].levelName.ToUpper()}";

        yield return StartCoroutine(DisplayMessageRoutine(text, Color.white, 2));

        GetPlayer().playerControl.EnablePlayerMovement();

        yield return StartCoroutine(Fade(1, 0, 2, Color.black));
    }

    private IEnumerator DisplayMessageRoutine(string messageText, Color color, float duration)
    {
        this.messageText.text = messageText;
        this.messageText.color = color;

        if (duration > 0f)
        {
            float timer = duration;

            while (timer > 0f && !Input.GetKeyDown(KeyCode.Return))
            {
                timer -= Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null;
            }
        }

        yield return null;

        this.messageText.SetText(" ");
    }

    private IEnumerator Fade(float startAlpha, float targetAlpha, float duration, Color color)
    {
        Image image = canvasGroup.GetComponent<Image>();
        image.color = color;

        canvasGroup.alpha = startAlpha;

        float time = 0f;

        while (time <= duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }
    }

    #region Validation

#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(dungeonLevelList), dungeonLevelList);
    }

#endif

    #endregion Validation
}