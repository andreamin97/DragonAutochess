using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController_Base : MonoBehaviour
{
    public BaseUnit.Range Range;
    public AIProfile profile;
    public List<abilityStruct> abilityList;
    public Unit target;
    
    protected GameObject attackFX;
    protected Unit _unit;
    protected float _nextAttack;
    protected BoardManager _boardManager;
    protected float _distance = float.PositiveInfinity;
    protected Unit.Statuses _condition = Unit.Statuses.None;
    protected float _conditionDuration;
    protected NavMeshAgent _navMeshAgent;
    protected GoogleSheetsForUnity _sheetsForUnity;
    protected List<GameObject> alliedUnits;

    public List<GameObject> AlliedUnits => alliedUnits;

    public List<GameObject> EnemyUnits => enemyUnits;

    protected List<GameObject> enemyUnits;

    private void Awake()
    {
        _sheetsForUnity = FindObjectOfType<GoogleSheetsForUnity>();
        _boardManager = FindObjectOfType<BoardManager>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        
        attackFX = (GameObject) Resources.Load("VFX/FX_BloodExplosion_AB");
        abilityList = new List<abilityStruct>();

        var abilities = GetComponents<Ability>();

        foreach (var abi in abilities)
        {
            Debug.Log(abi.abilityName);
            var temp = new abilityStruct(abi, abi.coolDown, false);
            abilityList.Add(temp);
            _unit = GetComponent<Unit>();
        }
    }

    public struct abilityStruct
    {
        public Ability ability;
        public bool isCasting;

        public abilityStruct(Ability abi, float cD, bool casting)
        {
            ability = abi;
            isCasting = casting;
        }
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
                    {
                        InitializeLists();
                        target = profile.AcquireTarget(enemyUnits, transform.position)
                            .GetComponent<Unit>();
                    }

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
    
    public void ReduceAbilitiesCD(float percent, float value=.25f)
    {
        foreach (var ability in abilityList)
        {
            ability.ability.currentCd -= value*percent;
        }
    }

    protected void AttackTarget(Unit target)
    { 
        var damage = Random.Range(_unit._attackDamageMin, _unit._attackDamageMax);

        switch (Range)
        {
            case BaseUnit.Range.Melee:
                target.TakeDamage( damage, damage/_unit._attackDamageMax );
                Instantiate(attackFX, target.transform);
             
                if (_unit.leech > 0f)
                    _unit.TakeDamage(-damage * _unit.leech, 0);
                break;
            case BaseUnit.Range.Ranged:
                var projectile = (GameObject) Instantiate(Resources.Load("Projectile"), gameObject.transform);
                var projBase = projectile.GetComponent<Projectile_Base>();
                projBase.damage = damage;
                projBase.damagePercent = damage / _unit._attackDamageMax;
                projBase.target = target;
                projBase.VFX = attackFX;
             
                if (_unit.leech > 0f)
                    _unit.TakeDamage(-damage * _unit.leech, 0);
             
                break;
        }
        ReduceAbilitiesCD(damage/_unit._attackDamageMax);
        _nextAttack = _unit._attackSpeed;
    }
    
    public void ResetTarget()
    {
        target = null;
    }

    protected virtual void InitializeLists()
    {
        return;
    }
}