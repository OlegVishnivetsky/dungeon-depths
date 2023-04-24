using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SetActiveWeaponEvent))]
public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;
    [SerializeField] private PolygonCollider2D weaponPolygonCollider2D;
    [SerializeField] private Transform weaponShootPositionTransform;
    [SerializeField] private Transform weaponEffectPositionTransfrom; 
    
    private SetActiveWeaponEvent setWeaponEvent;
    private Weapon currentWeapon;

    private void Awake()
    {
        setWeaponEvent = GetComponent<SetActiveWeaponEvent>();
    }

    private void OnEnable()
    {
        setWeaponEvent.OnSetActiveWeapon += SetWeaponEvent_OnSetActiveWeapon;
    }

    private void OnDisable()
    {
        setWeaponEvent.OnSetActiveWeapon -= SetWeaponEvent_OnSetActiveWeapon;
    }

    private void SetWeaponEvent_OnSetActiveWeapon(SetActiveWeaponEvent SetActiveWeaponEvent, 
        SetActiveWeaponEventArgs setActiveWeaponArgs)
    {
        SetWeapon(setActiveWeaponArgs.weapon);
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public void RemoveCurrentWeapon()
    {
        currentWeapon = null;
    }

    public Vector3 GetShootPosition()
    {
        return weaponShootPositionTransform.position;
    }

    public Vector3 GetWeaponEffectPosition()
    {
        return weaponEffectPositionTransfrom.position;
    }

    public AmmoDetailsSO GetCurrentAmmo()
    {
        return currentWeapon.weaponDetails.weaponCurrentAmmo;
    }

    private void SetWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
        weaponSpriteRenderer.sprite = weapon.weaponDetails.weaponSprite;

        if (weaponPolygonCollider2D != null && weaponSpriteRenderer.sprite != null)
        {
            List<Vector2> spritePhysicsShapePointsList = new List<Vector2>();

            weaponSpriteRenderer.sprite.GetPhysicsShape(0, spritePhysicsShapePointsList);

            weaponPolygonCollider2D.points = spritePhysicsShapePointsList.ToArray();
        }

        weaponShootPositionTransform.localPosition = weapon.weaponDetails.weaponShootPosition;
    }
}