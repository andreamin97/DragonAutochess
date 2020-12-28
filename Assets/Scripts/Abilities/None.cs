using UnityEngine.AI;

public class None : Ability
{
    // Start is called before the first frame update
    private void Start()
    {
        InitAbility("None", "None", 0f, 0f);
    }

    public override bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController controller)
    {
        return base.Cast(navMeshAgent, boardManager, controller);
    }
}