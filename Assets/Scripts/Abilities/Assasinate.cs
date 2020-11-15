﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Assasinate : Ability
{
    private float _cd = 6f;
    private PlayerUnit _unit;
    EnemyUnit lowestEnemy = null;
    private GameObject effect;

    public override bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController controller)
    {
        effect = (GameObject) Resources.Load("VFX/Smoke");
        
        if (_unit == null)
            _unit = controller.GetComponent<PlayerUnit>();
        
        foreach (var unit in boardManager.enemyFightingUnits)
        {
            if (lowestEnemy == null)
            {
                lowestEnemy = unit.GetComponent<EnemyUnit>();
            }
            else
            {
                if (unit.GetComponent<EnemyUnit>().currentHealth < lowestEnemy.currentHealth)
                {
                    lowestEnemy = unit.GetComponent<EnemyUnit>();
                }
            }
        }

        NavMeshHit hitPosition; 
        NavMesh.SamplePosition(lowestEnemy.transform.position, out hitPosition, 2f, NavMesh.AllAreas);

        Instantiate(effect, transform.position, Quaternion.identity);
        navMeshAgent.Warp(hitPosition.position);
        Instantiate(effect, transform.position, Quaternion.identity);
        lowestEnemy.TakeDamage(5f + 5*_unit.unitLevel);
        controller.ResetTarget();

        currentCd = _cd;
        return false;
    }
}
