using System;
using UnityEngine;

[DisallowMultipleComponent]
public class DestroyedEvent : MonoBehaviour
{
    public event Action<DestroyedEvent, DestroyedEventArgs> OnDestroyed;

    public void InvokeDestroyedEvent(bool isPlayerDied, int points)
    {
        OnDestroyed?.Invoke(this, new DestroyedEventArgs(isPlayerDied, points));
    }
}

public class DestroyedEventArgs : EventArgs
{
    public bool isPlayerDied;
    public int points;

    public DestroyedEventArgs(bool isPlayerDied, int points)
    {
        this.isPlayerDied = isPlayerDied;
        this.points = points;
    }
}