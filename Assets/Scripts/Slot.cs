using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public GameObject Unit;

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

    private PlayerController _playerController;
    private GoogleSheetsForUnity _sheets;
    private ShopManager _shopManager;
    private BoardManager boardManager;
    private bool isEnabled = true;

    private bool isInfoOpen;
    private List<GameObject> playerUnits;

    // private GameObject playerUnits;
    private PlayerUnit temp;
    private List<GameObject> units;

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
            var cost = Unit.GetComponent<PlayerUnit>().unitCost;

            if (_playerController.Gold >= cost && boardManager.GetBenchFreeSlot() != null)
            {
                CheckForLevelUp();
                var unitCount = units.Count;

                var benchSlot = boardManager.GetBenchFreeSlot();

                if (unitCount >= 2)
                {
                    units[0].GetComponent<PlayerUnit>().LevelUp();
                    Instantiate(Resources.Load("VFX/LevelUp"), units[0].gameObject.transform.position + Vector3.up,
                        Quaternion.Euler(-90f, 0f, 0f));

                    for (var i = 1; i < unitCount; i++)
                    {
                        boardManager.RemoveUnit(units[i], false);
                        Destroy(units[i]);
                    }

                    lvlUpArrow.enabled = false;
                    _playerController.EditGold(-cost);

                    // _sheets.AppendToSheet( "UnitsLevelUp", "A:A", new List<object>() { PlayerPrefs.GetString("MatchID"), unit.Name,  units[0].GetComponent<PlayerUnit>().unitLevel, boardManager.Stage } );
                }
                else
                {
                    if (benchSlot != null)
                    {
                        var spawnPosition = benchSlot.transform.position + Vector3.up;
                        var newUnit = Instantiate(Unit, spawnPosition, Quaternion.identity);

                        NavMeshHit navHit;
                        if (NavMesh.SamplePosition(spawnPosition, out navHit, 5, -1))
                        {
                            newUnit.transform.position = navHit.position;
                            newUnit.GetComponent<NavMeshAgent>().enabled = true;
                        }

                        temp = newUnit.GetComponent<PlayerUnit>();
                        temp.InitUnit();
                        isEnabled = false;

                        boardManager.BuyUnit(newUnit);
                        _playerController.EditGold(-cost);
                    }
                }

                // _sheets.AppendToSheet("Units", "A:A", new List<object>() { temp.unitName });
                isEnabled = false;
                UpdateSlot();

                foreach (var slot in _shopManager.slots)
                    if (slot != this)
                        slot.CheckForLevelUp();
            }
        }
    }

    public void Enable()
    {
        isEnabled = true;
    }

    public void UpdateSlot(GameObject unit = null)
    {
        Unit = unit;


        if (unit != null)
        {
            var unitScript = unit.GetComponent<PlayerUnit>();
            var unitClass = unitScript.UnitClass;
            unitName.text = unitScript.unitName;
            unitCost.text = unitScript.unitCost.ToString();
            role.text = unitClass.role.ToString();
            hp.text = "HP: " + unitClass.Health;
            armor.text = "ARMOR: " + unitClass.Armor;
            ad.text = "AD: " + unitClass.AttackDamage;
            atkSpd.text = "AS: " + unitClass.AttackSpeed;
            abilityName.text = GetAbilityTextByClass(unitClass.Name);
        }
        else
        {
            role.text = "";
            hp.text = "";
            armor.text = "";
            ad.text = "";
            atkSpd.text = "";
            abilityName.text = GetAbilityTextByClass("");
            unitName.text = "HIRED";
            unitCost.text = "";
            role.text = "";
        }

        CheckForLevelUp();
    }

    public void CheckForLevelUp()
    {
        units.Clear();
        BaseUnit unitClass = null;

        if (Unit != null) unitClass = Unit.GetComponent<PlayerUnit>().UnitClass;

        foreach (var ownedUnit in playerUnits)
            if (ownedUnit.GetComponent<PlayerUnit>().UnitClass == unitClass)
                units.Add(ownedUnit);

        if (units.Count >= 2)
            lvlUpArrow.enabled = true;
        else
            lvlUpArrow.enabled = false;
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