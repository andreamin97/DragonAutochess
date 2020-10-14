using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerUnit : Unit
{
    public BaseUnit UnitClass;
    private Image unitThumbnail;
    public int unitCost;
    
    private GameObject currentTile;
    private bool canBeSwapped = true;

    private PlayerController _playerController;
    private NavMeshAgent _navMeshAgent;
    private AIController _aiController;
    public GameObject levelUI;
    private Text levelText;

    [HideInInspector] public int unitLevel=0;

    protected override void Awake()
    {
        base.Awake();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _playerController = Camera.main.GetComponent<PlayerController>();
        _aiController = GetComponent<AIController>();
        levelText = levelUI.GetComponent<Text>();
        Physics.IgnoreLayerCollision(9,9);
    }


    private void Start()
    {
        //InitUnit();
        
        LevelUp();
    }
    
    void Update()
    {
        if(levelText.text == "-1")
            LevelUp();
        
        //Set this unit as the current unit at the overlapping board tile
        if (!_playerController.isFighting)
        {
            Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 1.75f, Quaternion.identity, LayerMask.GetMask("Board"));
            foreach (Collider coll in colliders)
            {
                currentTile = coll.gameObject;
                boardManager.SetUnitAtSlot(this.gameObject, coll.gameObject);
            }
        }
    }
    
     private void OnMouseDown()
    {
        _playerController.selectedUnit = this.gameObject;
        canBeSwapped = false;
        _navMeshAgent.enabled = false;

    }

    private void OnMouseDrag()
    {
        _playerController.isDragging = true;
        float x = Mathf.Lerp(transform.position.x, _playerController.selectedTile.transform.position.x, 0.75f);
        float z = Mathf.Lerp(transform.position.z, _playerController.selectedTile.transform.position.z, 0.75f);
        float y = _playerController.selectedTile.transform.position.y + 1f;
        transform.position = new Vector3(x, y, z) ;
    }

    private void OnMouseUp()
    {
        if (_playerController.isDragging)
        {
            _playerController.isDragging = false;
            
            Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 1.75f, Quaternion.identity,
                LayerMask.GetMask("Minis"));
            foreach (Collider coll in colliders)
            {
                coll.GetComponent<PlayerUnit>().Swap(currentTile.transform.position);
            }
        }

        canBeSwapped = true;

        NavMeshHit navHit;
        if(NavMesh.SamplePosition(transform.position, out navHit, 1, -1))
        {
            transform.position = navHit.position;
            _navMeshAgent.enabled = true;
        }
    }

    public void InitUnit()
    {
        unitName = UnitClass.Name;
        meshFilter.mesh = UnitClass.Mesh;
        attackRange = UnitClass.AttackRange;
        _attackDamage = UnitClass.AttackDamage;
        _attackSpeed = UnitClass.AttackSpeed;
        maxHealth = UnitClass.Health;
        currentHealth = maxHealth;
        armor = UnitClass.Armor;
        _navMeshAgent.speed = UnitClass.MovementSpeed;
        _aiController.profile = UnitClass._aiProfile;
    }

    public void Swap(Vector3 location)
    {
        if(canBeSwapped)
            transform.position = location + Vector3.up;
    }

    public GameObject GetCurretTile()
    {
        return currentTile;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void LevelUp()
    {
        levelText.text = (++unitLevel).ToString();
    }

    public override void TakeDamage(float damage)
    {
        
        if(currentHealth-damage <=0 )
            boardManager.fightingUnits.Remove(this.gameObject);
        base.TakeDamage(damage);
    }
}
