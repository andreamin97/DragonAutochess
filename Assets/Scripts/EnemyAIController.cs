using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.WSA;

public class EnemyAIController : MonoBehaviour
{
    private BoardManager _boardManager;
    private NavMeshAgent _navMeshAgent;
    private PlayerUnit _target;
    private EnemyUnit _unit;
    float distance = Single.PositiveInfinity;
    private float nextAttack = 0f;
        
    public AIProfile profile;

    private void Start()
    { 
        _boardManager = FindObjectOfType<BoardManager>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _unit = GetComponent<EnemyUnit>();

    }

    private void Update()
    {
        
        
        if(!_navMeshAgent)
            _navMeshAgent = GetComponent<NavMeshAgent>();
        
        if (_unit.isActive)
        {
            Debug.Log("Im Active");
            if (_target == null)
            {
                _target = profile.AcquireTarget(_boardManager.PlayerUnitList(), transform.position).GetComponent<PlayerUnit>();
                if (_target == null)
                    _unit.isActive = false;
            }
            
            distance = Vector3.Distance(_target.transform.position, transform.position);
        
            
            if (distance <= _unit.attackRange)
            {
                Debug.Log("Im Walking");
                _navMeshAgent.SetDestination(transform.position);
                if (nextAttack <= 0f)
                {
                    Debug.Log("Attacking Unit");
                    _target.TakeDamage(_unit._attackDamage);
                    nextAttack = _unit._attackSpeed;
                }
            }
            else if (distance > _unit.attackRange)
            {
                _navMeshAgent.SetDestination(_target.transform.position);
            }
        }

        nextAttack -= Time.deltaTime;

    }
}
