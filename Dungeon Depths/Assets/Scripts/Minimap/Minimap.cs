using Cinemachine;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] private GameObject miniMapPlayer;

    private Transform playerTransfrom;

    private void Start()
    {
        playerTransfrom = GameManager.Instance.GetPlayer().transform;

        CinemachineVirtualCamera cinemachineVirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = playerTransfrom;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = GameManager.Instance.GetPlayerMinimapIcon();
        }
    }

    private void Update()
    {
        if (playerTransfrom != null && miniMapPlayer != null)
        {
            miniMapPlayer.transform.position = playerTransfrom.position;
        }
    }
}