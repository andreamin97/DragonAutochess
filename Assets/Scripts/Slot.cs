using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public GameObject baseUnit;

    public Image unitThumbnail;
    public Text unitName;
    public Text unitCost;

    public BaseUnit unit;
    private PlayerController _playerController;
    private BoardManager boardManager;
    private GoogleSheetsForUnity _sheets;
    private bool isEnabled = true;

    private GameObject playerUnits;
    private PlayerUnit temp;

    private void Awake()
    {
        boardManager = FindObjectOfType<BoardManager>();
        _playerController = FindObjectOfType<PlayerController>();
        _sheets = FindObjectOfType<GoogleSheetsForUnity>();
    }

    private void Start()
    {
        UpdateSlot();
    }

    public void BuyUnit()
    {
        if (isEnabled)
        {

            if (_playerController.Gold >= unit.Cost)
            {
                //Check if the unit can level up
                var playerUnits = boardManager.PlayerUnitList();
                var units = new List<GameObject>();
                var unitCount = 0;

                var benchSlot = boardManager.GetBenchFreeSlot();

                foreach (var ownedUnit in playerUnits)
                    if (ownedUnit.GetComponent<PlayerUnit>().UnitClass == unit)
                    {
                        units.Add(ownedUnit);
                        unitCount++;
                    }

                if (unitCount >= 3)
                {
                    units[0].GetComponent<PlayerUnit>().LevelUp();
                    Instantiate(Resources.Load("VFX/LevelUp"), units[0].transform.position, Quaternion.identity);
                    for (var i = 1; i < unitCount; i++)
                    {
                        boardManager.RemoveUnit(units[i], false);
                        Destroy(units[i]);
                    }

                    _playerController.EditGold(-unit.Cost);
                    
                    _sheets.AppendToSheet( "UnitsLevelUp", "A:A", new List<object>() { unit.Name,  units[0].GetComponent<PlayerUnit>().unitLevel, boardManager.Stage } );
                    
                }
                else
                {
                    if (benchSlot != null)
                    {
                        var spawnPosition = benchSlot.transform.position + Vector3.up;
                        var newUnit = Instantiate(baseUnit, spawnPosition, Quaternion.identity);

                        NavMeshHit navHit;
                        if (NavMesh.SamplePosition(spawnPosition, out navHit, 5, -1))
                        {
                            newUnit.transform.position = navHit.position;
                            newUnit.GetComponent<NavMeshAgent>().enabled = true;
                        }

                        temp = newUnit.GetComponent<PlayerUnit>();
                        temp.UnitClass = unit;
                        temp.InitUnit();
                        isEnabled = false;

                        boardManager.BuyUnit(newUnit);
                        _playerController.EditGold(-unit.Cost);
                    }
                }
               
                _sheets.AppendToSheet("Units", "A:A", new List<object>() { temp.unitName });
                isEnabled = false;
                UpdateSlot();
            }

            
        }
        
    }

    public void Enable()
    {
        isEnabled = true;
    }

    public void UpdateSlot(string name = "", string cost = "", BaseUnit unitClass = null)
    {
        unitName.text = name;
        unitCost.text = cost;
        unit = unitClass;
    }
}