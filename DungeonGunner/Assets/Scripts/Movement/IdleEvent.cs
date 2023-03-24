using System;
using UnityEngine;

[DisallowMultipleComponent]
public class IdleEvent : MonoBehaviour
{
    public event Action<IdleEvent> OnIdle;

    public void InvokeIdleEvent()
    {
        OnIdle?.Invoke(this);
    }
}