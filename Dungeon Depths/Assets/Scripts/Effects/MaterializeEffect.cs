using System.Collections;
using UnityEngine;

public class MaterializeEffect : MonoBehaviour
{
    public IEnumerator MaterializeRoutine(Shader materializeShader, Color materializeColor, float materializeTime, 
        SpriteRenderer[] spriteRendererArray, Material normalMaterial)
    {
        Material materializeMaterial = new Material(materializeShader);

        materializeMaterial.SetColor("_EmissionColor", materializeColor);

        SetSpriteRendererArray(spriteRendererArray, materializeMaterial);

        float dissolveAmount = 0;

        while (dissolveAmount < 1)
        {
            dissolveAmount += Time.deltaTime / materializeTime;
            materializeMaterial.SetFloat("_DissolveAmount", dissolveAmount);

            yield return null;
        }

        SetSpriteRendererArray(spriteRendererArray, normalMaterial);
    }

    private void SetSpriteRendererArray(SpriteRenderer[] spriteRendererArray, Material material)
    {
        foreach (SpriteRenderer spriteRenderer in spriteRendererArray)
        {
            spriteRenderer.material = material;
        }
    }
}