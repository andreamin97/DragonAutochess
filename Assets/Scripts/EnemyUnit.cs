using UnityEngine;
using UnityEngine.AI;

public class EnemyUnit : Unit
{
    public BaseEnemy enemyClass;
    private EnemyAIController _aiController;
    private BoardManager _boardManager;
    private NavMeshAgent _navMeshAgent;

    private UnitInspector _unitInspector;

    protected override void Awake()
    {
        base.Awake();
        _aiController = GetComponent<EnemyAIController>();
        _boardManager = FindObjectOfType<BoardManager>();
        meshFilter = GetComponent<MeshFilter>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _unitInspector = FindObjectOfType<UnitInspector>();
        unitLevel = _boardManager.Stage;
    }

    private void OnMouseDown()
    {
        var _pc = FindObjectOfType<PlayerController>();
        _pc.selectedUnit = gameObject;
        _unitInspector.Show();
    }

    public void InitUnit()
    {
        meshFilter.mesh = enemyClass.Mesh;
        maxHealth = enemyClass.Health + enemyClass.HpPerLevel * _boardManager.Stage;
        currentHealth = maxHealth;
        armor = enemyClass.Armor + enemyClass.ArmorPerLevel * _boardManager.Stage;
        _attackDamageMin = enemyClass.AttackDamage + enemyClass.ADPerLevel * _boardManager.Stage;
        _attackDamageMax = _attackDamageMin + 5f;
        _attackSpeed = Mathf.Clamp(enemyClass.AttackSpeed - enemyClass.ASPerLevel * _boardManager.Stage, 0.05f, 10f);
        attackRange = enemyClass.AttackRange;
        _aiController.profile = enemyClass._aiProfile;
        _navMeshAgent.speed = enemyClass.MovementSpeed;
        _aiController.Range = enemyClass.range;
    }

    public override void TakeDamage(float damage, float percent)
    {
        base.TakeDamage(damage, percent);
    
        if (currentHealth <= 0)
        {
            boardManager.enemyFightingUnits.Remove(gameObject);
    
            foreach (var unit in boardManager.fightingUnits)
            {
                var _ai = unit.GetComponent<AIController_Base>();
    
                if (_ai.target == this) _ai.ResetTarget();
            }
            
            _boardManager.RemoveUnit(this.gameObject, false, true);
        }
    }
}