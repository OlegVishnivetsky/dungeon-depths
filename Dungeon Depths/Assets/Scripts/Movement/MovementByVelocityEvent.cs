using System;
using UnityEngine;

[DisallowMultipleComponent]
public class MovementByVelocityEvent : MonoBehaviour
{
    public event Action<MovementByVelocityEvent, MovementByVelocityEventArgs> OnMovementByVelocity;

    public void InvokeMovementByVelocityEvent(Vector3 moveDirection, float moveSpeed)
    {
        OnMovementByVelocity?.Invoke(this, new MovementByVelocityEventArgs(moveDirection, moveSpeed));
    }
}

public class MovementByVelocityEventArgs : EventArgs
{
    public Vector3 moveDirection;
    public float moveSpeed;

    public MovementByVelocityEventArgs(Vector3 moveDirection, float moveSpeed)
    {
        this.moveDirection = moveDirection;
        this.moveSpeed = moveSpeed;
    }
}