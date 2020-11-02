using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalCompanion : Ability
{
    private float cd = .2f;
    private float bonus = .1f;
    private float perLevel = 0.1f;
    private object _fb = null;
    private GameObject obj;
    private GameObject wolf = null;
    
    public override bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController controller)
    {
        if (!castOnce)
        {
            if (_fb == null)
                _fb = Resources.Load("Wolf");

            if (wolf == null)
            {
                obj = (GameObject) _fb;

                NavMeshHit hitPosition;
                NavMesh.SamplePosition(transform.position + Vector3.forward*3f, out hitPosition, 2f, NavMesh.AllAreas);

                wolf = Instantiate(obj, hitPosition.position, Quaternion.identity);
                wolf.GetComponent<PlayerUnit>().InitUnit();
            }
            else
            {
                wolf.GetComponent<PlayerUnit>().currentHealth = wolf.GetComponent<PlayerUnit>().maxHealth;
            }

            wolf.GetComponent<NavMeshAgent>().enabled = true;
            wolf.GetComponent<PlayerUnit>().isActive = true;
            boardManager.fightingUnits.Add(wolf);
            boardManager._ownedUnits.Add(wolf);

            foreach (var unit in boardManager.enemyFightingUnits)
            {
                var _controller = unit.GetComponent<EnemyAIController>();

                if (_controller.Target.gameObject == this.gameObject)
                {
                    _controller.Target = wolf.GetComponent<PlayerUnit>();
                }
            }
            
            castOnce = true;
        }

        currentCd = cd;
        return true;
    }
}