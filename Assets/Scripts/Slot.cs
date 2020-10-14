using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Slot : MonoBehaviour
{
    public GameObject baseUnit;
    
    public Image unitThumbnail;
    public Text unitName;
    public Text unitCost;

    public BaseUnit unit;
    
    private bool isEnabled = true;
    private BoardManager boardManager;
    private PlayerController _playerController;

    private GameObject playerUnits;

    private void Awake()
    {
        boardManager = FindObjectOfType<BoardManager>();
        _playerController = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        UpdateSlot();
    }

    public void BuyUnit()
    {
        //Check if the unit can level up
        List<GameObject> playerUnits = boardManager.PlayerUnitList();
        List<GameObject> units = new List<GameObject>();
        int unitCount = 0;
        
        GameObject benchSlot = boardManager.GetBenchFreeSlot();
        
        foreach (GameObject ownedUnit in playerUnits)
        {
            if (ownedUnit.GetComponent<PlayerUnit>().UnitClass == unit)
            {
                units.Add(ownedUnit);
                unitCount++;
            }
        }
        
        if (unitCount >= 3)
        {
            units[0].GetComponent<PlayerUnit>().LevelUp();
            for (int i = 1; i < unitCount; i++)
            {
                boardManager.RemoveUnit(units[i], false);
                Destroy(units[i]);
            }
        }
        else
        {
            if (benchSlot != null)
            {
                Vector3 spawnPosition = benchSlot.transform.position + Vector3.up;
                GameObject newUnit = Instantiate(baseUnit, spawnPosition, Quaternion.identity);

                NavMeshHit navHit;
                if (NavMesh.SamplePosition(spawnPosition, out navHit, 5, -1))
                {
                    newUnit.transform.position = navHit.position;
                    newUnit.GetComponent<NavMeshAgent>().enabled = true;
                }
            
                PlayerUnit temp = newUnit.GetComponent<PlayerUnit>();
                temp.UnitClass = unit;
                temp.InitUnit();
                isEnabled = false;
            
                boardManager.BuyUnit(newUnit);
            }
        }
        

    }

    public void Enable()
    {
        isEnabled = true;
    }

    public void UpdateSlot(string name="", string cost="", BaseUnit unitClass = null)
    {
        unitName.text = name;
        unitCost.text = cost;
        unit = unitClass;
    }
}
