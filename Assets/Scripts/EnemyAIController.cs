using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : AIController_Base
{
    private void Update()
    {
        if (!_navMeshAgent)
            _navMeshAgent = GetComponent<NavMeshAgent>();

        if (_unit.isActive)
            switch (_condition)
            {
                case Unit.Statuses.None:
                    if (target == null)
                        target = profile.AcquireTarget(_boardManager.PlayerFightingUnitList(), transform.position)
                            .GetComponent<PlayerUnit>();

                    if (target == null)
                    {
                        FindObjectOfType<PlayerController>().isFighting = false;
                        _unit.isActive = false;
                    }

                    _distance = Vector3.Distance(target.transform.position, transform.position);

                    if (_distance <= _unit.attackRange)
                    {
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
                    break;

                case Unit.Statuses.Snared:
                    _navMeshAgent.SetDestination(transform.position);

                    _conditionDuration -= Time.deltaTime;
                    _unit._conditionDuration = _conditionDuration;
                    if (_conditionDuration <= 0) SetCondition(Unit.Statuses.None, 0f);

                    break;
            }

        _nextAttack -= Time.deltaTime;
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

}