using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(IdleEvent))]
public class Idle : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    private IdleEvent idleEvent;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        idleEvent = GetComponent<IdleEvent>();
    }

    private void OnEnable()
    {
        idleEvent.OnIdle += IdleEvent_OnIdle; 
    }

    private void OnDisable()
    {
        idleEvent.OnIdle -= IdleEvent_OnIdle;
    }

    private void IdleEvent_OnIdle(IdleEvent idleEvent)
    {
        MoveRigidbody();
    }

    private void MoveRigidbody()
    {
        rigidbody2D.velocity = Vector3.zero;
    }
}