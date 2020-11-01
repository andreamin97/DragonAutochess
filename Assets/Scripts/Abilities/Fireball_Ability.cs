using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fireball_Ability : Ability
{
    public float cd=10f;
    public float damage = 30f;

    public override bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController controller)
    {
        var _fb = Resources.Load("Fireball");
        Instantiate(_fb, controller.transform.position + Vector3.up * 10f, Quaternion.identity);

        coolDown = cd;
        return false;
    }
}
    