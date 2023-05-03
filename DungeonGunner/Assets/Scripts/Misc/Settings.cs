using UnityEngine;

public static class Settings
{
    #region UNITS

    public const float pixelsPerUnit = 16f;
    public const float tileSizePixels = 16f;

    #endregion UNITS

    #region DUNGEON BUILD SETTINGS

    public const int maxDungeonRebuildAttemptsForRoomGraph = 1000;
    public const int maxDungeonBuildAttempts = 10;

    #endregion DUNGEON BUILD SETTINGS

    #region ROOM SETTINGS

    public const float fadeInTime = 0.5f;
    public const int maxChildCorridors = 3;

    #endregion ROOM SETTINGS

    #region ANIMATOR PARAMETERS

    public static int aimUp = Animator.StringToHash("aimUp");
    public static int aimDown = Animator.StringToHash("aimDown");
    public static int aimUpRight = Animator.StringToHash("aimUpRight");
    public static int aimUpLeft = Animator.StringToHash("aimUpLeft");
    public static int aimRight = Animator.StringToHash("aimRight");
    public static int aimLeft = Animator.StringToHash("aimLeft");
    public static int isIdle = Animator.StringToHash("isIdle");
    public static int isMoving = Animator.StringToHash("isMoving");
    public static int rollUp = Animator.StringToHash("rollUp");
    public static int rollDown = Animator.StringToHash("rollDown");
    public static int rollLeft = Animator.StringToHash("rollLeft");
    public static int rollRight = Animator.StringToHash("rollRight");
    public static int openDoor = Animator.StringToHash("open");

    public static float baseSpeedForPlayerAnimation = 8f;

    #endregion ANIMATOR PARAMETERS

    #region GAMEOBJECT TAGS

    public const string playerTag = "Player";
    public const string playerWeapon = "playerWeapon";

    #endregion GAMEOBJECT TAGS

    #region FIRING CONTROL

    public const float useAimAngleDistance = 3.5f;

    #endregion FIRING CONTROL

    #region ASTAR PATHFINDING PARAMETERS

    public const int defaultAStarMovementPenalty = 40;
    public const int preferredPathAStartMovementPenalty = 1;

    #endregion

    #region UI PARAMETERS

    public const float uiAmmoIconSpacing = 4f;

    #endregion
}