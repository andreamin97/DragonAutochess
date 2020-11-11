using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Inspiration : Ability
{
    private float cd = .2f;
    private float bonus = .1f;
    private float perLevel = 0.1f;
    
    public override bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController controller)
    {
        if (!castOnce)
        {
            Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(2.5f, 0.5f, 2.5f));

            foreach (var coll in colliders)
            {
                if (coll.GetComponent<PlayerUnit>() != null)
                {
                    coll.GetComponent<PlayerUnit>()._attackSpeed -= (bonus + perLevel);
                    Instantiate( Resources.Load("VFX/Bard_Buff"), coll.transform.position+Vector3.up, Quaternion.Euler(-90f, 0f, 0f));
                }
            }

            castOnce = true;
        }

        currentCd = cd;
        return true;
    }

}
