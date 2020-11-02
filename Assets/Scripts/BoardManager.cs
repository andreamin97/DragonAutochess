using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    public BoardTile[] board;
    public BoardTile[] bench;
    public BoardTile[] enemyBoard;
    public Board[] enemyPositioning;

    private int _currentFightBoardIndex;

    public List<GameObject> _ownedUnits = new List<GameObject>();
    public List<GameObject> fightingUnits = new List<GameObject>();
    public List<GameObject> enemyFightingUnits = new List<GameObject>();

    public GameObject baseEnemy;
    private PlayerController _playerController;
    private GoogleSheetsForUnity _sheetsForUnity;
    private ShopManager _shopManager;

    private float _fightTimer = 0f;
    private int _stage = 1;
    private int _unitCount;

    public int Stage => _stage;

    public Text stageText;
    public Text maxUnitText;
    public Text playerLevel;
    public Text playerGold;
    public int maxUnit = 2;
    
    private void Awake()
    {
        _sheetsForUnity = FindObjectOfType<GoogleSheetsForUnity>();
        _playerController = Camera.main.GetComponent<PlayerController>();
        _shopManager = FindObjectOfType<ShopManager>();
    }

    private void Start()
    {
        DeployEnemyBoard();
        stageText.text = _stage.ToString();
        maxUnitText.text = (_playerController.level + 1).ToString();
        maxUnit = _playerController.level + 1;
    }

    private void Update()
    {

        int fightingUnitCount = fightingUnits.Count;
        int enemyFightingUnitCount = EnemyList().Count;
        bool fightOver = false;

        maxUnitText.text = (1 + _playerController.level).ToString();
        playerLevel.text = _playerController.level.ToString();

        if (_playerController.isFighting)
        {
            if (fightingUnitCount <= 0)
            {
                // The player has lost
                // Check for Lose State: the player doesnt own any more units and has less then <CHEAPES UNIT COST> gold
                
                //if the player has lost
                
                _playerController.isFighting = false;

                foreach (var unit in enemyFightingUnits)
                {
                    unit.GetComponent<EnemyUnit>().TakeDamage(10000f);
                    Debug.Log("Dead");
                }
                enemyFightingUnits.Clear();
               
                DeployEnemyBoard();
                _shopManager.RandomizeShop(false);
                
                fightOver = true;
            }

            if (enemyFightingUnitCount <= 0)
            {
                //the player has won
                
                _playerController.isFighting = false;
                _playerController.GainExp(1);
                
                foreach (var unit in fightingUnits) 
                    unit.GetComponent<PlayerUnit>().isActive = false;
                
                ResetPlayerUnitsPosition();
                DeployEnemyBoard();
                fightOver = true;
               _stage++;
               stageText.text = _stage.ToString();
               _playerController.EditGold(5);
               
               _shopManager.RandomizeShop(false);
            }

            if (fightOver)
            {
                _sheetsForUnity.AppendToSheet("FightDuration", "B:B", new List<object>() { _unitCount, _stage-1, _fightTimer});
               fightingUnits.Clear();
            }
        }
       
        if (_playerController.isFighting)
            _fightTimer += Time.deltaTime;

        playerGold.text = _playerController.Gold.ToString();
    }

    private void DeployEnemyBoard()
    {
       _currentFightBoardIndex = Random.Range(0, enemyPositioning.Length - 1);

        for (var i = 0; i < 32; i++)
            if (enemyPositioning[_currentFightBoardIndex].enemyPositioning[i] != null)
            {
                var spawnPosition = enemyBoard[i].tile.transform.position + Vector3.up;

                var newUnit = Instantiate(baseEnemy, spawnPosition, Quaternion.identity);

                NavMeshHit navHit;
                if (NavMesh.SamplePosition(spawnPosition, out navHit, 5, -1))
                {
                    newUnit.transform.position = navHit.position;
                    newUnit.GetComponent<NavMeshAgent>().enabled = true;
                }

                newUnit.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                var temp = newUnit.GetComponent<EnemyUnit>();
                temp.enemyClass = enemyPositioning[_currentFightBoardIndex].GetEnemyAtIndex(i);
                temp.InitUnit();

                enemyBoard[i].unit = newUnit;
            }
    }

    public GameObject GetBenchFreeSlot()
    {
        foreach (var tile in bench)
            if (tile.unit == null)
                return tile.tile;

        return null;
    }

    public void SetUnitAtSlot(GameObject unit, GameObject tile)
    {
        for (var i = 0; i < bench.Length; i++)
            if (bench[i].tile == tile)
                bench[i].unit = unit;
        for (var i = 0; i < board.Length; i++)
            if (board[i].tile == tile)
                board[i].unit = unit;
    }

    public GameObject IsSlotFree(GameObject tile)
    {
        foreach (var boardTile in bench) return boardTile.unit;

        return null;
    }

    public bool IsUnitBenched(GameObject unit)
    {
        foreach (var tile in bench)
            if (tile.unit == unit)
                return true;

        return false;
    }

    public void StartEncounter()
    {
        for (var i = 0; i < 32; i++)
            if (board[i].unit != null)
            {
                if (fightingUnits.Count < _playerController.level + 1 && board[i].unit.GetComponent<PlayerUnit>().UnitClass.Name != "Wolf")
                {
                    fightingUnits.Add(board[i].unit);
                }
                else if (board[i].unit.GetComponent<PlayerUnit>().UnitClass.Name == "Wolf")
                {
                    fightingUnits.Add(board[i].unit);
                }
                else
                {
                    board[i].unit.GetComponent<NavMeshAgent>().Warp(GetBenchFreeSlot().transform.position);
                }

            }

        foreach (var unit in fightingUnits)
        {
            var _unit = unit.GetComponent<PlayerUnit>();
            var _ai = _unit.GetComponent<AIController>(); 
            _unit.isActive = true;
           
            if ( _ai != null)
            {
                //_ai.ability1.currentCd = _ai.ability1.coolDown;
                _ai.SetCondition(Unit.Statuses.None, 0f);
                _ai.ability1.castOnce = false;
            }
        }

    for (var i = 0; i < 32; i++)
            if (enemyBoard[i].unit != null)
            {
                enemyBoard[i].unit.GetComponent<EnemyUnit>().isActive = true;
                enemyFightingUnits.Add(enemyBoard[i].unit);
            }

        _playerController.isFighting = true;
        _unitCount = fightingUnits.Count;
    }

    public List<GameObject> EnemyList()
    {
        var tempList = new List<GameObject>();

        foreach (var tile in enemyBoard)
            if (tile.unit != null)
                tempList.Add(tile.unit.gameObject);

        return tempList;
    }

    public List<GameObject> PlayerFightingUnitList()
    {
        return fightingUnits;
    }

    public List<GameObject> PlayerUnitList()
    {
        return _ownedUnits;
    }

    public void BuyUnit(GameObject unit)
    {
        _ownedUnits.Add(unit);
    }

    public void RemoveUnit(GameObject unit, bool wasSold)
    {

        if (wasSold)
        {
            _playerController.EditGold(unit.GetComponent<PlayerUnit>().unitCost);
            Debug.Log(unit.GetComponent<PlayerUnit>().unitCost);
        }

        for (int i = 0; i < 32; i++)
                if(board[i].unit == unit)
                    SetUnitAtSlot(null, board[i].tile);
            
        _ownedUnits.Remove(unit);

        Destroy(unit);
    }

    private void ResetPlayerUnitsPosition()
    {
        if (fightingUnits.Count > 0)
            foreach (var unit in fightingUnits)
                if (unit != null)
                    for (var i = 0; i < 32; i++)
                        if (unit == board[i].unit)
                        {
                            unit.GetComponent<AIController>().ResetUnit(board[i].tile.transform.position);
                            unit.GetComponent<PlayerUnit>()._attackSpeed =
                                unit.GetComponent<PlayerUnit>().UnitClass.AttackSpeed +
                                unit.GetComponent<PlayerUnit>().UnitClass.ASPerLevel;
                        }
    
}

    [Serializable]
    public struct BoardTile
    {
        public GameObject tile;
        public GameObject unit;
    }
}