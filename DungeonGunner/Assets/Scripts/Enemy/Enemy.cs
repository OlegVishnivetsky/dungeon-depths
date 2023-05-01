using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

[DisallowMultipleComponent]
[RequireComponent(typeof(SortingGroup))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(PolygonCollider2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyDetailsSO enemyDetails;

    private CircleCollider2D circleCollider2D;
    private PolygonCollider2D polygonCollider2D;

    [HideInInspector] public SpriteRenderer[] spriteRenderers;

    private void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }
}