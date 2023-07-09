using UnityEngine;

public class AmmoPattern : MonoBehaviour, IFireable
{
    [SerializeField] private Ammo[] ammoArray;

    private float ammoRange;
    private float ammoSpeed;
    private Vector3 fireDirectionVector;
    private float fireDirectionAngle;
    private AmmoDetailsSO ammoDetails;
    private float ammoChargeTimer;

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void InitialiseAmmo(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, float ammoSpeed, Vector3 weaponAimDirectionVector, bool overrideAmmoMovement)
    {
        this.ammoDetails = ammoDetails;

        this.ammoSpeed = ammoSpeed;

        // Set fire direction
        SetFireDirection(ammoDetails, aimAngle, weaponAimAngle, weaponAimDirectionVector);

        // Set ammo range
        ammoRange = ammoDetails.ammoRange;

        // Activate ammo pattern gameobject
        gameObject.SetActive(true);

        // Loop through all child ammo and initialise it
        foreach (Ammo ammo in ammoArray)
        {
            ammo.InitialiseAmmo(ammoDetails, aimAngle, weaponAimAngle, ammoSpeed, weaponAimDirectionVector, true);
        }

        // Set ammo charge timer - this will hold the ammo briefly
        if (ammoDetails.ammoChargeTime > 0f)
        {
            ammoChargeTimer = ammoDetails.ammoChargeTime;
        }
        else
        {
            ammoChargeTimer = 0f;
        }
    }

    private void Update()
    {
        if (ammoChargeTimer > 0f)
        {
            ammoChargeTimer -= Time.deltaTime;
            return;
        }

        Vector3 distanceVector = fireDirectionVector * ammoSpeed * Time.deltaTime;

        transform.position += distanceVector;

        transform.Rotate(new Vector3(0f, 0f, ammoDetails.ammoRotationSpeed * Time.deltaTime));

        ammoRange -= distanceVector.magnitude;

        if (ammoRange < 0f)
        {
            DisableAmmo();
        }

    }

    private void SetFireDirection(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        float randomSpread = Random.Range(ammoDetails.ammoSpreadMin, ammoDetails.ammoSpreadMax);

        int spreadToggle = Random.Range(0, 2) * 2 - 1;

        if (weaponAimDirectionVector.magnitude < Settings.useAimAngleDistance)
        {
            fireDirectionAngle = aimAngle;
        }
        else
        {
            fireDirectionAngle = weaponAimAngle;
        }

        fireDirectionAngle += spreadToggle * randomSpread;

        fireDirectionVector = HelperUtilities.GetDirectionVectorFromAngle(fireDirectionAngle);
    }

    private void DisableAmmo()
    {
        gameObject.SetActive(false);
    }
}