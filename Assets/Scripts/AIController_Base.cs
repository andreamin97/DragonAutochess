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
}