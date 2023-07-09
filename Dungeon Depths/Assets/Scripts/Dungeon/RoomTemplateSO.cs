using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Room_", menuName = "Scriptable Objects/Dungeon/Room")]
public class RoomTemplateSO : ScriptableObject
{
    [HideInInspector] public string guid;

    public GameObject prefab;

    [HideInInspector] public GameObject previousPrefab;

    [Header("ROOM MUSIC")]
    public MusicTrackSO battleMusic;
    public MusicTrackSO ambientMusic;

    public RoomNodeTypeSO roomNodeType;

    public Vector2Int lowerBounds;
    public Vector2Int upperBounds;

    [SerializeField] public List<Doorway> doorwayList;

    public Vector2Int[] spawnPositionArray;

    [Header("ENEMY DETAILS")]
    public List<SpawnableObjectByLevel<EnemyDetailsSO>> enemiesByLevelList;
    public List<RoomEnemySpawnParameters> roomEnemySpawnParametersList;

    public List<Doorway> GetDoorwayList()
    {
        return doorwayList;
    }

    #region Validation

#if UNITY_EDITOR

    private void OnValidate()
    {
        if (guid == "" || previousPrefab != prefab)
        {
            guid = GUID.Generate().ToString();
            previousPrefab = prefab;
            EditorUtility.SetDirty(this);
        }

        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(doorwayList), doorwayList);
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(spawnPositionArray), spawnPositionArray);
    }

#endif

    #endregion Validation
}