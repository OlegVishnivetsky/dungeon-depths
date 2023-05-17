using UnityEngine;

public class DealContactDamage : MonoBehaviour
{
    [SerializeField] private int contactDamageAmount;
    [SerializeField] private LayerMask layerMask;

    private bool isColliding = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isColliding)
        {
            return;
        }

        ContactDamage(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isColliding)
        {
            return;
        }

        ContactDamage(other);
    }

    private void ContactDamage(Collider2D other)
    {
        int collisionObjectLayerMask = (1 << other.gameObject.layer);

        if ((layerMask.value & collisionObjectLayerMask) == 0)
        {
            return;
        }

        ReceiveContactDamage receiveContactDamage = other.gameObject.GetComponent<ReceiveContactDamage>();

        if (receiveContactDamage != null)
        {
            isColliding = true;

            Invoke("ResetContsctCollision", Settings.contactDamageCollisionResetDelay);
            receiveContactDamage.TakeContactDamage(contactDamageAmount);
        }
    }

    private void ResetContsctCollision()
    {
        isColliding = false;
    }
}