﻿using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public Slot[] slots;
    public UnitList unitsList;
    public Canvas canvas;

    private bool isShopOpen = false;
    private BaseUnit unit;

    private void Start()
    {
        canvas.gameObject.SetActive(false); 
        RandomizeShop();
    }

    public void ToggleShopUI()
    {
        isShopOpen = !isShopOpen;
        canvas.gameObject.SetActive(isShopOpen);
    }

    public void RandomizeShop()
    {
        foreach (Slot slot in slots)
        {
            BaseUnit unit = unitsList.RandomUnit();
            slot.Enable();
            slot.UpdateSlot(unit.Name, unit.Cost.ToString(), unit);
        }
    }
}
