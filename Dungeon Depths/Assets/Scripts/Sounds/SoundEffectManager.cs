using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class SoundEffectManager : SingletonMonobehaviour<SoundEffectManager>
{
    public int soundsVolume = 0;

    private void Start()
    {
        SetSoundsVolume(soundsVolume);
    }

    public void PlaySoundEffect(SoundEffectSO soundEffect)
    {
        SoundEffect sound = (SoundEffect)PoolManager.Instance.ReuseComponent(soundEffect.soundPrefab, Vector3.zero,
            Quaternion.identity);

        sound.SetSound(soundEffect);
        sound.gameObject.SetActive(true);

        StartCoroutine(DisableSound(sound, soundEffect.soundEffectClip.length));
    }

    private IEnumerator DisableSound(SoundEffect sound, float clipDuration)
    {
        yield return new WaitForSeconds(clipDuration);

        sound.gameObject.SetActive(false);
    }

    private void SetSoundsVolume(int soundsVolume)
    {
        float muteDecibels = -80f;

        if (soundsVolume == 0f)
        {
            GameResources.Instance.soundsMasterMixerGroup.audioMixer.SetFloat("soundsVolume", muteDecibels);
        }
        else
        {
            GameResources.Instance.soundsMasterMixerGroup.audioMixer.SetFloat("soundsVolume",
                HelperUtilities.LinearToDecibels(soundsVolume));
        }
    }
}