using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerUnit : Unit
{
    public BaseUnit UnitClass;
    public int unitCost;
    public GameObject levelUI;

    [HideInInspector] public int unitLevel;
    private AIController _aiController;
    [SerializeField]private NavMeshAgent _navMeshAgent;

    private PlayerController _playerController;
    private bool canBeSwapped = true;

    private GameObject currentTile;
    private Text levelText;
    private Image unitThumbnail;
    public bool CanBeSold = false;
    public GameObject vfx;

    private UnitInspector _unitInspector;

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

        if (!_playerController.isFighting)
        {
            _playerController.isDragging = true;
            var x = Mathf.Lerp(transform.position.x, _playerController.selectedTile.transform.position.x, 0.75f);
            var z = Mathf.Lerp(transform.position.z, _playerController.selectedTile.transform.position.z, 0.75f);
            var y = _playerController.selectedTile.transform.position.y + 1f;
            transform.position = new Vector3(x, y, z);
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
        boardManager.SetUnitAtSlot(this.gameObject, _playerController.selectedTile);

        if (boardManager.IsUnitBenched(this.gameObject))
        {
            boardManager.fightingUnits.Remove(this.gameObject);
        }

        CanBeSold = false;
    }

    public void InitUnit()
    {
        unitName = UnitClass.Name;
        unitCost = UnitClass.Cost;
        meshFilter.mesh = UnitClass.Mesh;
        attackRange = UnitClass.AttackRange;
        _attackDamage = UnitClass.AttackDamage;
        _attackSpeed = UnitClass.AttackSpeed;
        maxHealth = UnitClass.Health;
        currentHealth = maxHealth;
        armor = UnitClass.Armor;
        _navMeshAgent.speed = UnitClass.MovementSpeed;
        _aiController.profile = UnitClass._aiProfile;
        _aiController.Range = UnitClass.range;
        
        //create ability behaviour based on the class
        switch (unitName)
        {
            case "Cleric":
                _aiController.ability1 = gameObject.AddComponent<Heal>();
                _aiController.ability1.InitAbility("Heal", "Heal a friendly target for 10HP/lvl", 5f, 5f);
                break;
            case "Druid":
                _aiController.ability1 = gameObject.AddComponent<SproutingRoots>();
                _aiController.ability1.InitAbility("Sprouting Roots", "Snare an enemy for 5 seconds, impeding them to move and attack.", 4f, 4f);
                break;
            case "Sorcerer":
                _aiController.ability1 = gameObject.AddComponent<Fireball_Ability>();
                _aiController.ability1.InitAbility("Fireball", "Deal 30 damage around the target", 5f, 5f);
                break;
            case "Warlock":
                _aiController.ability1 = gameObject.AddComponent<SapLife>();
                _aiController.ability1.InitAbility("Sap Life", "Deal twice your attack damage to your target and heal half that amount" , 4f, 4f);
                break;
            case "Paladin":
                _aiController.ability1 = gameObject.AddComponent<Bastion>();
                _aiController.ability1.InitAbility("Bastion", "Snare self for 3seconds, but gain .5 armor/level" , 6f, 6f);
                break;
            case "Barbarian":
                _aiController.ability1 = gameObject.AddComponent<Enrage>();
                _aiController.ability1.InitAbility("Enrage", "Gain 1% leech for every 1% missing hp, up to 70%", 0f, 0f);
                break;
            case "Wizard":
                _aiController.ability1 = gameObject.AddComponent<Singularity>();
                _aiController.ability1.InitAbility("Singularity", "After a 3 seconds delay, pull all teleport all enemies in range to the center of the map and deal them 7 damage", 6f, 6f);
                break;
            case "Rogue":
                _aiController.ability1 = gameObject.AddComponent<Assasinate>();
                _aiController.ability1.InitAbility("Assasinate", "Teleport to the lowest hp enemy and deal 5(+5/lvl) damage to it", 6f, 6f);
                break;
            case "Fighter":
                _aiController.ability1 = gameObject.AddComponent<BladeSpin>();
                _aiController.ability1.InitAbility("Blade Spin", "Deal my AD*lvl to all units around me", 3f, 3f);
                break;
            case "Bard":
                _aiController.ability1 = gameObject.AddComponent<Inspiration>();
                _aiController.ability1.InitAbility("Inspiration", "Give all allies within a square from me at the beginning of the encounter 0.1AS/lvl", 0f, 0f);
                break;
            case "Ranger":
                _aiController.ability1 = gameObject.AddComponent<AnimalCompanion>();
                break;
            case "Monk":
                _aiController.ability1 = gameObject.AddComponent<ConcussiveFist>();
                _aiController.ability1.InitAbility("Concussive Fist", "Strike the target, knocking them backwards and briefly snaring them", 4f, 4f);
                break;}
        
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
        _attackDamage += UnitClass.ADPerLevel;
        _attackSpeed = Mathf.Clamp( _attackSpeed - UnitClass.ASPerLevel, 0.005f, 10f);
        armor += UnitClass.ArmorPerLevel;
        _mRes += UnitClass.MrPerLevel;
        
        // Instantiate(vfx, this.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
        
        
    }
    
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        
        if (currentHealth <= 0)
        {
            boardManager.fightingUnits.Remove(gameObject);

            foreach (var unit in boardManager.enemyFightingUnits)
            {
                var _ai = unit.GetComponent<EnemyAIController>();

                if (_ai.Target == this)
                {
                    _ai.Target = null;
                }
            }
            
            boardManager.RemoveUnit(this.gameObject, false);
        }
    }

    public void AddArmor(float value)
    {
        armor += value;
    }
}