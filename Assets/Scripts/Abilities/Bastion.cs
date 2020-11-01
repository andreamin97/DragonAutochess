using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bastion : Ability 
{
    private float _cd = 8f;
    private float _duration = 3f;
    [SerializeField]private float _time = 0f;
    private bool isCasting = false;
    private PlayerUnit _unit = null;
    public override bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController controller)
    {
        if (_unit == null)
            _unit = controller.GetComponent<PlayerUnit>();
        
        if (!isCasting)
        {
            _unit.AddArmor(_unit.unitLevel * .5f);
            isCasting = true;
            Debug.Log("+ armor");
        }
        
        if (isCasting && _time < _duration)
        {
            _time += Time.deltaTime;
            controller.SetCondition(Unit.Statuses.Snared, .05f);
            return isCasting = true;
        }
        
        Debug.Log("- armor");
        _unit.AddArmor(-(_unit.unitLevel*.5f));
        _time = 0f;
        currentCd = _cd;
        return isCasting = false;
    }
}
