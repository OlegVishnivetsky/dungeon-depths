using System;
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

    [HideInInspector] public int[,] aStarMovementPenalty;

    [HideInInspector] public Bounds roomColliderBounds;

    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();

        roomColliderBounds = boxCollider2D.bounds;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() && room != GameManager.Instance.GetCurrentRoom())
        {
            room.isPreviouslyVisited = true;
            StaticEventHandler.InvokeRoomChangedEvent(room);
        }
    }

    public void Initialise(GameObject roomGameobject)
    {
        PopulateTilemapMemberVariables(roomGameobject);

        BlockOfUnusedDoorways();

        AddObstaclesAndPreferredPath();

        DisableCollisionTilemapRenderer();

        AddDoorsToRooms();
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

    private void BlockOfUnusedDoorways()
    {
        foreach (Doorway doorway in room.doorWayList)
        {
            if (doorway.isConnected)
                continue;

            if (collisionTilemap != null)
            {
                BlockDoorwayOnTilemapLayer(collisionTilemap, doorway);
            }

            if (minimapTilemap != null)
            {
                BlockDoorwayOnTilemapLayer(minimapTilemap, doorway);
            }

            if (groundTilemap != null)
            {
                BlockDoorwayOnTilemapLayer(groundTilemap, doorway);
            }

            if (decoration1Tilemap != null)
            {
                BlockDoorwayOnTilemapLayer(decoration1Tilemap, doorway);
            }

            if (decoration2Tilemap != null)
            {
                BlockDoorwayOnTilemapLayer(decoration2Tilemap, doorway);
            }

            if (frontTilemap != null)
            {
                BlockDoorwayOnTilemapLayer(frontTilemap, doorway);
            }
        }
    }

    private void AddObstaclesAndPreferredPath()
    {
        aStarMovementPenalty = new int[room.templateUpperBounds.x - room.templateLowerBounds.x + 1, 
            room.templateUpperBounds.y - room.templateLowerBounds.y + 1];

        for (int x = 0; x < (room.templateUpperBounds.x - room.templateLowerBounds.x + 1); x++)
        {
            for (int y = 0; y < (room.templateUpperBounds.y - room.templateLowerBounds.y + 1); y++)
            {
                aStarMovementPenalty[x, y] = Settings.defaultAStarMovementPenalty;

                TileBase tile = collisionTilemap.GetTile(new Vector3Int(x + room.templateLowerBounds.x, y + room.templateLowerBounds.y, 0));

                foreach (TileBase collisionTile in GameResources.Instance.enemyUnwalkableCollisionTilesArray)
                {
                    if (tile == collisionTile)
                    {
                        aStarMovementPenalty[x, y] = 0;
                        break;
                    }
                }

                if (tile == GameResources.Instance.preferredEnemyPathTile)
                {
                    aStarMovementPenalty[x, y] = Settings.preferredPathAStartMovementPenalty;
                }
            }
        }
    }

    private void BlockDoorwayOnTilemapLayer(Tilemap tilemap, Doorway doorway)
    {
        switch (doorway.orientation)
        {
            case Orientation.North:
            case Orientation.South:
                BlockDoorwayHorizontally(tilemap, doorway);
                break;

            case Orientation.East:
            case Orientation.West:
                BlockDoorwayVertically(tilemap, doorway);
                break;

            case Orientation.None:
                break;
        }
    }

    private void BlockDoorwayHorizontally(Tilemap tilemap, Doorway doorway)
    {
        Vector2Int startPosition = doorway.doorwayStartCopyPosition;

        for (int xPos = 0; xPos < doorway.doorwayCopyTileWidth; xPos++)
        {
            for (int yPos = 0; yPos < doorway.doorwayCopyTileHeight; yPos++)
            {
                Matrix4x4 transformMatrix = tilemap.GetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0));

                tilemap.SetTile(new Vector3Int(startPosition.x + 1 + xPos, startPosition.y - yPos, 0),
                    tilemap.GetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0)));

                tilemap.SetTransformMatrix(new Vector3Int(startPosition.x + 1 + xPos, startPosition.y - yPos, 0), transformMatrix);
            }
        }
    }

    private void BlockDoorwayVertically(Tilemap tilemap, Doorway doorway)
    {
        Vector2Int startPosition = doorway.doorwayStartCopyPosition;

        for (int yPos = 0; yPos < doorway.doorwayCopyTileHeight; yPos++)
        {
            for (int xPos = 0; xPos < doorway.doorwayCopyTileWidth; xPos++)
            {
                Matrix4x4 transformMatrix = tilemap.GetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0));

                tilemap.SetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - 1 - yPos, 0),
                    tilemap.GetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0)));

                tilemap.SetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - 1 - yPos, 0), transformMatrix);
            }
        }
    }

    private void DisableCollisionTilemapRenderer()
    {
        collisionTilemap.gameObject.GetComponent<TilemapRenderer>().enabled = false;
    }

    private void AddDoorsToRooms()
    {
        if (room.roomNodeType.isCorridorEW || room.roomNodeType.isCorridorNS)
        {
            return;
        }

        float tileDistance = Settings.tileSizePixels / Settings.pixelsPerUnit;

        foreach (Doorway doorway in room.doorWayList)
        {
            GameObject doorObject = null;

            if (doorway.doorPrefab != null && doorway.isConnected)
            {
                if (doorway.orientation == Orientation.North)
                {
                    doorObject = InstantiateAndPositionDoorObject(doorway.doorPrefab, new Vector3(doorway.position.x + tileDistance / 2f,
                        doorway.position.y + tileDistance, 0f));
                }
                else if (doorway.orientation == Orientation.South)
                {
                    doorObject = InstantiateAndPositionDoorObject(doorway.doorPrefab, new Vector3(doorway.position.x + tileDistance / 2f,
                        doorway.position.y, 0f));
                }
                else if (doorway.orientation == Orientation.East)
                {
                    doorObject = InstantiateAndPositionDoorObject(doorway.doorPrefab, new Vector3(doorway.position.x + tileDistance,
                        doorway.position.y + tileDistance * 1.25f, 0f));
                }
                else if (doorway.orientation == Orientation.West)
                {
                    doorObject = InstantiateAndPositionDoorObject(doorway.doorPrefab, new Vector3(doorway.position.x,
                        doorway.position.y + tileDistance * 1.25f, 0f));
                }

                Door doorComponent = doorObject.GetComponent<Door>();

                if (room.roomNodeType.isBossRoom)
                {
                    doorComponent.isBossRoomDoor = true;
                    doorComponent.LockDoor();
                }
            }
        }

        GameObject InstantiateAndPositionDoorObject(GameObject doorPrefab, Vector3 position)
        {
            GameObject doorObject = Instantiate(doorPrefab, gameObject.transform);
            doorObject.transform.localPosition = position;
            return doorObject;
        }
    }
}