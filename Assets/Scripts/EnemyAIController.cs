﻿using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : AIController_Base
{
    public AIProfile profile;
    private BoardManager _boardManager;
    private NavMeshAgent _navMeshAgent;
    private PlayerUnit _target;

    public PlayerUnit Target
    {
        get => _target;
        set => _target = value;
    }

    private EnemyUnit _unit;
    private float distance = float.PositiveInfinity;
    private float nextAttack;
    private Unit.Statuses _condition = Unit.Statuses.None;
    private float _conditionDuration;

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
            switch (_condition)
            {
                case Unit.Statuses.None:
                    if (_target == null)
                        _target = profile.AcquireTarget(_boardManager.PlayerFightingUnitList(), transform.position)
                            .GetComponent<PlayerUnit>();

                    if (_target == null)
                    {
                        FindObjectOfType<PlayerController>().isFighting = false;
                        _unit.isActive = false;
                    }

                    distance = Vector3.Distance(_target.transform.position, transform.position);

                    if (distance <= _unit.attackRange)
                    {
                        _navMeshAgent.SetDestination(transform.position);
                        if (nextAttack <= 0f)
                        {
                            AttackTarget(_target);
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

                    _conditionDuration -= Time.deltaTime;
                    _unit._conditionDuration = _conditionDuration;
                    if (_conditionDuration <= 0)
                    {
                        SetCondition(Unit.Statuses.None, 0f);
                    }

                    break;
            }


        }

        nextAttack -= Time.deltaTime;
    }

    public void SetCondition(Unit.Statuses cond, float duration)
    {
        _condition = cond;
        _unit._conditionMaxDuration = duration;
        _conditionDuration = duration;
        _unit.currentStatus = cond;
    }

    public Unit.Statuses GetCondition()
    {
        return _condition;
    }

    private void AttackTarget(PlayerUnit target)
    {
        switch (Range)
        {
            case BaseUnit.Range.Melee:
                target.TakeDamage(_unit._attackDamage);
                Instantiate(attackFX, target.transform);

                if (_unit.leech > 0f)
                    _unit.TakeDamage(-(_unit._attackDamage * _unit.leech));
                break;
            
            case BaseUnit.Range.Ranged:
                var projectile = (GameObject) Instantiate(Resources.Load("Projectile"), this.gameObject.transform);
                var projBase = projectile.GetComponent<Projectile_Base>();
                projBase.damage = _unit._attackDamage;
                projBase.target = target;
                projBase.VFX = attackFX;

                break;
        }

    }
}