using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{

    public string abilityName;
    public string abilityText;
    public float coolDown = 0f;
    public float currentCd;
    public bool castOnce = false;

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
