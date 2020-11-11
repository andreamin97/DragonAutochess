using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public LayerMask boardLayer;
    public LayerMask unitLayer;
    /*[HideInInspector]*/ public GameObject selectedUnit;
    [HideInInspector] public bool isDragging;
    [HideInInspector] public GameObject selectedTile;
    public ShopManager shopManager;
    [Range(0, 100)] public int Gold;
    public bool isFighting;
    private BoardManager bm;
    private RaycastHit hitData;
    private readonly List<GameObject> ownedUnits = new List<GameObject>();
    public int level = 1;
    
    private int _experience = 0;
    private int _nextLevelExperience = 1;

    public Text experienceText;

    public int Experience
    {
        get => _experience;
        set => _experience = value;
    }

    private Ray ray;

    private void Start()
    {
        bm = FindObjectOfType<BoardManager>();
        EditGold(6);
    }

    // Update is called once per frame
    private void Update()
    {
        if (selectedUnit != null && isDragging)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitData, 1000, boardLayer))
            {
                var newObject = hitData.transform.gameObject;

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

        _nextLevelExperience = Mathf.FloorToInt((Mathf.Pow(level+1f, 2f))); 
        experienceText.text = _experience + " / " +  _nextLevelExperience;
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

    public void EditGold(int gold)
    {
        Gold += gold;
    }

    public void GainExp(int exp)
    {
        _experience += exp;
        level = Mathf.FloorToInt( Mathf.Sqrt(_experience + 1) );
    }
}