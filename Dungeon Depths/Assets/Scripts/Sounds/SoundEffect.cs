using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioSource))]
public class SoundEffect : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    private void OnDisable()
    {
        audioSource.Stop();
    }

    public void SetSound(SoundEffectSO soundEffect)
    {
        audioSource.pitch = Random.Range(soundEffect.soundEffectPitchRandomVariationMin, 
            soundEffect.soindEffectPitchRandomVariationMax);
        audioSource.volume = soundEffect.soundEffectVolume;
        audioSource.clip = soundEffect.soundEffectClip;
    }
}