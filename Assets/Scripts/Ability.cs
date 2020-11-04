using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{

    public string abilityName;
    public string abilityText;
    public float coolDown = 5f;
    public float currentCd;
    public bool castOnce = false;

    public void InitAbility(string name, string text, float cd)
    {
        abilityName = name;
        abilityText = text;
        coolDown = cd;
        currentCd = cd;
    }
    
    public virtual bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController controller)
    {
        return true;
    }
}
