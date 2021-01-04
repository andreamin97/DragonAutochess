using UnityEngine;
using UnityEngine.AI;

public class SapLife : Ability
{
    private readonly float cd = 4f;

    public override bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController controller)
    {
        var unit = controller.GetComponent<PlayerUnit>();

        var effect = Resources.Load("VFX/SapLife");
        var vEffetc = (GameObject) Instantiate(effect, controller.target.transform.position + Vector3.up,
            Quaternion.identity);
        vEffetc.GetComponentInChildren<particleAttractorLinear>().target = unit.transform;

        var damage = Random.Range(unit._attackDamageMin, unit._attackDamageMax);
        
        controller.target.TakeDamage(damage * 2f, 1);
        unit.TakeDamage(-damage, 0);

        currentCd = cd;
        return false;
    }
}