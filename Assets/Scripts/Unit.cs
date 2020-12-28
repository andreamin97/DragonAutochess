using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum Statuses
    {
        None,
        Snared
    }

    [Header("Base Unit Stats")] public float attackRange = 2f;

    public float _attackSpeed;
    public float _attackDamage;
    public float movementSpeed = 3f;

    public bool isActive;

    public bool isFighting;
    [SerializeField] public float armor;
    public float currentHealth;
    public float maxHealth;
    public float leech;

    public float _conditionMaxDuration;
    public float _conditionDuration;

    public string unitName;

    public Statuses currentStatus = Statuses.None;
    protected float _mRes;
    protected BoardManager boardManager;

    protected MeshFilter meshFilter;

    protected virtual void Awake()
    {
        boardManager = FindObjectOfType<BoardManager>();
        meshFilter = GetComponent<MeshFilter>();
    }

    public virtual void TakeDamage(float damage)
    {
        if (damage > 0f)
            currentHealth -= Mathf.Clamp(damage - armor, 1f, maxHealth);
        else if (damage < 0f) currentHealth = Mathf.Clamp(currentHealth -= damage, 0f, maxHealth);
    }
}