using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LayerMask boardLayer;
    public LayerMask unitLayer;
    public GameObject selectedUnit;
    public bool isDragging = false;
    public GameObject selectedTile = null;
    public ShopManager shopManager;
    
    private Ray ray;
    private RaycastHit hitData;
    

    // Update is called once per frame
    void Update()
    {
        if (selectedUnit != null && isDragging)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    
            if (Physics.Raycast(ray, out hitData, 1000, boardLayer))
            {
                GameObject newObject = hitData.transform.gameObject;
                
                if (newObject != selectedTile)
                {
                    if (selectedTile != null) selectedTile.GetComponent<Tile>().ToggleSelected();
                    
                    selectedTile = newObject;
                    selectedTile.GetComponent<Tile>().ToggleSelected();
                }
            }
            else
            {
                if (selectedTile != null)
                {
                    selectedTile.GetComponent<Tile>().ToggleSelected();
                    selectedTile = null;
                }
            }
        }
        else if (!isDragging && selectedTile != null)
        {
            selectedTile.GetComponent<Tile>().ToggleSelected();
            selectedTile = null;
        }
        
        if (Input.GetKeyDown("p"))
            shopManager.ToggleShopUI();
        
        if (Input.GetKeyDown("d"))
            shopManager.RandomizeShop();
    }
    
}
