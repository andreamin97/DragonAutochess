using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.WSA;

public class AIController : MonoBehaviour
{
    enum State { Idle, Moving, Attacking };

    [SerializeField] private State _state = State.Idle;
    private BoardManager _boardManager;
    private NavMeshAgent _navMeshAgent;
    private Enemy _target;
    private Unit _unit;
    float distance = Single.PositiveInfinity;
    private float nextAttack = 0f;
        
    public AIProfile profile;

    private void Start()
    {
        _boardManager = FindObjectOfType<BoardManager>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _unit = GetComponent<Unit>();

    }

    private void Update()
    {
        if(!_navMeshAgent)
            _navMeshAgent = GetComponent<NavMeshAgent>();
        
        if (_unit.isActive)
        {
            if (_target == null)
            {
                _target = profile.AcquireTarget(_boardManager.EnemyList(), transform.position).GetComponent<Enemy>();
                Debug.Log("Targeted " + _target.name);
            }
            
            distance = Vector3.Distance(_target.transform.position, transform.position);

            if (distance <= _unit.attackRange)
            {
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

    public void Activate()
    {
        _state = State.Moving;
    }
}
