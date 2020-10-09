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
        if (isEnabled)
        {
            GameObject benchSlot = boardManager.GetBenchFreeSlot();

            if (benchSlot != null)
            {
                Vector3 spawnPosition = benchSlot.transform.position + Vector3.up;
                GameObject newUnit = Instantiate(baseUnit, spawnPosition, Quaternion.identity);

                NavMeshHit navHit;
                if (NavMesh.SamplePosition(spawnPosition, out navHit, 5, -1))
                {
                    newUnit.transform.position = navHit.position;
                    newUnit.AddComponent<NavMeshAgent>();
                }
                
                Unit temp = newUnit.GetComponent<Unit>();
                temp.UnitClass = unit;
                temp.InitUnit();
                isEnabled = false;
                
                _playerController.AddOwnedUnit(newUnit);
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
