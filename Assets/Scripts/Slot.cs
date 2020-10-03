using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void Start()
    {
        boardManager = FindObjectOfType<BoardManager>();
    }

    private void Update()
    {
        if (isEnabled)
        {
            unitName.text = unit.Name;
            unitCost.text = unit.Cost.ToString();
        }
        else
        {
            unitName.text = "null";
            unitCost.text = "null";
        }
    }

    public void BuyUnit()
    {
        if (isEnabled)
        {
            Debug.Log("Bought " + unitName.text);

            GameObject benchSlot = boardManager.GetBenchFreeSlot();

            if (benchSlot != null)
            {
                Vector3 spawnPosition = benchSlot.transform.position + Vector3.up;
                GameObject newUnit = Instantiate(baseUnit, spawnPosition, Quaternion.identity);
                Unit temp = newUnit.GetComponent<Unit>();
                temp.UnitClass = unit;
                temp.InitUnit();
                isEnabled = false;
            }

            
        }
        

    }

    public void Enable()
    {
        isEnabled = true;
    }
}
