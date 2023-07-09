using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class CharacterSelectorUI : MonoBehaviour
{
    [SerializeField] private Transform characterSelector;
    [SerializeField] private TMP_InputField playerNameInput;

    private List<PlayerDetailsSO> playerDetailsList;
    private GameObject playerSelectionPrefab;
    private CurrentPlayerSO currentPlayer;
    private List<GameObject> playerCharacterGameObjectList = new List<GameObject>();

    private Coroutine coroutine;

    private int selectedPlayerIndex;
    private float offset = 4f;

    private void Awake()
    {
        playerSelectionPrefab = GameResources.Instance.playerSelectionPrefab;
        playerDetailsList = GameResources.Instance.playerDetailsList;
        currentPlayer = GameResources.Instance.currentPlayerSO;
    }

    private void Start()
    {
        for (int i = 0; i < playerDetailsList.Count; i++)
        {
            GameObject playerSelectionObject = Instantiate(playerSelectionPrefab, characterSelector);
            playerCharacterGameObjectList.Add(playerSelectionObject);

            playerSelectionObject.transform.localPosition = new Vector3(i * offset, 0, 0);
            playerSelectionObject.transform.localScale = new Vector3(1.5f, 1.5f, 1);
            PopulatePlayerDetails(playerSelectionObject.GetComponent<PlayerSelectionUI>(), playerDetailsList[i]);
        }

        playerNameInput.text = currentPlayer.name;
        currentPlayer.playerDetails = playerDetailsList[selectedPlayerIndex];
    }

    private void PopulatePlayerDetails(PlayerSelectionUI playerSelectionUI, PlayerDetailsSO playerDetailsSO)
    {
        playerSelectionUI.playerHandSpriteRenderer.sprite = playerDetailsSO.playerHandSprite;
        playerSelectionUI.playerHandNoWeaponSpriteRenderer.sprite = playerDetailsSO.playerHandSprite;
        playerSelectionUI.playerWeaponSpriteRenderer.sprite = playerDetailsSO.startingWeapon.weaponSprite;
        playerSelectionUI.animator.runtimeAnimatorController = playerDetailsSO.controller;
    }

    public void NextCharacter()
    {
        if (selectedPlayerIndex >= playerDetailsList.Count - 1)
        {
            return;
        }

        selectedPlayerIndex++;

        currentPlayer.playerDetails = playerDetailsList[selectedPlayerIndex];

        MoveToSelectedCharacter(selectedPlayerIndex);
    }

    public void PreviousCharacter()
    {
        if (selectedPlayerIndex == 0)
        {
            return;
        }

        selectedPlayerIndex--;

        currentPlayer.playerDetails = playerDetailsList[selectedPlayerIndex];

        MoveToSelectedCharacter(selectedPlayerIndex);
    }

    private void MoveToSelectedCharacter(int selectedPlayerIndex)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(MoveToSelectedCharacterRoutine(selectedPlayerIndex));
    }

    private IEnumerator MoveToSelectedCharacterRoutine(int selectedPlayerIndex)
    {
        float currentLocalXPosition = characterSelector.localPosition.x;
        float targetLocalXPosition = selectedPlayerIndex * offset * characterSelector.localScale.x * -1f;

        while (Mathf.Abs(currentLocalXPosition - targetLocalXPosition) > 0.01f)
        {
            currentLocalXPosition = Mathf.Lerp(currentLocalXPosition, targetLocalXPosition, Time.deltaTime * 10f);

            characterSelector.localPosition = new Vector3(currentLocalXPosition, characterSelector.localPosition.y, 0f);
            yield return null;
        }

        characterSelector.localPosition = new Vector3(targetLocalXPosition, characterSelector.localPosition.y, 0f);
    }

    public void UpdatePlayerName()
    {
        playerNameInput.text = playerNameInput.text.ToUpper();

        currentPlayer.playerName = playerNameInput.text;
    }
}