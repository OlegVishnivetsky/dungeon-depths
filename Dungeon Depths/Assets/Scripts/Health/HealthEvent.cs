using System;
using UnityEngine;

[DisallowMultipleComponent]
public class HealthEvent : MonoBehaviour
{
    public event Action<HealthEvent, HealthEventArgs> OnHealthChanged;

    public void InvokeHealthChangedEvent(float healthPercent, int healthAmount, int damageAmount)
    {
        OnHealthChanged?.Invoke(this, new HealthEventArgs(healthPercent, healthAmount, damageAmount));
    }
}

public class HealthEventArgs : EventArgs
{
    public float healthPercent;
    public int healthAmount;
    public int damageAmount;

    public HealthEventArgs(float healthPercent, int healthAmount, int damageAmount)
    {
        this.healthPercent = healthPercent;
        this.healthAmount = healthAmount;
        this.damageAmount = damageAmount;
    }
}