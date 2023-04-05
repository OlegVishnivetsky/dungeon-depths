using UnityEngine;

[DisallowMultipleComponent]
public class Ammo : MonoBehaviour, IFireable
{
    [SerializeField] private TrailRenderer trailRenderer;

    private float ammoRange = 0f;
    private float ammoSpeed;

    private Vector3 fireDirectionVector;

    private float fireDirectionAngle;

    private SpriteRenderer spriteRenderer;
    private AmmoDetailsSO ammoDetails;

    private float ammoChargeTimer;

    private bool isAmmoMaterialSet = false;
    private bool overrideAmmoMovement = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (ammoChargeTimer > 0)
        {
            ammoChargeTimer -= Time.deltaTime;
            return;
        }
        else if (!isAmmoMaterialSet)
        {
            SetAmmoMaterial(ammoDetails.ammoMaterial);
            isAmmoMaterialSet = true;
        }

        Vector3 distanceVector = fireDirectionVector * ammoSpeed * Time.deltaTime;

        transform.position += distanceVector;
        ammoRange -= distanceVector.magnitude;

        if (ammoRange < 0f)
        {
            DisableAmmo();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DisableAmmo();
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void InitializeAmmo(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, float ammoSpeed,
        Vector3 weaponAimDirectionVector, bool overrideAmmoMovement = false)
    {
        this.ammoDetails = ammoDetails;
        spriteRenderer.sprite = ammoDetails.ammoSprite;
        ammoRange = ammoDetails.ammoRange;
        this.ammoSpeed = ammoSpeed;
        this.overrideAmmoMovement = overrideAmmoMovement;

        SetFireDirection(ammoDetails, aimAngle, weaponAimAngle, weaponAimDirectionVector);

        if (ammoDetails.ammoChargeTime > 0f)
        {
            ammoChargeTimer = ammoDetails.ammoChargeTime;
            SetAmmoMaterial(ammoDetails.ammoChargeMaterial);
            isAmmoMaterialSet = false;
        }
        else
        {
            ammoChargeTimer = 1f;
            SetAmmoMaterial(ammoDetails.ammoMaterial);
            isAmmoMaterialSet = true;
        }

        gameObject.SetActive(true);

        if (ammoDetails.isAmmoTrail)
        {
            trailRenderer.gameObject.SetActive(true);
            trailRenderer.emitting = true;
            trailRenderer.material = ammoDetails.ammoTrailMaterial;
            trailRenderer.startWidth = ammoDetails.ammoTrailStartWidth;
            trailRenderer.endWidth = ammoDetails.ammoTrailEndWidth;
            trailRenderer.time = ammoDetails.ammoTrailLifetime;
        }
        else
        {
            trailRenderer.emitting = false;
            trailRenderer.gameObject.SetActive(false);
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
        transform.eulerAngles = new Vector3(0f, 0f, fireDirectionAngle);
        fireDirectionVector = HelperUtilities.GetDirectionVectorFronAngle(fireDirectionAngle);
    }

    private void SetAmmoMaterial(Material ammoMaterial)
    {
        spriteRenderer.material = ammoMaterial;
    }

    private void DisableAmmo()
    {
        gameObject.SetActive(false);
    }
}