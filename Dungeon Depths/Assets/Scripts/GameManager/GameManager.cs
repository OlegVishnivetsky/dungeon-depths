using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    [SerializeField] private List<DungeonLevelSO> dungeonLevelList;
    [SerializeField] private int currentDungeonLevelListIndex = 0;

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
                    StopAllCoroutines();
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

        yield return new WaitForSeconds(10f);

        gameState = GameState.RestartGame;
    }

    private IEnumerator GameLost()
    {
        previousGameState = GameState.GameLost;

        yield return new WaitForSeconds(10f);

        gameState = GameState.RestartGame;

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

    private IEnumerator BossStage()
    {
        bossRoom.gameObject.SetActive(true);
        bossRoom.UnlockDoors();

        yield return new WaitForSeconds(2f);
    }

    private void DestroyedEvent_OnDestroyed(DestroyedEvent destroyedEvent, DestroyedEventArgs destroyedEventArgs)
    {
        
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