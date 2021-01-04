using UnityEngine;
using UnityEngine.AI;

public class BladeSpin : Ability
{
    public float range = 3.5f;
    public float cd = 3f;
    private PlayerUnit _unit;


    public override bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController controller)
    {
        if (_unit == null)
            _unit = controller.GetComponent<PlayerUnit>();

        Instantiate(Resources.Load("Cleave"), transform.position + Vector3.up, Quaternion.Euler(90f, 0f, 0f));

        var colliders = Physics.OverlapSphere(transform.position, range);
        foreach (var coll in colliders)
        {

            if (coll.gameObject.GetComponent<EnemyUnit>())
            {
                var damage = Random.Range(_unit._attackDamageMin, _unit._attackDamageMax);
                coll.gameObject.GetComponent<EnemyUnit>().TakeDamage(damage * _unit.unitLevel, 1);
            }
        }

        currentCd = cd;
        return false;
    }
}