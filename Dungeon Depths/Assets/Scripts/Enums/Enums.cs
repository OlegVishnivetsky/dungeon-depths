public enum Orientation
{
    North,
    East,
    South,
    West,
    None
}

public enum GameState
{
    GameStarted,
    PlayingLevel,
    EngagingEnemies,
    BossStage,
    EngagingBoss,
    LevelCompleted,
    GameWon,
    GameLost,
    GamePaused,
    DungeonOverviewMap,
    RestartGame,
    None
}

public enum ChestSpawnEvent
{
    OnRoomEntry,
    OnEnemiesDefeated
}

public enum ChestSpawnPosition
{
    AtSpawnPosition,
    AtPlayerPosition
}

public enum ChestState
{
    Closed,
    HealthItem,
    AmmoItem,
    WeaponItem,
    Empty
}

public enum AimDirection
{
    Up,
    UpRight,
    UpLeft,
    Right,
    Left,
    Down
}