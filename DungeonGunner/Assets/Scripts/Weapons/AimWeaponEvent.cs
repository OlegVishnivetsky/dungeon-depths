using System;
using UnityEngine;

[DisallowMultipleComponent]
public class AimWeaponEvent : MonoBehaviour
{
    public event Action<AimWeaponEvent, AimWeaponEventArgs> OnWeaponAim;

    public void InvokeAimWeaponEvent(AimDirection aimDirection, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirection)
    {
        OnWeaponAim?.Invoke(this, new AimWeaponEventArgs(aimDirection, aimAngle, weaponAimAngle, weaponAimDirection));
    }
}

public class AimWeaponEventArgs : EventArgs
{
    public AimDirection aimDirection;

    public float aimAngle;
    public float weaponAimAngle;

    public Vector3 weaponAimDirection;

    public AimWeaponEventArgs(AimDirection aimDirection, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirection)
    {
        this.aimDirection = aimDirection;
        this.aimAngle = aimAngle;
        this.weaponAimAngle = weaponAimAngle;
        this.weaponAimDirection = weaponAimDirection;
    }
}