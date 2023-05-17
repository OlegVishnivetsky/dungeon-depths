using System;
using UnityEngine;

[DisallowMultipleComponent]
public class DestroyedEvent : MonoBehaviour
{
    public event Action<DestroyedEvent, DestroyedEventArgs> OnDestroyed;

    public void InvokeDestroyedEvent(bool isPlayerDied)
    {
        OnDestroyed?.Invoke(this, new DestroyedEventArgs(isPlayerDied));
    }
}

public class DestroyedEventArgs : EventArgs
{
    public bool isPlayerDied;

    public DestroyedEventArgs(bool isPlayerDied)
    {
        this.isPlayerDied = isPlayerDied;
    }
}