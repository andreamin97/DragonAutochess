using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class SproutingRoots : Ability
{
    public float cd = 8f;
    public float snareDuration = 2f;
    
    public override bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController controller)
    {
        float distance = 0f;
        EnemyUnit target = null;
        float tempDistance;
        //get target
        foreach (var unit in boardManager.enemyFightingUnits)
        {
            if (target == null)
            {
                target = unit.GetComponent<EnemyUnit>();
            }
            else
            {
                tempDistance = Vector3.Distance(unit.transform.position, transform.position);
                if (tempDistance < distance)
                {
                    distance = tempDistance;
                    target = unit.GetComponent<EnemyUnit>();
                }
            }
        }
        
        currentCd = cd;
        
        target.GetComponent<EnemyAIController>().SetCondition(Unit.Statuses.Snared, snareDuration);
        
        //return
        controller.ResetTarget();
        return false;
    }
}
