using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public BaseUnit UnitClass;
    
    private Image unitThumbnail;
    private string unitName;
    private int unitCost;
    public float attackRange = 2f;
    public float _attackSpeed;
    public float _attackDamage;
    private PlayerController _playerController;
    private MeshFilter meshFilter;
    private BoardManager boardManager;
    private GameObject currentTile;
    private bool canBeSwapped = true;
    public bool isActive = false;
    private NavMeshAgent _navMeshAgent;
    private AIController _aiController;
    private float maxHealth;
    private float currentHealth;
    private float armor;
    
    public bool isFighting = false;

    private void Awake()
    {
        _playerController = Camera.main.GetComponent<PlayerController>();
    }

    private void Start()
    {
        boardManager = FindObjectOfType<BoardManager>();
        meshFilter = GetComponent<MeshFilter>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _aiController = GetComponent<AIController>();
        
        Physics.IgnoreLayerCollision(9,9);
        
        InitUnit();
    }

    private void Update()
    {
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
                coll.GetComponent<Unit>().Swap(currentTile.transform.position);
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

    public void TakeDamage(float damage)
    {
        currentHealth -= (damage-armor);
        
        if(currentHealth<=0f)
            Destroy(this.gameObject);
    }
}
