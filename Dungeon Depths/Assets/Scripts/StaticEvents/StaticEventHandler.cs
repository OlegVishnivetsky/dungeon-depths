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