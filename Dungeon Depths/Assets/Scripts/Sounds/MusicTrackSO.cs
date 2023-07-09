using UnityEngine;

[CreateAssetMenu(fileName = "MusicTrack_", menuName = "Scriptable Objects/Sounds/MusicTrack")]
public class MusicTrackSO : ScriptableObject
{
    [Header("MUSIC TRACK DETAILS")]
    public string musicName;
    public AudioClip musicClip;
    [Range(0, 1)] public float musicVolume;
}