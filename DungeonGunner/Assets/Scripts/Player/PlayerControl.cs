using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private MovementDetailsSO movementDetails;
    [SerializeField] private Transform weaponShootPosition;

    private Player player;

    private float moveSpeed;

    private void Awake()
    {
        moveSpeed = movementDetails.GetMoveSpeed();
        player = GetComponent<Player>();
    }

    private void Update()
    {
        MovementInput();
        WeaponInput();
    }

    private void MovementInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(horizontalInput, verticalInput).normalized;

        if (direction != Vector2.zero )
        {
            player.movementByVelocityEvent.InvokeMovementByVelocityEvent(direction, moveSpeed);
        }
        else
        {
            player.idleEvent.InvokeIdleEvent();
        }
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

    #region Validation

#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(movementDetails), movementDetails);
    }

#endif

    #endregion
}