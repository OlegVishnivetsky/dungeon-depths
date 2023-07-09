using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    [System.Serializable]
    private struct RangeByLevel
    {
        public DungeonLevelSO dungeonLevel;
        [Range(0, 100)] public int min;
        [Range(0, 100)] public int max;
    }

    [Header("CHEST PREFAB")]
    [SerializeField] private GameObject chestPrefab;

    [Header("CHEST SPAWN CHANCE")]
    [SerializeField, Range(0, 100)] private int chestSpawnChanceMin;
    [SerializeField, Range(0, 100)] private int chestSpawnChanceMax;
    [SerializeField] private List<RangeByLevel> chestSpawnChanceByLevelList;

    [Header("CHEST SPAWN DETAILS")]
    [SerializeField] private ChestSpawnEvent chestSpawnEvent;
    [SerializeField] private ChestSpawnPosition chestSpawnPosition;
    [SerializeField, Range(0, 3)] private int numberOfItemsToSpawnMin;
    [SerializeField, Range(0, 3)] private int numberOfItemsToSpawnMax;

    [Header("CHEST CONTENT DETAILS")]
    [SerializeField] private List<SpawnableObjectByLevel<WeaponDetailsSO>> weaponSpawnByLevelList;
    [SerializeField] private List<RangeByLevel> healthSpawnByLevelList;
    [SerializeField] private List<RangeByLevel> ammoSpawnByLevelList;

    private Room chestRoom;
    private bool chestSpawned = false;

    private void OnEnable()
    {
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;
        StaticEventHandler.OnRoomEnemiesDefeated += StaticEventHandler_OnRoomEnemiesDefeated;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;
        StaticEventHandler.OnRoomEnemiesDefeated -= StaticEventHandler_OnRoomEnemiesDefeated;
    }

    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {
        if (chestRoom == null)
        {
            chestRoom = GetComponentInParent<InstantiatedRoom>().room;
        }

        if (!chestSpawned && chestSpawnEvent == ChestSpawnEvent.OnRoomEntry && chestRoom == roomChangedEventArgs.room)
        {
            SpawnChest();
        }
    }

    private void StaticEventHandler_OnRoomEnemiesDefeated(RoomEnemiesDefeatedArgs roomEnemiesDefeatedArgs)
    {
        if (chestRoom == null)
        {
            chestRoom = GetComponentInParent<InstantiatedRoom>().room;
        }
        if (!chestSpawned && chestSpawnEvent == ChestSpawnEvent.OnEnemiesDefeated && chestRoom == roomEnemiesDefeatedArgs.room)
        {
            SpawnChest();
        }
    }

    private void SpawnChest()
    {
        chestSpawned = true;

        if (!RandomSpawnChest()) return;

        GetItemsToSpawn(out int ammoNum, out int healthNum, out int weaponNum);

        GameObject chestGameObject = Instantiate(chestPrefab, this.transform);

        if (chestSpawnPosition == ChestSpawnPosition.AtSpawnPosition)
        {
            chestGameObject.transform.position = this.transform.position;
        }
        else if (chestSpawnPosition == ChestSpawnPosition.AtPlayerPosition)
        {
            Vector3 spawnPosition = HelperUtilities.GetSpawnPositionNearestToPlayer(GameManager.Instance.GetPlayer().transform.position);

            Vector3 variation = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);

            chestGameObject.transform.position = spawnPosition + variation;
        }

        Chest chest = chestGameObject.GetComponent<Chest>();

        if (chestSpawnEvent == ChestSpawnEvent.OnRoomEntry)
        {
            chest.Initialize(false, GetHealthPercentToSpawn(healthNum), GetWeaponDetailsToSpawn(weaponNum), GetAmmoPercentToSpawn(ammoNum));
        }
        else
        {
            chest.Initialize(true, GetHealthPercentToSpawn(healthNum), GetWeaponDetailsToSpawn(weaponNum), GetAmmoPercentToSpawn(ammoNum));
        }
    }

    private bool RandomSpawnChest()
    {
        int chancePercent = Random.Range(chestSpawnChanceMin, chestSpawnChanceMax + 1);

        foreach (RangeByLevel rangeByLevel in chestSpawnChanceByLevelList)
        {
            if (rangeByLevel.dungeonLevel == GameManager.Instance.GetCurrentDungeonLevel())
            {
                chancePercent = Random.Range(rangeByLevel.min, rangeByLevel.max + 1);
                break;
            }
        }

        int randomPercent = Random.Range(1, 100 + 1);

        if (randomPercent <= chancePercent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void GetItemsToSpawn(out int ammo, out int health, out int weapons)
    {
        ammo = 0;
        health = 0;
        weapons = 0;

        int numberOfItemsToSpawn = Random.Range(numberOfItemsToSpawnMin, numberOfItemsToSpawnMax + 1);

        int choice;

        if (numberOfItemsToSpawn == 1)
        {
            choice = Random.Range(0, 3);
            if (choice == 0) { weapons++; return; }
            if (choice == 1) { ammo++; return; }
            if (choice == 2) { health++; return; }
            return;
        }
        else if (numberOfItemsToSpawn == 2)
        {
            choice = Random.Range(0, 3);
            if (choice == 0) { weapons++; ammo++; return; }
            if (choice == 1) { ammo++; health++; return; }
            if (choice == 2) { health++; weapons++; return; }
        }
        else if (numberOfItemsToSpawn >= 3)
        {
            weapons++;
            ammo++;
            health++;
            return;
        }
    }

    private int GetAmmoPercentToSpawn(int ammoNumber)
    {
        if (ammoNumber == 0) return 0;

        foreach (RangeByLevel spawnPercentByLevel in ammoSpawnByLevelList)
        {
            if (spawnPercentByLevel.dungeonLevel == GameManager.Instance.GetCurrentDungeonLevel())
            {
                return Random.Range(spawnPercentByLevel.min, spawnPercentByLevel.max);
            }
        }

        return 0;
    }

    private int GetHealthPercentToSpawn(int healthNumber)
    {
        if (healthNumber == 0) return 0;

        foreach (RangeByLevel spawnPercentByLevel in healthSpawnByLevelList)
        {
            if (spawnPercentByLevel.dungeonLevel == GameManager.Instance.GetCurrentDungeonLevel())
            {
                return Random.Range(spawnPercentByLevel.min, spawnPercentByLevel.max);
            }
        }

        return 0;
    }

    private WeaponDetailsSO GetWeaponDetailsToSpawn(int weaponNumber)
    {
        if (weaponNumber == 0) return null;

        RandomSpawnableObject<WeaponDetailsSO> weaponRandom = new RandomSpawnableObject<WeaponDetailsSO>(weaponSpawnByLevelList);

        WeaponDetailsSO weaponDetails = weaponRandom.GetItem();

        return weaponDetails;
    }
}