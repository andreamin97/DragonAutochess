﻿using System.Diagnostics;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

public class EnemyAIController : MonoBehaviour
{
    public AIProfile profile;
    private BoardManager _boardManager;
    private NavMeshAgent _navMeshAgent;
    private PlayerUnit _target;
    private EnemyUnit _unit;
    private float distance = float.PositiveInfinity;
    private float nextAttack;
    private Unit.Statuses condition = Unit.Statuses.None;
    private float conditionDuration;

    private void Start()
    {
        _boardManager = FindObjectOfType<BoardManager>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _unit = GetComponent<EnemyUnit>();
    }

    private void Update()
    {
        if (!_navMeshAgent)
            _navMeshAgent = GetComponent<NavMeshAgent>();

        if (_unit.isActive)
        {
            switch (condition)
            {
                case Unit.Statuses.None:
                    if (_target == null)
                       _target = profile.AcquireTarget(_boardManager.PlayerFightingUnitList(), transform.position)
                           .GetComponent<PlayerUnit>();
            
                    distance = Vector3.Distance(_target.transform.position, transform.position);
            
                    if (distance <= _unit.attackRange)
                    {
                        _navMeshAgent.SetDestination(transform.position);
                        if (nextAttack <= 0f)
                        {
                            _target.TakeDamage(_unit._attackDamage);
                            nextAttack = _unit._attackSpeed;
                        }
                    }
                    else if (distance > _unit.attackRange)
                    {
                        _navMeshAgent.SetDestination(_target.transform.position);
                    }        
                    break;
                
                case Unit.Statuses.Snared:
                    _navMeshAgent.SetDestination(transform.position);

                    if (distance <= _unit.attackRange && nextAttack <= 0f)
                    {
                        _target.TakeDamage(_unit._attackDamage);
                        nextAttack = _unit._attackSpeed;
                    }
                    
                    conditionDuration -= Time.deltaTime;
                    if (conditionDuration <= 0)
                    {
                        condition = Unit.Statuses.None;
                        conditionDuration = 0f;
                    }
                    break;
            }
            
            
        }

        nextAttack -= Time.deltaTime;
    }

    public void SetCondition(Unit.Statuses cond, float duration)
    {
        condition = cond;
        conditionDuration = duration;
    }
    
}