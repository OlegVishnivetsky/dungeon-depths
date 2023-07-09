using System.Collections.Generic;
using UnityEngine;

public class ActivateRooms : MonoBehaviour
{
    [SerializeField] private Camera minimapCamera;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        InvokeRepeating("EnableRooms", 0.5f, 0.75f);
    }

    private void EnableRooms()
    {
        HelperUtilities.CameraWorldPositionBounds(out Vector2Int minimapCameraWorldPositionLowerBounds,
                out Vector2Int minimapCameraWorldPositionUpperBounds, minimapCamera);

        HelperUtilities.CameraWorldPositionBounds(out Vector2Int cameraWorldPositionLowerBounds,
            out Vector2Int cameraWorldPositionUpperBounds, mainCamera);

        foreach (KeyValuePair<string, Room> keyValuePair in DungeonBuilder.Instance.dungeonBuilderRoomDictionary)
        {
            Room room = keyValuePair.Value;

            if ((room.lowerBounds.x <= minimapCameraWorldPositionUpperBounds.x && room.lowerBounds.y <= minimapCameraWorldPositionUpperBounds.y)
                && (room.upperBounds.x >= minimapCameraWorldPositionLowerBounds.x && room.upperBounds.y >= minimapCameraWorldPositionLowerBounds.y))
            {
                room.instantiatedRoom.gameObject.SetActive(true);

                if ((room.lowerBounds.x <= cameraWorldPositionUpperBounds.x && room.lowerBounds.y <= cameraWorldPositionUpperBounds.y)
                && (room.upperBounds.x >= cameraWorldPositionLowerBounds.x && room.upperBounds.y >= cameraWorldPositionLowerBounds.y))
                {
                    room.instantiatedRoom.ActivateEnvironmentGameObjects();
                }
                else
                {
                    room.instantiatedRoom.DeactivateEnvironmentGameObjects();
                }
            }
            else
            {
                room.instantiatedRoom.gameObject.SetActive(false);
            }
        }
    }
}