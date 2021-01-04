using UnityEngine;
using UnityEngine.AI;

public class Heal : Ability
{
    public float range = 3f;
    public float amount = 10f;
    public float perLevel = 10f;
    public float cd = 5f;
    public GameObject vEffect;
    private float distance;
    private bool hasTarget;
    private Unit lowestUnit;

    private void Start()
    {
        vEffect = (GameObject) Resources.Load("VFX/FX_Healing_AOE_AA");
    }

    public override bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController_Base controller)
    {
        if (!hasTarget)
        {
            // acquire lowest on health friendly target
            foreach (var unit in controller.AlliedUnits)
                if (lowestUnit == null)
                {
                    lowestUnit = unit.GetComponent<Unit>();
                }
                else
                {
                    if (unit.GetComponent<Unit>().currentHealth / unit.GetComponent<Unit>().maxHealth <
                        lowestUnit.currentHealth / lowestUnit.maxHealth)
                        lowestUnit = unit.GetComponent<Unit>();
                }

            hasTarget = true;
            controller.target = lowestUnit;
        }

        //move to him
        if (lowestUnit != null)
        {
            distance = Vector3.Distance(lowestUnit.transform.position, transform.position);
        }
        else
        {
            hasTarget = false;
            return true;
        }

        if (distance > range)
        {
            //navMeshAgent.SetDestination(lowestUnit.transform.position);
        }
        else
        {
            //Effect
            Instantiate(vEffect, lowestUnit.transform);
            //heal
            lowestUnit.TakeDamage(-(amount + perLevel*controller.GetComponent<Unit>().unitLevel), 0);

            //reset the target and set the cooldown, return false to stop casting from the AI controller
            controller.ResetTarget();
            currentCd = cd;
            hasTarget = false;
            return false;
        }

        //return true to keep casting from the controller
        return true;
    }
}