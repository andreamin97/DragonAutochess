using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public Slot[] slots;
    public GameObject[] unitsList;
    public Canvas canvas;

    private bool isShopOpen = false;
    private Unit unit;
    

    public PlayerController pc;

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
        GameObject unit;
        foreach (Slot slot in slots)
        {
            unit = unitsList[Random.Range(0, 2)];
            slot.unitName.text = unit.GetComponent<Unit>().unitName;
            slot.unitCost.text = unit.GetComponent<Unit>().unitCost.ToString();
        }
    }
}
