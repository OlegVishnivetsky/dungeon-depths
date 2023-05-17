using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Health))]
public class ReceiveContactDamage : MonoBehaviour
{
    [SerializeField] private int contactDamgeAmount;
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    public void TakeContactDamage(int damageAmount = 0)
    {
        if (contactDamgeAmount > 0)
        {
            damageAmount = contactDamgeAmount;
        }

        health.TakeDamage(damageAmount);
    }
}