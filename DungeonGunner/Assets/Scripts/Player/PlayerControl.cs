using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private Transform weaponShootPosition;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        MovementInput();
        WeaponInput();
    }

    private void MovementInput()
    {
        player.idleEvent.InvokeIdleEvent();
    }

    private void WeaponInput()
    {
        Vector3 weaponDirection;
        float weaponAngleDegrees, playerAngleDengrees;
        AimDirection aimDirection;

        AimWeaponInput(out weaponDirection, out weaponAngleDegrees, out playerAngleDengrees, out aimDirection);
    }

    private void AimWeaponInput(out Vector3 weaponDirection, out float weaponAngleDegrees, out float playerAngleDengrees, 
        out AimDirection aimDirection)
    {
        Vector3 mouseWorldPosition = HelperUtilities.GetMouseWorldPosition();
        Vector3 playerDirection = (mouseWorldPosition - transform.position);

        weaponDirection = (mouseWorldPosition - weaponShootPosition.position);
        weaponAngleDegrees = HelperUtilities.GetAngleFromVector(weaponDirection);
        playerAngleDengrees = HelperUtilities.GetAngleFromVector(playerDirection);
        aimDirection = HelperUtilities.GetAimDirection(playerAngleDengrees);

        player.aimWeaponEvent.InvokeAimWeaponEvent(aimDirection, playerAngleDengrees, weaponAngleDegrees,
            playerDirection);
    }
}