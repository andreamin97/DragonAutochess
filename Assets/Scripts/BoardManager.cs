using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    public BoardTile[] board;
    public BoardTile[] bench;
    public BoardTile[] enemyBoard;
    public Board[] enemyPositioning;
    public Board[] enemyPositioningBoss;
    
    private int _currentFightBoardIndex;

    public List<GameObject> _ownedUnits = new List<GameObject>();
    public List<GameObject> fightingUnits = new List<GameObject>();
    public List<GameObject> enemyFightingUnits = new List<GameObject>();
    public List<GameObject> summonedUnits = new List<GameObject>();

    public GameObject baseEnemy;
    public GameObject FloatingText;
    private PlayerController _playerController;
    private GoogleSheetsForUnity _sheetsForUnity;
    private ShopManager _shopManager;
    private float boardResetTimer = 0f;

    public bool gameRunning = false;
    public Canvas menu;

    private float _fightTimer = 0f;
    private int _stage = 1;
    private int _unitCount;

    public int Stage => _stage;

    public Text stageText;
    public Text maxUnitText;
    public Text playerLevel;
    public Text playerGold;
    public int maxUnit = 2;
    public Button fightButton;

    private string result;
    
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

        fightButton.GetComponent<Image>().enabled = !_playerController.isFighting;
        fightButton.GetComponentInChildren<Text>().enabled = !_playerController.isFighting;
       
        if (_playerController.isFighting)
        {

            if (fightingUnitCount <= 0)
            {
                // The player has lost

                result = "Lost";
                _playerController.isFighting = false;
                fightOver = true;
                
                // Destroy the still alive enemies and spawn a new board
                foreach (var unit in enemyFightingUnits)
                {
                    Destroy(unit);
                }
                enemyFightingUnits.Clear();
               
                DeployEnemyBoard();
                _shopManager.RandomizeShop(false);

                foreach (var summon in summonedUnits)
                {
                    Destroy(summon);
                }
                summonedUnits.Clear();
                _sheetsForUnity.AppendToSheet("FightDuration", "B:B", new List<object>() { PlayerPrefs.GetString("MatchID"), _unitCount, _stage, _fightTimer, result});
                
            }

            if (enemyFightingUnitCount <= 0)
            {
                //the player has won
                _stage++;
                result = "Won";
                _playerController.isFighting = false;
                _playerController.GainExp(1);
 
                foreach (var summon in summonedUnits)
                {
                    Destroy(summon);
                }
                summonedUnits.Clear();
                
                // Reset player board and spawn new enemies
                foreach (var unit in fightingUnits) 
                    unit.GetComponent<PlayerUnit>().isActive = false;
                
                ResetPlayerUnitsPosition();
                
                //give round won gold;
                var goldFloat = (GameObject)Instantiate(FloatingText, playerGold.GetComponentInParent<Image>().transform.position + new Vector3( Random.Range(-10f, 10f),Random.Range(-10f, 10f), 0f ), Quaternion.identity, playerGold.GetComponentInParent<Image>().transform);
                goldFloat.GetComponent<Text>().text = "+5";
                _playerController.EditGold(5);
                
                fightOver = true;
                DeployEnemyBoard();
                
                stageText.text = _stage.ToString();
                _shopManager.RandomizeShop(false);
                boardResetTimer = 0f;
                _sheetsForUnity.AppendToSheet("FightDuration", "B:B", new List<object>() { PlayerPrefs.GetString("MatchID"), _unitCount, _stage-1, _fightTimer, result});

            }

            if (fightOver)
            {
                
                var unitsThatWon = new List<object>();
                foreach (var unit in fightingUnits)
                {
                    unitsThatWon.Add(unit.GetComponent<PlayerUnit>().unitName);
                }
                _sheetsForUnity.AppendToSheet("UnitsWon", "A:A", unitsThatWon);
                
                fightingUnits.Clear();
            }
        }
        else
        {
            // Check for Lose State: the player doesnt own any more units and has less then <CHEAPES UNIT COST> gold
            if (_ownedUnits.Count <= 0 && _playerController.Gold < 2)
            {
                PlayerLost();
            }
        }
       
        if (_playerController.isFighting)
            _fightTimer += Time.deltaTime;

        playerGold.text = _playerController.Gold.ToString();
    }

    private void BoardResetAfterTime(float time)
    {
        if (boardResetTimer < time)
        {
            boardResetTimer += Time.deltaTime;
        }
        else
        {
            
        }
    }

    private void DeployEnemyBoard()
    {
        
        //Give Player Gold Interest
        _playerController.EditGold( (int)(_playerController.Gold*0.1f) );
        var goldFloat = (GameObject)Instantiate(FloatingText, playerGold.transform.position + new Vector3( Random.Range(-10f, 10f),Random.Range(-10f, 10f), 0f ), Quaternion.identity, playerGold.transform);
        goldFloat.GetComponent<Text>().text = "+" + ((int)(_playerController.Gold * 0.1)).ToString();
        
        if (_stage % 10 != 0)
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
        else
        {
            
            _currentFightBoardIndex = Random.Range(0, enemyPositioningBoss.Length - 1);

            for (var i = 0; i < 32; i++)
                if (enemyPositioningBoss[_currentFightBoardIndex].enemyPositioning[i] != null)
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
                    temp.enemyClass = enemyPositioningBoss[_currentFightBoardIndex].GetEnemyAtIndex(i);
                    temp.InitUnit();

                    enemyBoard[i].unit = newUnit;
                }
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

    public GameObject IsBoardSlotFree(GameObject tile)
    {
        foreach (var boardTile in board) return boardTile.unit;

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
        if (!_playerController.isFighting)
        {
            for (var i = 0; i < 32; i++)
                if (board[i].unit != null)
                {
                    if (fightingUnits.Count < _playerController.level + 1 &&
                        !board[i].unit.gameObject.CompareTag("Summon"))
                    {
                        fightingUnits.Add(board[i].unit);
                    }
                    else if (board[i].unit.gameObject.CompareTag("Summon"))
                    {
                        fightingUnits.Add(board[i].unit);
                    }
                    else
                    {
                        board[i].unit.GetComponent<NavMeshAgent>().Warp(GetBenchFreeSlot().transform.position);
                    }

                }

            if (fightingUnits.Count > 0)
            {
                foreach (var unit in fightingUnits)
                {
                    var _unit = unit.GetComponent<PlayerUnit>();
                    var _ai = _unit.GetComponent<AIController>();
                    _unit.isActive = true;

                    if (_ai != null)
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
        }
        
        var unitsFighting = new List<object>();
        foreach (var unit in fightingUnits)
        {
            unitsFighting.Add(unit.GetComponent<PlayerUnit>().unitName);
        }
        _sheetsForUnity.AppendToSheet("UnitsTotalGames", "A:A", unitsFighting);
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
            PlayerUnit _unit = unit.GetComponent<PlayerUnit>();
            _playerController.EditGold(_unit.unitCost + (_unit.unitLevel-1)*_unit.unitCost/2);
            Debug.Log(unit.GetComponent<PlayerUnit>().unitCost);
        }

        for (int i = 0; i < 32; i++)
            if(board[i].unit == unit)
                SetUnitAtSlot(null, board[i].tile);
            
        _ownedUnits.Remove(unit);

        Destroy(unit);
    }

    public void SellSelectedUnit()
    {
        var _unit = _playerController.selectedUnit.GetComponent<PlayerUnit>(); 
        _playerController.EditGold(_unit.unitCost + (_unit.unitLevel-1)*_unit.unitCost/2);
        
        for (int i = 0; i < 32; i++)
            if(board[i].unit == _playerController.selectedUnit)
                SetUnitAtSlot(null, board[i].tile);
        
        _ownedUnits.Remove(_playerController.selectedUnit);
        
        Destroy(_playerController.selectedUnit);
        _playerController.selectedUnit = null;
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
                            unit.GetComponent<AIController>().ability1.ResetCD();
                            unit.GetComponent<PlayerUnit>()._attackSpeed =
                                unit.GetComponent<PlayerUnit>().UnitClass.AttackSpeed +
                                unit.GetComponent<PlayerUnit>().UnitClass.ASPerLevel;
                        }
    
    }

    public int GetUnitIndexOnBoard(GameObject _unit)
    {
        int index = 0;

        for (index = 0; index < 32; index++)
        {
            if (board[index].unit == _unit)
            {
                return index;
            }
        }
        
        return -1;
    }

    public void BuyExp()
    {
        if (_playerController.Gold >= 2)
        {
            _playerController.Gold -= 2;
            _playerController.GainExp(2);
        }
    }

    private void PlayerLost()
    {
        SceneManager.LoadScene(0);
    }
    
    [Serializable]
    public struct BoardTile
    {
        public GameObject tile;
        public GameObject unit;
    }
}