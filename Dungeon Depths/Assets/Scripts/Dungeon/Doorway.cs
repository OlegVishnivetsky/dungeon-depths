using UnityEngine;

[System.Serializable]
public class Doorway 
{
    public Vector2Int position;
    public Orientation orientation;
    public GameObject doorPrefab;
    public Vector2Int doorwayStartCopyPosition;
    public int doorwayCopyTileWidth;
    public int doorwayCopyTileHeight;
    [HideInInspector]
    public bool isConnected = false;
    [HideInInspector]
    public bool isUnavailable = false;
}
