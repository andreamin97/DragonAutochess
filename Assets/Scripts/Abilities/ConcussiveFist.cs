using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using UnityEngine;
using UnityEngine.AI;

public class ConcussiveFist : Ability
{
    private float _cd = 4;
    private float timer = 0f;
    private bool isCasting = false;
    private Vector3 knockDirection;
    public override bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController controller)
    {
        var target = controller.target;

        if (Vector3.Distance(transform.position, target.transform.position) <=
            controller.GetComponent<PlayerUnit>().attackRange && !isCasting)
        {
            target.TakeDamage(target.maxHealth*0.05f);
            target.GetComponent<EnemyAIController>().SetCondition(Unit.Statuses.Snared, 1f);
            // target.GetComponent<NavMeshAgent>().Warp(target.transform.position + transform.forward * 3f);
            knockDirection = transform.right;
            isCasting = true;
        }
        else if (isCasting && timer < .5f)
        {
            timer += Time.deltaTime;
            target.transform.Translate(knockDirection * 8f * Time.deltaTime);
            isCasting = true;
        }
        else if (timer >= .5f)
        {
            timer = 0f;
            isCasting = false;
            currentCd = _cd;
        }

        return false;
    }
}