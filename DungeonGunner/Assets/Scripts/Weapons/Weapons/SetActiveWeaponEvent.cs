using System;
using UnityEngine;

[DisallowMultipleComponent]
public class SetActiveWeaponEvent : MonoBehaviour
{
    public event Action<SetActiveWeaponEvent, SetActiveWeaponEventArgs> OnSetActiveWeapon;

    public void InvokeSetActiveWeaponEvent(Weapon weapon)
    {
        OnSetActiveWeapon?.Invoke(this, new SetActiveWeaponEventArgs(weapon));
    }
}

public class SetActiveWeaponEventArgs : EventArgs
{
    public Weapon weapon;

    public SetActiveWeaponEventArgs(Weapon weapon)
    {
        this.weapon = weapon;
    }
}