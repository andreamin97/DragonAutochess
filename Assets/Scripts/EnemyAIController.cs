using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : AIController_Base
{
    
    private void Start()
    {
       InitializeLists(); 
    }

    protected override void InitializeLists()
    {
        enemyUnits = _boardManager.PlayerFightingUnitList();
        alliedUnits = _boardManager.EnemyList();
    }
}