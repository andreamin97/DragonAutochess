using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public AIProfile profile;
    private BoardManager _boardManager;
    private NavMeshAgent _navMeshAgent;
    public EnemyUnit _target;
    private PlayerUnit _unit;
    private float distance = float.PositiveInfinity;
    private float nextAttack;
    private GoogleSheetsForUnity _sheetsForUnity;
    
    public Ability ability1;
    public float abilit1Cd;
    private bool isCasting = false;
    private bool hasCondition = false;
    
    private void Start()
    {
        _sheetsForUnity = FindObjectOfType<GoogleSheetsForUnity>();
        _boardManager = FindObjectOfType<BoardManager>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _unit = GetComponent<PlayerUnit>();
        abilit1Cd = ability1.coolDown;
    }

    private void Update()
    {
        if (!_navMeshAgent)
            _navMeshAgent = GetComponent<NavMeshAgent>();

        if (_unit.isActive)
        {
            //Acquire a target if don't have one and save the distance
            if (_target == null)
                _target = profile.AcquireTarget(_boardManager.EnemyList(), transform.position)
                    .GetComponent<EnemyUnit>();

            distance = Vector3.Distance(_target.transform.position, transform.position);
            
            //Always prioritize casting an ability if able, abilities use the unit attackspeed
            if (ability1 != null)
            {
                if ((ability1.currentCd <= 0f) || isCasting)
                {
                    isCasting = ability1.Cast(_navMeshAgent, _boardManager, this);
                }
            }
            
            
            //move to the target or attack based on distance 
            if (distance <= _unit.attackRange)
            {
                _navMeshAgent.SetDestination(transform.position);
                if (nextAttack <= 0f)
                {
                    Debug.Log("Attacking " + _target.name);
                    _target.TakeDamage(_unit._attackDamage);
                    nextAttack = _unit._attackSpeed;
                }
            }
            else if (distance > _unit.attackRange)
            {
                _navMeshAgent.SetDestination(_target.transform.position);
            }
            
            transform.LookAt(_target.transform.position);
            transform.Rotate(new Vector3(0f, -90f, 0f));
            
        }
        
        nextAttack -= Time.deltaTime;
        ability1.currentCd -= Time.deltaTime;
    }

    public void ResetUnit(Vector3 position)
    {
        _navMeshAgent.Warp(position);
    }

    public void ResetTarget()
    {
        _target = null;
    }

    public void SetCondition()
    {
        
    }
}