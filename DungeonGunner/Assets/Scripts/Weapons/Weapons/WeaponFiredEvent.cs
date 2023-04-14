using System;
using UnityEngine;

[DisallowMultipleComponent]
public class WeaponFiredEvent : MonoBehaviour
{
    public event Action<WeaponFiredEvent, WeaponFiredEventArgs> OnWeaponFired;

    public void InvokeWeaponFiredEvent(Weapon weapon)
    {
        OnWeaponFired?.Invoke(this, new WeaponFiredEventArgs(weapon));
    }
}

public class WeaponFiredEventArgs : EventArgs
{
    public Weapon weapon;

    public WeaponFiredEventArgs(Weapon weapon)
    {
        this.weapon = weapon;
    }
}