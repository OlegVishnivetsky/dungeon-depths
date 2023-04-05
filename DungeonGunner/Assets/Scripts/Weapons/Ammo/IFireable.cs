using UnityEngine;

public interface IFireable
{
    void InitializeAmmo(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, float ammoSpeed,
        Vector3 weaponAimDirectionVector, bool overrideAmmoMovement = false);

    GameObject GetGameObject();
}