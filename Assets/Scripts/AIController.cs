using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class AIController : AIController_Base
{
    private void Start()
    {
       InitializeLists();
    }

    public void ResetUnit(Vector3 position)
    {
        _navMeshAgent.Warp(position);
    }
    
    protected override void InitializeLists()
    {
        alliedUnits = _boardManager.PlayerFightingUnitList();
        enemyUnits = _boardManager.EnemyList();
    }

}