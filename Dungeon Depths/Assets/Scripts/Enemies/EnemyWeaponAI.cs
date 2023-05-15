using UnityEngine;
using UnityEngine.Rendering;

[DisallowMultipleComponent]
[RequireComponent(typeof(Enemy))]
public class EnemyWeaponAI : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform weaponShootPosition;

    private Enemy enemy;
    private EnemyDetailsSO enemyDetails;

    private float firingIntervalTimer;
    private float firingDurationTimer;

    private Vector3 directionForGizmos;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        enemyDetails = enemy.enemyDetails;

        firingIntervalTimer = WeaponShootInterval();
        firingDurationTimer = WeaponShootDuration();
    }

    private void Update()
    {
        firingIntervalTimer -= Time.deltaTime;

        if (firingIntervalTimer < 0f)
        {
            if (firingDurationTimer >= 0f)
            {
                firingDurationTimer -= Time.deltaTime;

                FireEnemyWeapon();
            }
            else
            {
                firingIntervalTimer = WeaponShootInterval();
                firingDurationTimer = WeaponShootDuration();
            }
        }
    }

    private void FireEnemyWeapon()
    {
        Vector3 playerDirectionVector = GameManager.Instance.GetPlayer().GetPlayerPosition() - transform.position;
        Vector3 weaponDirection = GameManager.Instance.GetPlayer().GetPlayerPosition() - weaponShootPosition.position;
        directionForGizmos = weaponDirection;

        float enemyAngleDegrees = HelperUtilities.GetAngleFromVector(playerDirectionVector);
        float weaponAngleDegrees = HelperUtilities.GetAngleFromVector(weaponDirection);

        AimDirection enemyAimDirection = HelperUtilities.GetAimDirection(enemyAngleDegrees);

        enemy.aimWeaponEvent.InvokeAimWeaponEvent(enemyAimDirection, enemyAngleDegrees, 
            weaponAngleDegrees, weaponDirection);

        if (enemyDetails.enemyWeapon != null)
        {
            float enemyAmmoRange = enemyDetails.enemyWeapon.weaponCurrentAmmo.ammoRange;

            if (playerDirectionVector.magnitude <= enemyAmmoRange)
            {
                if (enemyDetails.firingLineOfSightRequired && !IsPlayerInLineOfSight(weaponDirection))
                {
                    return;
                }

                enemy.fireWeaponEvent.InvokeFireWeaponEvent(true, true, enemyAimDirection, enemyAngleDegrees, 
                    weaponAngleDegrees, weaponDirection);
            }
        }
    }

    private bool IsPlayerInLineOfSight(Vector3 weaponDirection)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(weaponShootPosition.position, weaponDirection,
            enemyDetails.enemyWeapon.weaponCurrentAmmo.ammoRange, layerMask);

        if (raycastHit2D && raycastHit2D.transform.CompareTag(Settings.playerTag))
        {
            return true;
        }

        return false;
    }

    private float WeaponShootInterval()
    {
        return Random.Range(enemyDetails.firingIntervalMin, enemyDetails.firingIntervalMax);
    }

    private float WeaponShootDuration()
    {
        return Random.Range(enemyDetails.firingDurationMin, enemyDetails.firingDurationMax);
    }

    private void OnDrawGizmos()
    {
        Ray ray = new Ray(weaponShootPosition.position, directionForGizmos);

        Gizmos.DrawRay(ray);
    }
}