using UnityEngine;

[CreateAssetMenu(fileName = "CurrentPlayer_", menuName = "Scriptable Objects/Dungeon/Current Player")]
public class CurrentPlayerSO : ScriptableObject
{
    public PlayerDetailsSO playerDetails;
    public string playerName;
}