using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour
{
    [SerializeField] private BoxCollider2D doorCollider;
    private BoxCollider2D doorTrigger;

    private Animator animator;

    [HideInInspector] public bool isBossRoomDoor = false;
    private bool isOpen = false;
    private bool isPreviouslyOpened = false;
     
    private void Awake()
    {
        doorCollider.enabled = false;

        doorTrigger = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        animator.SetBool(Settings.openDoor, isOpen);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            isPreviouslyOpened = true;

            doorCollider.enabled = false;
            doorTrigger.enabled = false;

            animator.SetBool(Settings.openDoor, true);
        }
    }

    public void LockDoor()
    {
        isOpen = false;

        doorCollider.enabled = true;
        doorTrigger.enabled = false;

        animator.SetBool(Settings.openDoor, false);
    }

    public void UnlockDoor()
    {
        doorCollider.enabled = false;
        doorTrigger.enabled = true;

        if (isPreviouslyOpened)
        {
            isOpen = false;
            OpenDoor();
        }
    }

    #region Validation

#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(doorCollider), doorCollider);
    }

#endif

    #endregion Validation
}