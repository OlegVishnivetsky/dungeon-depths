using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDetails_", menuName = "Scriptable Objects/Enemy/Enemy Details")]
public class EnemyDetailsSO : ScriptableObject
{
    [Header("ENEMY BASE DETAILS")]
    public string enemyName;
    public GameObject enemyPrefab;
    public float chaseDistance = 50f;

    [Space(10)]
    [Header("ENEMY MATERIAL")]
    public Material enemyStandartMaterial;

    [Space(10)]
    [Header("ENEMY MATERIALIZE SETTINGS")]
    public float enemyMaterializeTime;
    public Shader enemyMaterializeShader;
    [ColorUsage(true, true)] public Color enemyMaterializeColor;
}