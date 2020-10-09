using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LayerMask boardLayer;
    public LayerMask unitLayer;
    [HideInInspector] public GameObject selectedUnit;
    [HideInInspector] public bool isDragging = false;
    [HideInInspector] public GameObject selectedTile = null;
    public ShopManager shopManager;
    [Range(0,100)]public int Gold;
    public bool isFighting = false;
    
    private Ray ray;
    private RaycastHit hitData;
    private BoardManager bm;
    private List<GameObject> ownedUnits = new List<GameObject>();
    
    private void Start()
    {
        bm = FindObjectOfType<BoardManager>();
    }

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
                      if (isDragging)
                      {
                          // do nothing
                      }
                      else // qif (selectedTile != null)
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

    public void AddOwnedUnit(GameObject unit)
    {
        ownedUnits.Add(unit);
    }

    public void RemoveOwnedUnit(GameObject unit)
    {
        ownedUnits.Remove(unit);
    }

    public List<GameObject> GetOwnedUnits()
    {
        return ownedUnits;
    }
}
