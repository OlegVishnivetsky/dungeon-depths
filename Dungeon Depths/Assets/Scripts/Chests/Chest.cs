using System;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(MaterializeEffect))]
public class Chest : MonoBehaviour, IUseable
{
    [SerializeField, ColorUsage(false, true)] private Color materializeColor;
    [SerializeField] private float materializeTime = 3f;
    [SerializeField] private Transform itemSpawnPoint;

    private int healthPrecent;
    private int ammoPrecent;
    private WeaponDetailsSO weaponDetails;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private MaterializeEffect materializeEffect;

    private bool isEnabled = false;

    private ChestState chestState = ChestState.Closed;

    private GameObject chestItemObject;
    private ChestItem chestItem;

    private TextMeshPro messageTextTMP;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        materializeEffect = GetComponent<MaterializeEffect>();
        messageTextTMP = GetComponentInChildren<TextMeshPro>();
    }

    public void Initialize(bool shouldMaterialize, int healthPrecent, WeaponDetailsSO weaponDetails, int ammoPrecent)
    {
        this.healthPrecent = healthPrecent;
        this.weaponDetails = weaponDetails;
        this.ammoPrecent = ammoPrecent;

        if (shouldMaterialize)
        {
            StartCoroutine(MaterializeChest());
        }
        else
        {
            EnableChest();
        }
    }

    private IEnumerator MaterializeChest()
    {
        SpriteRenderer[] spriteRenderers = new SpriteRenderer[] { spriteRenderer };

        yield return StartCoroutine(materializeEffect.MaterializeRoutine(GameResources.Instance.materializeShader,
            materializeColor, materializeTime, spriteRenderers, GameResources.Instance.litMaterial));

        EnableChest();
    }

    private void EnableChest()
    {
        isEnabled = true;
    }

    public void UseItem()
    {
        if (!isEnabled) { return; }

        switch (chestState)
        {
            case ChestState.Closed:
                OpenChest();
                break;

            case ChestState.HealthItem:
                CollectHealthItem();
                break;

            case ChestState.AmmoItem:
                CollectAmmoItem();
                break;

            case ChestState.WeaponItem:
                CollectWeaponItem();
                break;

            case ChestState.Empty:
                break;

            default:
                break;
        }
    }

    private void OpenChest()
    {
        animator.SetBool(Settings.use, true);

        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.chestOpen);

        if (weaponDetails != null)
        {
            if (GameManager.Instance.GetPlayer().IsWeaponHeldByPlayer(weaponDetails))
            {
                weaponDetails = null;
            }
        }

        UpdateChestState();
    }

    private void CollectHealthItem()
    {
        if (chestItem == null || !chestItem.isMaterialized)
        {
            return;
        }

        GameManager.Instance.GetPlayer().health.AddHealth(healthPrecent);

        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.healthPickup);

        healthPrecent = 0;
        Destroy(chestItemObject);
        UpdateChestState();
    }

    private void CollectWeaponItem()
    {
        if (chestItem == null || !chestItem.isMaterialized)
        {
            return;
        }

        if (!GameManager.Instance.GetPlayer().IsWeaponHeldByPlayer(weaponDetails))
        {
            GameManager.Instance.GetPlayer().AddWeaponToPlayer(weaponDetails);

            SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.weaponPickup);
        }
        else
        {
            StartCoroutine(DisplayMessage("WEAPON\nALREADY\nEQUIPPED", 5F));
        }

        weaponDetails = null;
        Destroy(chestItemObject);
        UpdateChestState();
    }

    private void CollectAmmoItem()
    {
        if (chestItem == null || !chestItem.isMaterialized)
        {
            return;
        }

        Player player = GameManager.Instance.GetPlayer();

        player.reloadWeaponEvent.CallReloadWeaponEvent(player.activeWeapon.GetCurrentWeapon(), ammoPrecent);

        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.ammoPickup);

        ammoPrecent = 0;
        Destroy(chestItemObject);
        UpdateChestState();
    }

    private void UpdateChestState()
    {
        if (healthPrecent != 0)
        {
            chestState = ChestState.HealthItem;
            InstantiateHealthItem();
        }
        else if (weaponDetails != null)
        {
            chestState = ChestState.WeaponItem;
            InstantiateWeaponItem();
        }
        else if (ammoPrecent != 0)
        {
            chestState = ChestState.AmmoItem;
            InstantiateAmmoItem();
        }
        else
        {
            chestState = ChestState.Empty;
        }
    }

    private void InstantiateAmmoItem()
    {
        InstantiateItem();
        chestItem.Initialize(GameResources.Instance.bulletIcon, healthPrecent.ToString() + "%", itemSpawnPoint.position,
            materializeColor);
    }

    private void InstantiateWeaponItem()
    {
        InstantiateItem();
        chestItemObject.GetComponent<ChestItem>().Initialize(weaponDetails.weaponSprite, weaponDetails.weaponName,
            itemSpawnPoint.position, materializeColor);
    }

    private void InstantiateHealthItem()
    {
        InstantiateItem();
        chestItem.Initialize(GameResources.Instance.heartIcon, healthPrecent.ToString() + "%", itemSpawnPoint.position,
            materializeColor);
    }

    private void InstantiateItem()
    {
        chestItemObject = Instantiate(GameResources.Instance.chestItemPrefab, transform);
        chestItem = chestItemObject.GetComponent<ChestItem>();
    }

    private IEnumerator DisplayMessage(string text, float duration)
    {
        messageTextTMP.text = text;

        yield return new WaitForSeconds(duration);

        messageTextTMP.text = "";
    }
}