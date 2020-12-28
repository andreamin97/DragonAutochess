using UnityEngine;
using UnityEngine.AI;

public class Singularity : Ability
{
    public float cd = 10f;
    private Object _fb;
    private readonly float castTimer = .2f;
    private GameObject prefab;
    private float time;

    public override bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController controller)
    {
        if (_fb == null)
            _fb = Resources.Load("Singularity");


        if (time <= castTimer && currentCd <= 0f)
        {
            time += Time.deltaTime;
            return true;
        }

        if (time >= castTimer)
        {
            prefab = (GameObject) Instantiate(_fb, new Vector3(7f, 1f, 7f), Quaternion.Euler(-90f, 0, 0f));

            boardManager.summonedUnits.Add(prefab);

            time = 0f;
            currentCd = cd;
            return false;
        }

        return false;
    }
}