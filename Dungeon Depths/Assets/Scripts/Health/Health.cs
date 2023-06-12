using System;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(HealthEvent))]
public class Health : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;

    private int startingHealth;
    private int currentHealth;

    private Player player;
    [HideInInspector] public Enemy enemy;

    private Coroutine immunityCoroutine;
    private bool isImmunityAfterHit;
    private float immunityTime;
    private const float spriteFlashInterval = 0.2f;

    private SpriteRenderer spriteRenderer;

    private HealthEvent healthEvent;

    [HideInInspector] public bool isDamageable = true;

    private void Awake()
    {
        healthEvent = GetComponent<HealthEvent>();
    }

    private void Start()
    {
        CallHealthEvent(0);

        player = GetComponent<Player>();
        enemy = GetComponent<Enemy>();

        if (player != null)
        {
            if (player.playerDetails.isImmuneAfterHit)
            {
                isImmunityAfterHit = true;
                immunityTime = player.playerDetails.hitImmunityTime;
                spriteRenderer = player.spriteRenderer;
            }
        }
        else if (enemy != null)
        {
            if (enemy.enemyDetails.isImmuneAfterHit)
            {
                isImmunityAfterHit = true;
                immunityTime = enemy.enemyDetails.hitImunityTime;
                spriteRenderer = enemy.spriteRenderers[0];
            }
        }

        if (enemy != null && enemy.enemyDetails.isHealthBarDisplayed && healthBar != null)
        {
            healthBar.EnableHealthBar();
        }
        else if (healthBar != null)
        {
            healthBar.DisableHealthBar();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        bool isRolling = false;

        if (player != null)
        {
            isRolling = player.playerControl.isPlayerRolling;
        }

        if (isDamageable && !isRolling)
        {
            currentHealth -= damageAmount;

            CallHealthEvent(damageAmount);
            PostHitImmunity();

            if (healthBar != null)
            {
                healthBar.SetHealthBarValue((float)currentHealth / (float)startingHealth);
            }
        }
    }

    private void CallHealthEvent(int damageAmount)
    {
        healthEvent.InvokeHealthChangedEvent(((float)currentHealth / (float)startingHealth), currentHealth, damageAmount);
    }

    private void PostHitImmunity()
    {
        if (gameObject.activeSelf == false)
        {
            return;
        }

        if (isImmunityAfterHit)
        {
            if (immunityCoroutine != null)
            {
                StopCoroutine(immunityCoroutine);
            }

            immunityCoroutine = StartCoroutine(PostHitImmunityRoutine(immunityTime, spriteRenderer));
        }
    }

    private IEnumerator PostHitImmunityRoutine(float immunityTime, SpriteRenderer spriteRenderer)
    {
        int iterations = Mathf.RoundToInt(immunityTime / spriteFlashInterval / 2f);

        isDamageable = false;

        while (iterations > 0)
        {
            spriteRenderer.color = Color.red;

            yield return new WaitForSeconds(spriteFlashInterval);

            spriteRenderer.color = Color.white;

            yield return new WaitForSeconds(spriteFlashInterval);

            iterations--;

            yield return null;
        }

        isDamageable = true;
    }

    public void SetStartingHealth(int value)
    {
        startingHealth = value;
        currentHealth = value;
    }

    public int GetStartingHealth()
    {
        return startingHealth;
    }
}