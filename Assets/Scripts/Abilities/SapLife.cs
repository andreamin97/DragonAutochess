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

        controller.target.TakeDamage(unit._attackDamage * 2f);
        unit.TakeDamage(-unit._attackDamage);

        currentCd = cd;
        return false;
    }
}