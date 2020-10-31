using UnityEngine;

public class EnemyUnit : Unit
{
    public BaseEnemy enemyClass;
    private EnemyAIController _aiController;
    private BoardManager _boardManager;
    protected override void Awake()
    {
        base.Awake();
        _aiController = GetComponent<EnemyAIController>();
        _boardManager = FindObjectOfType<BoardManager>();
        meshFilter = GetComponent<MeshFilter>();
    }

    public void InitUnit()
    {
        meshFilter.mesh = enemyClass.Mesh;
        maxHealth = enemyClass.Health + enemyClass.HpPerLevel*_boardManager.Stage;
        currentHealth = maxHealth;
        armor = enemyClass.Armor + enemyClass.ArmorPerLevel*_boardManager.Stage;
        _attackDamage = enemyClass.AttackDamage + enemyClass.ADPerLevel*_boardManager.Stage;
        _attackSpeed = Mathf.Clamp(enemyClass.AttackSpeed - enemyClass.ASPerLevel*_boardManager.Stage, 0.05f, 10f);
        attackRange = enemyClass.AttackRange;
        _aiController.profile = enemyClass._aiProfile;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (currentHealth - damage <= 0)
        {
            boardManager.enemyFightingUnits.Remove(gameObject);
            Destroy(this.gameObject);
        }
    }
}