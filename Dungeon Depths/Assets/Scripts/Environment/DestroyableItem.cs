using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class DestroyableItem : MonoBehaviour
{
    [SerializeField] private int startingHealthAmount = 1;
    [SerializeField] private Sprite desroyedSprite;
    [SerializeField] private SoundEffectSO destroySound;

    private Health health;
    private HealthEvent healthEvent;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    private ReceiveContactDamage receiveContactDamage;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        healthEvent = GetComponent<HealthEvent>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        receiveContactDamage = GetComponent<ReceiveContactDamage>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        health.SetStartingHealth(startingHealthAmount);
    }

    private void OnEnable()
    {
        healthEvent.OnHealthChanged += HealthEvent_OnHealthChanged;
    }

    private void OnDisable()
    {
        healthEvent.OnHealthChanged -= HealthEvent_OnHealthChanged;
    }

    private void HealthEvent_OnHealthChanged(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
    {
        if (healthEventArgs.healthAmount <= 0)
        {
            StartCoroutine(PlayerAnimation());
        }
    }

    private IEnumerator PlayerAnimation()
    {
        Destroy(boxCollider2D);

        if (destroySound != null)
        {
            SoundEffectManager.Instance.PlaySoundEffect(destroySound);
        }

        animator.SetBool(Settings.destroy, true);

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(Settings.stateDestroyed))
        {
            yield return null;
        }

        Destroy(animator);

        yield return new WaitForSeconds(0.001f);
        spriteRenderer.sprite = desroyedSprite;

        Destroy(receiveContactDamage);
        Destroy(health);
        Destroy(healthEvent);
        Destroy(this);
    }
}