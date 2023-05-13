using UnityEngine;

[CreateAssetMenu(fileName = "CurrentPlayer_", menuName = "Scriptable Objects/Player/Current Player")]
public class CurrentPlayerSO : ScriptableObject
{
    public PlayerDetailsSO playerDetails;
    public string playerName;
}