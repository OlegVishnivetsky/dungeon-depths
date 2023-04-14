using System;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Player))]
public class PlayerControl : MonoBehaviour
{
    [SerializeField] private MovementDetailsSO movementDetails;

    private Player player;

    private int currentWeaponIndex = 1;

    private float moveSpeed;

    private Coroutine playerRollCoroutine;
    private WaitForFixedUpdate waitForFixedUpdate;

    private bool isPlayerRolling = false;
    private float playerRollCooldownTimer = 0f;

    private void Awake()
    {
        moveSpeed = movementDetails.GetMoveSpeed();
        player = GetComponent<Player>();
    }

    private void Start()
    {
        waitForFixedUpdate = new WaitForFixedUpdate();

        SetStartingWeapon();

        SetPlayerAnimationSpeed();
    }

    private void Update()
    {
        if (isPlayerRolling)
        {
            return;
        }

        MovementInput();
        WeaponInput();
        PlayerRollCooldownTimer();
    }

    private void MovementInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        bool rightMouseButtonDown = Input.GetMouseButtonDown(1);

        Vector2 direction = new Vector2(horizontalInput, verticalInput).normalized;

        if (direction != Vector2.zero)
        {
            if (!rightMouseButtonDown)
            {
                player.movementByVelocityEvent.InvokeMovementByVelocityEvent(direction, moveSpeed);
            }
            else if (playerRollCooldownTimer <= 0f)
            {
                RollPlayer((Vector3)direction);
            }
        }
        else
        {
            player.idleEvent.InvokeIdleEvent();
        }
    }

    private void RollPlayer(Vector3 direction)
    {
        playerRollCoroutine = StartCoroutine(RollPlayerRoutine(direction));
    }

    private IEnumerator RollPlayerRoutine(Vector3 direction)
    {
        float minDistance = 0.2f;

        isPlayerRolling = true;

        Vector3 targetPostion = player.transform.position + direction * movementDetails.rollDistance;

        while (Vector3.Distance(player.transform.position, targetPostion) > minDistance)
        {
            player.movementToPositionEvent.InvokeMovementToPositionEvent(targetPostion,
                player.transform.position, direction, movementDetails.rollSpeed, isPlayerRolling);

            yield return waitForFixedUpdate;
        }

        isPlayerRolling = false;
        playerRollCooldownTimer = movementDetails.rollCooldownTime;
        player.transform.position = targetPostion;
    }

    private void WeaponInput()
    {
        Vector3 weaponDirection;
        float weaponAngleDegrees, playerAngleDengrees;
        AimDirection playerAimDirection;

        AimWeaponInput(out weaponDirection, out weaponAngleDegrees, out playerAngleDengrees, out playerAimDirection);
        FireWeaponInput(weaponDirection, weaponAngleDegrees, playerAngleDengrees, playerAimDirection);
    }

    private void AimWeaponInput(out Vector3 weaponDirection, out float weaponAngleDegrees, out float playerAngleDengrees,
        out AimDirection aimDirection)
    {
        Vector3 mouseWorldPosition = HelperUtilities.GetMouseWorldPosition();
        Vector3 playerDirection = (mouseWorldPosition - transform.position);

        weaponDirection = (mouseWorldPosition - player.activeWeapon.GetWeaponShootPosition());
        weaponAngleDegrees = HelperUtilities.GetAngleFromVector(weaponDirection);
        playerAngleDengrees = HelperUtilities.GetAngleFromVector(playerDirection);
        aimDirection = HelperUtilities.GetAimDirection(playerAngleDengrees);

        player.aimWeaponEvent.InvokeAimWeaponEvent(aimDirection, playerAngleDengrees, weaponAngleDegrees,
            playerDirection);
    }

    private void FireWeaponInput(Vector3 weaponDirection, float weaponAngleDegrees, float playerAngleDengrees, AimDirection playerAimDirection)
    {
        if (Input.GetMouseButton(0))
        {
            player.fireWeaponEvent.InvokeFireWeaponEvent(true, playerAimDirection, playerAngleDengrees,
                weaponAngleDegrees, weaponDirection);
        }
    }

    private void PlayerRollCooldownTimer()
    {
        if (playerRollCooldownTimer >= 0f)
        {
            playerRollCooldownTimer -= Time.deltaTime;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        StopPlayerRollRoutne();
    }

    private void StopPlayerRollRoutne()
    {
        if (playerRollCoroutine != null)
        {
            StopCoroutine(playerRollCoroutine);
            isPlayerRolling = false;
        }
    }

    private void SetPlayerAnimationSpeed()
    {
        player.animator.speed = moveSpeed / Settings.baseSpeedForPlayerAnimation;
    }

    private void SetStartingWeapon()
    {
        int index = 1;

        foreach (Weapon weapon in player.weapoList)
        {
            if (weapon.weaponDetails == player.playerDetails.startingWeapon)
            {
                SetWeaponByIndex(index);
                break;
            }

            index++;
        }
    }

    private void SetWeaponByIndex(int weaponIndex)
    {
        if (weaponIndex - 1 < player.weapoList.Count)
        {
            currentWeaponIndex = weaponIndex;
            player.setActiveWeaponEvent.InvokeSetActiveWeaponEvent(player.weapoList[weaponIndex - 1]);
        }
    }

    #region Validation

#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(movementDetails), movementDetails);
    }

#endif

    #endregion Validation
}