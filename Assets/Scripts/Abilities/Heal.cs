using System;
using System.Collections;
using System.Collections.Generic;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AI;

public class Heal : Ability
{
    private PlayerUnit lowestUnit = null;
    private bool hasTarget = false;
    public float range = 3f;
    public float amount = 10f;
    public float perLevel = 10f;
    public float cd = 5f;
    private float distance;
    public GameObject vEffect = null;

    private void Start()
    {
        vEffect = (GameObject) Resources.Load("VFX/FX_Healing_AOE_AA");
    }

    public override bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController controller)
    {
        if (!hasTarget)
        {
            // acquire lowest on health friendly target
            foreach (var unit in boardManager.fightingUnits)
            {
                if (lowestUnit == null)
                {
                    lowestUnit = unit.GetComponent<PlayerUnit>();
                }
                else
                {    
                    if (unit.GetComponent<PlayerUnit>().currentHealth/unit.GetComponent<PlayerUnit>().maxHealth < lowestUnit.currentHealth/lowestUnit.maxHealth)
                        lowestUnit = unit.GetComponent<PlayerUnit>();
                }
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
            lowestUnit.TakeDamage(-(amount+perLevel));
            
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
