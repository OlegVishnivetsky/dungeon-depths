using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDetails_", menuName = "Scriptable Objects/Enemy/Enemy Details")]
public class EnemyDetailsSO : ScriptableObject
{
    [Header("ENEMY BASE DETAILS")]
    public string enemyName;
    public GameObject enemyPrefab;
}