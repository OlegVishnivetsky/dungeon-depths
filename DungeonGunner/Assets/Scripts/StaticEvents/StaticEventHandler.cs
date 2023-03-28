using System;

public static class StaticEventHandler
{
    public static event Action<RoomChangedEventArgs> OnRoomChanged;

    public static void InvokeRoomChangedEvent(Room room)
    {
        OnRoomChanged?.Invoke(new RoomChangedEventArgs(room));
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