using UnityEngine;

[CreateAssetMenu(fileName = "AmmoDetails_", menuName = "Scriptable Objects/Weapons/Ammo Details")]
public class AmmoDetailsSO : ScriptableObject
{
    [Header("BASIC AMMO DETAILS")]
    public string ammoName;

    public bool isPlayerAmmo;

    [Header("AMMO SPRITE, PREFAB & MATERIALS")]
    public Sprite ammoSprite;

    public GameObject[] ammoPrefabArray;
    public Material ammoMaterial;

    public float ammoChargeTime = 0.1f;
    public Material ammoChargeMaterial;

    [Header("AMMO BASE PARAMETERS")]
    public int ammoDamage = 1;

    public float ammoSpeedMin = 20f;
    public float ammoSpeedMax = 20f;
    public float ammoRange = 20f;
    public float ammoRotationSpeed = 1f;

    [Header("AMMO SPREAD DETAILS")]
    public float ammoSpreadMin = 0f;

    public float ammoSpreadMax = 0f;

    [Header("AMMO SPAWN DETAILS")]
    public int ammoSpawnAmountMin = 1;

    public int ammoSpawnAmountMax = 1;

    public float ammoSpawnIntervalMin = 0f;
    public float ammoSpawnIntervalMax = 0f;

    [Header("AMMO TRAIL DETAILS")]
    public bool isAmmoTrail = false;

    public float ammoTrailLifetime = 3f;
    public Material ammoTrailMaterial;
    [Range(0f, 1f)] public float ammoTrailStartWidth;
    [Range(0f, 1f)] public float ammoTrailEndWidth;
}