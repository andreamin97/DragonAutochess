using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Singularity : Ability
{

    public float cd=10f;
    private Object _fb = null;
    private float castTimer = .2f;
    private float time = 0f;
    
    public override bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController controller)
    {
        if (_fb == null)
            _fb = Resources.Load("Singularity");

        

        if (time <= castTimer && currentCd <= 0f)
        {
            time += Time.deltaTime;
            return  true;
        }
        
        else if (time >= castTimer)
        {
            Instantiate(_fb, new Vector3(7f, 0f, 7f), Quaternion.Euler(45f, 0, 45f));
            
            time = 0f;
            currentCd = cd;
            return false;
        }

        return false;
    }
}