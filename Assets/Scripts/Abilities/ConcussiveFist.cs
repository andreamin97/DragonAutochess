using UnityEngine;
using UnityEngine.AI;

public class ConcussiveFist : Ability
{
    private readonly float _cd = 4;
    private bool isCasting;
    private Vector3 knockDirection;
    private float timer;

    public override bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController controller)
    {
        var target = controller.target;

        if (Vector3.Distance(transform.position, target.transform.position) <=
            controller.GetComponent<PlayerUnit>().attackRange && !isCasting)
        {
            target.TakeDamage(target.maxHealth * 0.05f, 1);
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