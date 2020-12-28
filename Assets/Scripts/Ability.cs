using UnityEngine;
using UnityEngine.AI;

public class Ability : MonoBehaviour
{
    public string abilityName;
    public string abilityText;
    public float coolDown;
    public float currentCd;
    public bool castOnce;

    public void InitAbility(string name, string text, float InitialCd, float cd)
    {
        abilityName = name;
        abilityText = text;
        coolDown = cd;
        currentCd = InitialCd;
    }

    public virtual bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController controller)
    {
        return true;
    }

    public void ResetCD()
    {
        currentCd = coolDown;
    }
}