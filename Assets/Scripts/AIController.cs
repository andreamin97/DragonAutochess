﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class AIController : MonoBehaviour
{
    public AIProfile profile;
    private BoardManager _boardManager;
    private NavMeshAgent _navMeshAgent;
    [FormerlySerializedAs("_target")] public EnemyUnit target;
    private PlayerUnit _unit;
    private float _distance = float.PositiveInfinity;
    private float _nextAttack;
    private GoogleSheetsForUnity _sheetsForUnity;
    
    public Ability ability1;
    public float abilit1Cd;
    private bool _isCasting = false;
    private Unit.Statuses _condition = Unit.Statuses.None;
    private float _conditionDuration;
    
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
            switch (_condition)
            {
                case Unit.Statuses.None:
                    
                    //Acquire a target if don't have one and save the distance
                    if (target == null)
                        target = profile.AcquireTarget(_boardManager.EnemyList(), transform.position)
                            .GetComponent<EnemyUnit>();
            
                    _distance = Vector3.Distance(target.transform.position, transform.position);
                        
                    //Always prioritize casting an ability if able, abilities use the unit attackspeed
                    if (ability1 != null)
                    {
                        if ((ability1.currentCd <= 0f) || _isCasting)
                        {
                            _isCasting = ability1.Cast(_navMeshAgent, _boardManager, this);
                        }
                    }
                        
                        
                    //move to the target or attack based on distance 
                    if (_distance <= _unit.attackRange)
                    {
                        //Stop moving
                        _navMeshAgent.SetDestination(transform.position);
                        if (_nextAttack <= 0f)
                        {
                            AttackTarget(target);
                        }
                    }
                    else if (_distance > _unit.attackRange)
                    {
                        _navMeshAgent.SetDestination(target.transform.position);
                    }
                        
                    transform.LookAt(target.transform.position);
                    transform.Rotate(new Vector3(0f, -90f, 0f));
                        
                    _nextAttack -= Time.deltaTime;
                    ability1.currentCd -= Time.deltaTime;        
                    
                    break;        
                
                case Unit.Statuses.Snared:
                    _navMeshAgent.SetDestination(transform.position);
                    
                    //Always prioritize casting an ability if able, abilities use the unit attackspeed
                    if (ability1 != null)
                    {
                        if ((ability1.currentCd <= 0f) || _isCasting)
                        {
                            _isCasting = ability1.Cast(_navMeshAgent, _boardManager, this);
                        }
                    }
                
                    if (_distance <= _unit.attackRange && _nextAttack <= 0f)
                    {
                        AttackTarget(target);
                    }
                                    
                    _conditionDuration -= Time.deltaTime;
                    if (_conditionDuration <= 0)
                    {
                        _condition = Unit.Statuses.None;
                        _conditionDuration = 0f;
                    }
                    break;
            }
                   
        }
            
    }

    private void AttackTarget(EnemyUnit target)
    {
       
        target.TakeDamage(_unit._attackDamage);
        
        if(_unit.leech > 0f)
            _unit.TakeDamage(-(_unit._attackDamage*_unit.leech));
        
        _nextAttack = _unit._attackSpeed;
    }

    public void ResetUnit(Vector3 position)
    {
        _navMeshAgent.Warp(position);
    }

    public void ResetTarget()
    {
        target = null;
    }

    public void SetCondition(Unit.Statuses cond, float duration)
    {
        _condition = cond;
        _conditionDuration = duration;
    }
}