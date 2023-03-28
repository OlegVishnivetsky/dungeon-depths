using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class DoorLightingControl : MonoBehaviour
{
    [SerializeField] private Door door;

    [HideInInspector] public bool isLit = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        FadeInDoor(door);
    }

    public void FadeInDoor(Door door)
    {
        Material material = new Material(GameResources.Instance.variableLitShader);

        if (!isLit)
        {
            SpriteRenderer[] spriteRenderers = GetComponentsInParent<SpriteRenderer>();

            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                StartCoroutine(FadeInDoorRoutine(spriteRenderer, material));
            }

            isLit = true;
        }
    }

    private IEnumerator FadeInDoorRoutine(SpriteRenderer spriteRenderer, Material material)
    {
        spriteRenderer.material = material;

        for (float i = 0.05f; i <= 1f; i += Time.deltaTime / Settings.fadeInTime)
        {
            material.SetFloat("Alpha_Slider", i);
            yield return null;
        }

        spriteRenderer.material = GameResources.Instance.litMaterial;
    }
}