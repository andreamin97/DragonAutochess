using UnityEngine;
using UnityEngine.AI;

public class Fireball_Ability : Ability
{
    public float cd = 10f;
    public float damage = 30f;
    private Object _fb;
    private readonly float castTimer = .5f;
    private float time;

    public override bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController_Base controller)
    {
        if (_fb == null)
            _fb = Resources.Load("Fireball");


        if (time <= castTimer && currentCd <= 0f)
        {
            time += Time.deltaTime;
            return true;
        }

        if (time >= castTimer)
        {
            Instantiate(_fb, controller.target.transform.position + Vector3.up * 15f, Quaternion.identity);

            time = 0f;
            currentCd = cd;
            return false;
        }

        return false;
    }
}