using System;
using UnityEngine;

[DisallowMultipleComponent]
public class MovementToPositionEvent : MonoBehaviour
{
    public event Action<MovementToPositionEvent, MovementToPositionEventArgs> OnMovementToPosition;

    public void InvokeMovementToPositionEvent(Vector2 movePosition, Vector2 currentPosition, Vector2 moveDirection, float moveSpeed,
        bool isRolling = false)
    {
        OnMovementToPosition?.Invoke(this, new MovementToPositionEventArgs(movePosition, currentPosition, moveDirection, moveSpeed, isRolling));
    }
}

public class MovementToPositionEventArgs : EventArgs
{
    public Vector2 movePosition;
    public Vector2 currentPosition;
    public Vector2 moveDirection;

    public float moveSpeed;

    public bool isRolling;

    public MovementToPositionEventArgs(Vector2 movePosition, Vector2 currentPosition, Vector2 moveDirection, float moveSpeed, bool isRolling)
    {
        this.movePosition = movePosition;
        this.currentPosition = currentPosition;
        this.moveDirection = moveDirection;
        this.moveSpeed = moveSpeed;
        this.isRolling = isRolling;
    }
}