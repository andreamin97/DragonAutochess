using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fireball_Ability : Ability
{
    public float cd=10f;
    public float damage = 30f;
    private bool isCasting = false;
    private Object _fb = null;
    private float castTimer = .5f;
    private int fireballsSpawned = 0;
    private float time = 0f;
    
    public override bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController controller)
    {
        if (_fb == null)
            _fb = Resources.Load("Fireball");

        

        if (time <= castTimer && currentCd <= 0f)
        {
            time += Time.deltaTime;
            return isCasting = true;
        }
        
        else if (time >= castTimer)
        {
            Instantiate(_fb, controller._target.transform.position + Vector3.up * 10f, Quaternion.identity);

            time = 0f;
            currentCd = cd;
            return isCasting = false;
        }

        return false;
    }
}
    