using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDetail_", menuName = "Scriptable Objects/Player/Player Details")]
public class PlayerDetailsSO : ScriptableObject
{
    [Header("PLAYER BASE DETAILS")]
    public string playerCharacterName;

    public GameObject playerPrefab;
    public RuntimeAnimatorController controller;

    [Header("HEALTH")]
    public int playerHealth;

    [Header("OTHER")]
    public Sprite playerMinimapIcon;

    public Sprite playerHandSprite;

    #region Validation

#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(playerCharacterName), playerCharacterName);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(playerHealth), playerHealth, true);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerPrefab), playerPrefab);
    }

#endif

    #endregion Validation
}