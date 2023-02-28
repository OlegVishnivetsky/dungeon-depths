using UnityEngine;
using UnityEngine.Tilemaps;

[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider2D))]
public class InstantiatedRoom : MonoBehaviour
{
    [HideInInspector] public Room room;
    [HideInInspector] public Grid grid;
    [HideInInspector] public Tilemap groundTilemap;
    [HideInInspector] public Tilemap decoration1Tilemap;
    [HideInInspector] public Tilemap decoration2Tilemap;
    [HideInInspector] public Tilemap frontTilemap;
    [HideInInspector] public Tilemap collisionTilemap;
    [HideInInspector] public Tilemap minimapTilemap;
    [HideInInspector] public Bounds roomColliderBounds;

    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();

        roomColliderBounds = boxCollider2D.bounds;
    }

    public void Initialise(GameObject roomGameobject)
    {
        PopulateTilemapMemberVariables(roomGameobject);

        DisableCollisionTilemapRenderer();
    }

    private void PopulateTilemapMemberVariables(GameObject roomGameobject)
    {
        grid = roomGameobject.GetComponentInChildren<Grid>();

        Tilemap[] tilemaps = roomGameobject.GetComponentsInChildren<Tilemap>();

        foreach (Tilemap tilemap in tilemaps)
        {
            if (tilemap.gameObject.tag == StringConstants.groundTilemapTag)
            {
                groundTilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == StringConstants.decoration1TilemapTag)
            {
                decoration1Tilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == StringConstants.decoration2TilemapTag)
            {
                decoration2Tilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == StringConstants.frontTilemapTag)
            {
                frontTilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == StringConstants.collisionTilemapTag)
            {
                collisionTilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == StringConstants.minimapTilemapTag)
            {
                minimapTilemap = tilemap;
            }
        }
    }

    private void DisableCollisionTilemapRenderer()
    {
        collisionTilemap.gameObject.GetComponent<TilemapRenderer>().enabled = false;
    }
}