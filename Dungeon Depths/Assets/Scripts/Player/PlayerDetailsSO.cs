using System.Collections.Generic;
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
    public bool isImmuneAfterHit = true;
    public float hitImmunityTime;

    [Header("WEAPON")]
    public WeaponDetailsSO startingWeapon;

    public List<WeaponDetailsSO> startingWeaponList;

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
        HelperUtilities.ValidateCheckNullValue(this, nameof(startingWeapon), startingWeapon);
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(startingWeaponList), startingWeaponList);
    }

#endif

    #endregion Validation
}