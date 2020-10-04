using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{

    private List<Enemy> _enemiesList;
    private List<Unit> _unitsList;

    void StartBattle()
    {
        _enemiesList = new List<Enemy>();
        
        foreach (Enemy go in Resources.FindObjectsOfTypeAll(typeof(Enemy)) as Enemy[])
        {
        }
    }


}
