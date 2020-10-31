using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Base Unit Stats")] public float attackRange = 2f;

    public float _attackSpeed;
    public float _attackDamage;

    public bool isActive;

    public bool isFighting;
    protected float armor;
    protected float _mRes;
    protected BoardManager boardManager;
    public float currentHealth;
    public float maxHealth;

    protected MeshFilter meshFilter;

    protected string unitName;

    protected virtual void Awake()
    {
        boardManager = FindObjectOfType<BoardManager>();
        meshFilter = GetComponent<MeshFilter>();
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage - armor;
    }
}