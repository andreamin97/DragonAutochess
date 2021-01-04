using UnityEngine;
using UnityEngine.AI;

public class Assasinate : Ability
{
    private readonly float _cd = 6f;
    private PlayerUnit _unit;
    private GameObject effect;
    private EnemyUnit lowestEnemy;

    public override bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController_Base controller)
    {
        effect = (GameObject) Resources.Load("VFX/Smoke");

        if (_unit == null)
            _unit = controller.GetComponent<PlayerUnit>();

        foreach (var unit in boardManager.enemyFightingUnits)
            if (lowestEnemy == null)
            {
                lowestEnemy = unit.GetComponent<EnemyUnit>();
            }
            else
            {
                if (unit.GetComponent<EnemyUnit>().currentHealth < lowestEnemy.currentHealth)
                    lowestEnemy = unit.GetComponent<EnemyUnit>();
            }

        NavMeshHit hitPosition;
        NavMesh.SamplePosition(lowestEnemy.transform.position, out hitPosition, 2f, NavMesh.AllAreas);

        Instantiate(effect, transform.position, Quaternion.identity);
        navMeshAgent.Warp(hitPosition.position);
        Instantiate(effect, transform.position, Quaternion.identity);
        lowestEnemy.TakeDamage(5f + 5 * _unit.unitLevel, 1);
        controller.ResetTarget();

        currentCd = _cd;
        return false;
    }
}