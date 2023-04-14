using System;
using UnityEngine;

[DisallowMultipleComponent]
public class FireWeaponEvent : MonoBehaviour
{
    public event Action<FireWeaponEvent, FireWeaponEventArgs> OnFireWeapon;

    public void InvokeFireWeaponEvent(bool fire, AimDirection aimDirection, float aimAngle, float weaponAimAngle,
         Vector3 weaponAimDirectionVector)
    {
        OnFireWeapon?.Invoke(this, new FireWeaponEventArgs(fire, aimDirection, aimAngle, weaponAimAngle,
            weaponAimDirectionVector));
    }
}

public class FireWeaponEventArgs : EventArgs
{
    public bool fire;
    public AimDirection aimDirection;
    public float aimAngle;
    public float weaponAimAngle;
    public Vector3 weaponAimDirectionVector;

    public FireWeaponEventArgs(bool fire, AimDirection aimDirection, float aimAngle, float weaponAimAngle,
        Vector3 weaponAimDirectionVector)
    {
        this.fire = fire;
        this.aimDirection = aimDirection;
        this.aimAngle = aimAngle;
        this.weaponAimAngle = weaponAimAngle;
        this.weaponAimDirectionVector = weaponAimDirectionVector;
    }
}