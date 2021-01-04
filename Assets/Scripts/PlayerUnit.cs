using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerUnit : Unit
{
    public BaseUnit UnitClass;
    public int unitCost;
    public GameObject levelUI;

    [HideInInspector] public int unitLevel;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    public bool CanBeSold;
    public GameObject vfx;
    private AIController _aiController;

    private PlayerController _playerController;

    private UnitInspector _unitInspector;
    private bool canBeSwapped = true;

    private GameObject currentTile;
    private Text levelText;
    private Image unitThumbnail;

    protected override void Awake()
    {
        base.Awake();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _playerController = Camera.main.GetComponent<PlayerController>();
        _aiController = GetComponent<AIController>();
        levelText = levelUI.GetComponent<Text>();
        Physics.IgnoreLayerCollision(9, 9);
        _unitInspector = FindObjectOfType<UnitInspector>();
    }


    private void Start()
    {
        //InitUnit();

        LevelUp();
    }

    private void Update()
    {
        if (levelText.text == "-1")
            LevelUp();

        //Set this unit as the current unit at the overlapping board tile
        if (!_playerController.isFighting)
        {
            var colliders = Physics.OverlapBox(transform.position, transform.localScale / 1.75f, Quaternion.identity,
                LayerMask.GetMask("Board"));
            foreach (var coll in colliders)
            {
                currentTile = coll.gameObject;
                boardManager.SetUnitAtSlot(gameObject, coll.gameObject);
            }
        }
    }

    private void OnMouseDown()
    {
        _playerController.selectedUnit = gameObject;

        _unitInspector.Show();
        if (!_playerController.isFighting)
        {
            canBeSwapped = false;
            _navMeshAgent.enabled = false;
            CanBeSold = true;
        }
    }

    private void OnMouseDrag()
    {
        if (!_playerController.isFighting && _playerController.isDragging)
        {
            var x = Mathf.Lerp(transform.position.x, _playerController.selectedTile.transform.position.x, 0.5f);
            var z = Mathf.Lerp(transform.position.z, _playerController.selectedTile.transform.position.z, 0.5f);
            var y = _playerController.selectedTile.transform.position.y + 1f;
            transform.position = new Vector3(x, y, z);
        }
        else if (!_playerController.isFighting && !_playerController.isDragging)
        {
            _playerController.isDragging = true;
        }
    }

    private void OnMouseUp()
    {
        if (_playerController.isDragging)
        {
            _playerController.isDragging = false;

            var colliders = Physics.OverlapBox(transform.position, transform.localScale / 1.75f, Quaternion.identity,
                LayerMask.GetMask("Minis"));
            foreach (var coll in colliders) coll.GetComponent<PlayerUnit>().Swap(currentTile.transform.position);
        }

        canBeSwapped = true;

        NavMeshHit navHit;
        if (NavMesh.SamplePosition(transform.position, out navHit, 1, -1))
        {
            transform.position = navHit.position;
            _navMeshAgent.enabled = true;
        }

        boardManager.SetUnitAtSlot(null, currentTile);
        boardManager.SetUnitAtSlot(gameObject, _playerController.selectedTile);

        if (boardManager.IsUnitBenched(gameObject)) boardManager.fightingUnits.Remove(gameObject);

        CanBeSold = false;
    }

    public void InitUnit()
    {
        unitName = UnitClass.Name;
        unitCost = UnitClass.Cost;
        meshFilter.mesh = UnitClass.Mesh;
        attackRange = UnitClass.AttackRange;
        _attackDamageMin = UnitClass.AttackDamage;
        _attackDamageMax = _attackDamageMin + 5f;
        _attackSpeed = UnitClass.AttackSpeed;
        maxHealth = UnitClass.Health;
        currentHealth = maxHealth;
        armor = UnitClass.Armor;
        _navMeshAgent.speed = UnitClass.MovementSpeed;
        _aiController.profile = UnitClass._aiProfile;
        _aiController.Range = UnitClass.range;
    }

    public void Swap(Vector3 location)
    {
        if (canBeSwapped)
            transform.position = location + Vector3.up;
    }

    public GameObject GetCurretTile()
    {
        return currentTile;
    }

    public void LevelUp()
    {
        levelText.text = (++unitLevel).ToString();
        maxHealth += UnitClass.HpPerLevel;
        currentHealth = maxHealth;
        _attackDamageMin += UnitClass.ADPerLevel;
        _attackDamageMax += UnitClass.ADPerLevel;
        _attackSpeed = Mathf.Clamp(_attackSpeed - UnitClass.ASPerLevel, 0.005f, 10f);
        armor += UnitClass.ArmorPerLevel;
        _mRes += UnitClass.MrPerLevel;

        // Instantiate(vfx, this.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
    }

    public override void TakeDamage(float damage, float percent)
    {
        base.TakeDamage(damage, percent);

        if (currentHealth <= 0)
        {
            boardManager.fightingUnits.Remove(gameObject);

            foreach (var unit in boardManager.enemyFightingUnits)
            {
                var _ai = unit.GetComponent<EnemyAIController>();

                if (_ai.target == this) _ai.target = null;
            }

            boardManager.RemoveUnit(gameObject, false);
        }
    }

    public void AddArmor(float value)
    {
        armor += value;
    }
}