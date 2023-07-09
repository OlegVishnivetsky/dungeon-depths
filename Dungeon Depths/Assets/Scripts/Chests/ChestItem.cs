using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(MaterializeEffect))]
public class ChestItem : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private TextMeshPro textTMP;
    private MaterializeEffect materializeEffect;

    [HideInInspector] public bool isMaterialized = false;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        textTMP = GetComponentInChildren<TextMeshPro>();
        materializeEffect = GetComponent<MaterializeEffect>();
    }

    public void Initialize(Sprite sprite, string text, Vector3 spawnPosition, Color materializeColor)
    {
        spriteRenderer.sprite = sprite;
        transform.position = spawnPosition;
        StartCoroutine(MaterializeItem(materializeColor, text));
    }

    private IEnumerator MaterializeItem(Color materializeColor, string text)
    {
        SpriteRenderer[] spriteRenderers = new SpriteRenderer[] {spriteRenderer };

        yield return StartCoroutine(materializeEffect.MaterializeRoutine(GameResources.Instance.materializeShader,
            materializeColor, 1f, spriteRenderers, GameResources.Instance.litMaterial));

        isMaterialized = true;
        textTMP.text = text;
    }
}