using UnityEngine;
using UnityEngine.Rendering;

//[DisallowMultipleComponent]
//[RequireComponent(typeof(Health))]
//[RequireComponent(typeof(PlayerControl))]
//[RequireComponent(typeof(AnimatePlayer))]
//[RequireComponent(typeof(IdleEvent))]
//[RequireComponent(typeof(Idle))]
//[RequireComponent(typeof(AimWeaponEvent))]
//[RequireComponent(typeof(AimWeapon))]
//[RequireComponent(typeof(SortingGroup))]
//[RequireComponent(typeof(SpriteRenderer))]
//[RequireComponent(typeof(Animator))]
//[RequireComponent(typeof(BoxCollider2D))]
//[RequireComponent(typeof(PolygonCollider2D))]
//[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerDetailsSO playerDetails;
    [HideInInspector] public Health health;
    [HideInInspector] public IdleEvent idleEvent;
    [HideInInspector] public AimWeaponEvent aimWeaponEvent;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;

    private void Awake()
    {
        health = GetComponent<Health>();
        idleEvent = GetComponent<IdleEvent>();
        aimWeaponEvent = GetComponent<AimWeaponEvent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void Initialize(PlayerDetailsSO playerDetails)
    {
        this.playerDetails = playerDetails;

        SetPlayerHealth();
    }

    private void SetPlayerHealth()
    {
        health.SetStartingHealth(playerDetails.playerHealth);
    }
}