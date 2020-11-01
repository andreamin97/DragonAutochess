using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SapLife : Ability
{
  private float cd = 4f;
  public override bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController controller)
  {
    var unit = controller.GetComponent<PlayerUnit>();
    controller.target.TakeDamage(unit._attackDamage*2f);
    unit.TakeDamage(-unit._attackDamage);

    currentCd = cd;
    return false;
  }
}
