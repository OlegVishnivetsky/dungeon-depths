using UnityEngine;

[CreateAssetMenu(fileName = "SoundEffect_", menuName = "Scriptable Objects/Sounds/Sound Effect")]
public class SoundEffectSO : ScriptableObject
{
    [Header("SOUND EFFECT DETAILS")]
    public string soundEffectName;

    public GameObject soundPrefab;
    public AudioClip soundEffectClip;

    [Range(0.1f, 1.5f)]
    public float soundEffectPitchRandomVariationMin = 0.8f;
    [Range(0.1f, 1.5f)]
    public float soindEffectPitchRandomVariationMax = 1.2f;

    [Range(0f, 1f)]
    public float soundEffectVolume = 1f;
}