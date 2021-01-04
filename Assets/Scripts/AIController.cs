using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class AIController : AIController_Base
{
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
                    if (abilityList != null)
                    {
                        for (var i = 0; i < abilityList.Count; i++)
                        {
                            if (abilityList[i].ability.currentCd <= 0f || abilityList[i].isCasting)
                            {
                                var ability = abilityList[i];
                                ability.isCasting = abilityList[i].ability.Cast(_navMeshAgent, _boardManager, this);
                                break;
                            }
                        }
                    }


                    //move to the target or attack based on distance 
                    if (target != null)
                    {
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
                        for (var i = 0; i < abilityList.Count; i++)
                        {
                            var ability = abilityList[i];
                            ability.ability.currentCd -= Time.deltaTime;
                        }
                    }

                    break;

                case Unit.Statuses.Snared:
                    _navMeshAgent.SetDestination(transform.position);

                    //Always prioritize casting an ability if able, abilities use the unit attackspeed
                    if (abilityList != null)
                    {
                        for (var i = 0; i < abilityList.Count; i++)
                        {
                            if (abilityList[i].ability.currentCd <= 0f || abilityList[i].isCasting)
                            {
                                var ability = abilityList[i];
                                ability.isCasting = abilityList[i].ability.Cast(_navMeshAgent, _boardManager, this);
                                break;
                            }
                        }
                    }

                    _conditionDuration -= Time.deltaTime;
                    _unit._conditionDuration = _conditionDuration;

                    if (_conditionDuration <= 0)
                    {
                        SetCondition(Unit.Statuses.None, 0f);
                    }

                    break;
            }
        }
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
        _unit._conditionMaxDuration = duration;
        _conditionDuration = duration;
        _unit.currentStatus = cond;
    }
}