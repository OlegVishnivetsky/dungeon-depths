using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class MusicManager : SingletonMonobehaviour<MusicManager>
{
    private AudioSource musicAudioSource;
    private AudioClip currentAudioClip;

    private Coroutine fadeOutMusicRoutine;
    private Coroutine fadeInMusicRoutine;

    public int musicVolume = 10;

    protected override void Awake()
    {
        base.Awake();
        musicAudioSource = GetComponent<AudioSource>();

        GameResources.Instance.musicOffSnapshot.TransitionTo(0);
    }

    private void Start()
    {
        SetMusicVolume(musicVolume);
    }

    private void SetMusicVolume(int musicVolume)
    {
        float muteDecibels = -80f;

        if (musicVolume == 0)
        {
            GameResources.Instance.musicMixerGroup.audioMixer.SetFloat("musicVolume", muteDecibels);
        }
        else
        {
            GameResources.Instance.musicMixerGroup.audioMixer.SetFloat("musicVolume", HelperUtilities.LinearToDecibels(musicVolume));
        }
    }

    public void PlayMusic(MusicTrackSO musicTrack, float fadeOutTime = Settings.musicFadeOutTime, float fadeInTime = Settings.musicFadeInTime)
    {
        StartCoroutine(PlayMusicRoutine(musicTrack, fadeOutTime, fadeInTime));
    }

    private IEnumerator PlayMusicRoutine(MusicTrackSO musicTrack, float fadeOutTime, float fadeInTime)
    {
        if (fadeOutMusicRoutine != null)
        {
            StopCoroutine(fadeOutMusicRoutine);
        }

        if (fadeInMusicRoutine != null)
        {
            StopCoroutine(fadeInMusicRoutine);
        }

        if (musicTrack.musicClip != currentAudioClip)
        {
            currentAudioClip = musicTrack.musicClip;

            yield return fadeOutMusicRoutine = StartCoroutine(FadeOutMusic(fadeOutTime));
            yield return fadeInMusicRoutine = StartCoroutine(FadeInMusic(musicTrack, fadeInTime));
        }

        yield return null;
    }

    private IEnumerator FadeInMusic(MusicTrackSO musicTrack, float fadeInTime)
    {
        musicAudioSource.clip = musicTrack.musicClip;
        musicAudioSource.volume = musicTrack.musicVolume;
        musicAudioSource.Play();

        GameResources.Instance.musicFullSnapshot.TransitionTo(fadeInTime);

        yield return new WaitForSeconds(fadeInTime);
    }

    private IEnumerator FadeOutMusic(float fadeOutTime)
    {
        GameResources.Instance.musiLowSnapshot.TransitionTo(fadeOutTime);

        yield return new WaitForSeconds(fadeOutTime);
    }
}