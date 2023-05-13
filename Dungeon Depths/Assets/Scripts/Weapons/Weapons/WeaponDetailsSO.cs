using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDetails_", menuName = "Scriptable Objects/Weapons/Weapon Details")]
public class WeaponDetailsSO : ScriptableObject
{
    [Header("WEAPON BASE DETAILS")]
    public string weaponName;

    public Sprite weaponSprite;

    [Header("WEAPON CONFIGURATION")]
    public Vector3 weaponShootPosition;

    public AmmoDetailsSO weaponCurrentAmmo;

    public WeaponShootEffectSO weaponShootEffect;

    [Header("WEAPON SOUNDS")]
    public SoundEffectSO weaponFireSoundEffect;
    public SoundEffectSO weaponReloadSoundEffect;

    [Header("WEAPON OPERATING VALUES")]
    public bool hasInfiniteAmmo = false;

    public bool hasInfiniteClipCapacity = false;

    public int weaponClipAmmoCapacity = 6;
    public int weaponAmmoCapacity = 100;

    public float weaponFireRate = 0.2f;
    public float weaponPreChargeTime = 0f;
    public float weaponReloadTime = 0f;

    #region Validation

#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(weaponName), weaponName);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponFireRate), weaponFireRate, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponPreChargeTime), weaponPreChargeTime, true);

        if (!hasInfiniteAmmo)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponAmmoCapacity), weaponAmmoCapacity, false);
        }

        if (!hasInfiniteClipCapacity)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponClipAmmoCapacity), weaponClipAmmoCapacity, false);
        }
    }

#endif

    #endregion Validation
}