using System;
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
    public Text role;
    public Text hp;
    public Text armor;
    public Text ad;
    public Text atkSpd;
    public Text abilityName;
    public Image lvlUpArrow;
    public GameObject InfoPane;
        
    public BaseUnit unit;
    private PlayerController _playerController;
    private BoardManager boardManager;
    private ShopManager _shopManager;
    private GoogleSheetsForUnity _sheets;
    private bool isEnabled = true;

    // private GameObject playerUnits;
    private PlayerUnit temp;
    private List<GameObject> playerUnits;
    private List<GameObject> units;

    private bool isInfoOpen = false;
    
    private void Awake()
    {
        boardManager = FindObjectOfType<BoardManager>();
        _playerController = FindObjectOfType<PlayerController>();
        _sheets = FindObjectOfType<GoogleSheetsForUnity>();
        _shopManager = FindObjectOfType<ShopManager>();
    }
    
    private void Start()
    {
        playerUnits = boardManager.PlayerUnitList();
        units = new List<GameObject>();
    }
    
    public void BuyUnit()
    {
        if (isEnabled)
        {
            if (_playerController.Gold >= unit.Cost && boardManager.GetBenchFreeSlot() != null)
            {
                CheckForLevelUp();
                var unitCount = units.Count;

                var benchSlot = boardManager.GetBenchFreeSlot();

                if (unitCount >= 2)
                {
                    units[0].GetComponent<PlayerUnit>().LevelUp();
                    Instantiate(Resources.Load("VFX/LevelUp"), units[0].gameObject.transform.position + Vector3.up, Quaternion.Euler(-90f, 0f, 0f));
                    
                    for (var i = 1; i < unitCount; i++)
                    {
                        boardManager.RemoveUnit(units[i], false);
                        Destroy(units[i]);
                    }

                    lvlUpArrow.enabled = false;
                    _playerController.EditGold(-unit.Cost);
                    
                    _sheets.AppendToSheet( "UnitsLevelUp", "A:A", new List<object>() { PlayerPrefs.GetString("MatchID"), unit.Name,  units[0].GetComponent<PlayerUnit>().unitLevel, boardManager.Stage } );
                    
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
               
                // _sheets.AppendToSheet("Units", "A:A", new List<object>() { temp.unitName });
                isEnabled = false;
                UpdateSlot("HIRED");
                
                foreach (var slot in _shopManager.slots)
                {
                    if (slot != this)
                        slot.CheckForLevelUp();
                }
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

        role.text = unit.role.ToString();
        hp.text = "HP: " + unit.Health.ToString();
        armor.text = "ARMOR: " + unit.Armor.ToString();
        ad.text = "AD: " + unit.AttackDamage.ToString();
        atkSpd.text = "AS: " + unit.AttackSpeed.ToString();
        abilityName.text = GetAbilityTextByClass(unit.Name);
        CheckForLevelUp();
    }

    public void CheckForLevelUp()
    {
        units.Clear();
        
        foreach (var ownedUnit in playerUnits)
            if (ownedUnit.GetComponent<PlayerUnit>().UnitClass == unit)
            {
                units.Add(ownedUnit);
            }

        if (units.Count >= 2)
        {
            lvlUpArrow.enabled = true;
        }
        else
        {
            lvlUpArrow.enabled = false;
        }
    }

    public void ToggleInfo()
    {
        isInfoOpen = !isInfoOpen;
        InfoPane.SetActive(isInfoOpen);
    }

    private string GetAbilityTextByClass(string name)
    {
        switch (name)
        {
            case "Druid":
                return "Snare an enemy for 5 seconds, impeding them to move and attack.";
            case "Cleric":
                return "Heal a friendly target for 10HP/lvl";
            case "Sorcerer":
                
            case "Warlock":
                return "Deal twice your attack damage to your target and heal half that amount";
            case "Paladin":
                return "Snare self for 3seconds, but gain .5 armor/level";
            case "Ranger":
                return "Summon a wolf at the beginning of the combat";
            case "Fighter":
                return "Deal my AD*lvl to all units around me";
            case "Barbarian":
                return "Gain 1% leech for every 1% missing hp, up to 70%";
            case "Rogue":
                return "Teleport to the lowest hp enemy and deal 5(+5/lvl) damage to it";
            case "Monk":
                return "Strike the target, knocking them backwards and briefly snaring them";
            case "Bard":
                return "Give all allies within a square from me at the beginning of the encounter 0.1AS/lvl";
            case "Wizard":
            default:
                return " ";
        }
    }
}