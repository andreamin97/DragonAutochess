using UnityEngine;
using UnityEngine.AI;

public class SproutingRoots : Ability
{
    public float cd = 8f;
    public float snareDuration = 3f;
    public float durationPerLevel = 0.5f;

    public override bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController controller)
    {
        var distance = 0f;
        EnemyUnit target = null;
        float tempDistance;

        var _unit = controller.GetComponent<PlayerUnit>();

        //get target
        foreach (var unit in boardManager.enemyFightingUnits)
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

        currentCd = cd;

        target.GetComponent<EnemyAIController>()
            .SetCondition(Unit.Statuses.Snared, snareDuration + durationPerLevel * _unit.unitLevel);
        var snareFX = (GameObject) Instantiate(Resources.Load("VFX/Druid_Snare"),
            target.transform.position + Vector3.up / 2f, Quaternion.Euler(-90f, 0f, 0f));
        snareFX.GetComponent<vfx_duration>().duration = snareDuration;
        //return
        controller.ResetTarget();
        return false;
    }
}