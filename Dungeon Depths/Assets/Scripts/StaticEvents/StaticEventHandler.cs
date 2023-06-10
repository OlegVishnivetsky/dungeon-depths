using System;

public static class StaticEventHandler
{
    public static event Action<RoomChangedEventArgs> OnRoomChanged;

    public static void InvokeRoomChangedEvent(Room room)
    {
        OnRoomChanged?.Invoke(new RoomChangedEventArgs(room));
    }

    public static event Action<RoomEnemiesDefeatedArgs> OnRoomEnemiesDefeated;

    public static void InvokeRoomEnemiesDefeatedEvent(Room room)
    {
        OnRoomEnemiesDefeated?.Invoke(new RoomEnemiesDefeatedArgs(room));
    }

    public static event Action<PointsScoredArgs> OnPointsScored;

    public static void InvokePointsScoredEvent(int points)
    {
        OnPointsScored?.Invoke(new PointsScoredArgs(points));
    }

    public static event Action<ScoreChangedArgs> OnScoreChanged;

    public static void InvokeScoreChangedEvent(long score, int multiplier)
    {
        OnScoreChanged?.Invoke(new ScoreChangedArgs(score, multiplier));
    }

    public static event Action<MultiplierArgs> OnMultiplier;

    public static void InvokeMultiplierEvent(bool multiplier)
    {
        OnMultiplier?.Invoke(new MultiplierArgs(multiplier));
    }
}

public class RoomChangedEventArgs : EventArgs
{
    public Room room;

    public RoomChangedEventArgs(Room room)
    {
        this.room = room;
    }
}

public class RoomEnemiesDefeatedArgs : EventArgs
{
    public Room room;

    public RoomEnemiesDefeatedArgs(Room room)
    {
        this.room = room;
    }
}

public class PointsScoredArgs : EventArgs
{
    public int points;

    public PointsScoredArgs(int points)
    {
        this.points = points;
    }
}

public class ScoreChangedArgs : EventArgs
{
    public long score;
    public int multiplier;

    public ScoreChangedArgs(long score, int multiplier)
    {
        this.score = score;
        this.multiplier = multiplier;
    }
}

public class MultiplierArgs : EventArgs
{
    public bool multiplier;

    public MultiplierArgs(bool multiplier)
    {
        this.multiplier = multiplier;
    }
}