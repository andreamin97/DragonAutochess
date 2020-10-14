using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public struct BoardTile
    {
        public GameObject tile;
        public GameObject unit;
    }

    public BoardTile[] board;
    public BoardTile[] bench;
    public BoardTile[] enemyBoard;
    public Board[] enemyPositioning;
    private PlayerController _playerController;

    public List<GameObject> _ownedUnits = new List<GameObject>();
    public List<GameObject> fightingUnits = new List<GameObject>();
    public List<GameObject> enemyFightingUnits = new List<GameObject>();

    public GameObject baseEnemy;

    private void Awake()
    {
        _playerController = Camera.main.GetComponent<PlayerController>();
    }

    private void Start()
    {
        DeployEnemyBoard();
    }

    private void Update()
    {
        if (_playerController.isFighting && (fightingUnits.Count <= 0 || EnemyList().Count <= 0))
        {
            foreach (GameObject unit in fightingUnits)
            {
                unit.GetComponent<PlayerUnit>().isActive = false;
            }
            foreach (GameObject unit in enemyFightingUnits)
            {
                unit.GetComponent<EnemyUnit>().isActive = false;
            }

            _playerController.isFighting = false;
            ResetPlayerUnitsPosition();
            DeployEnemyBoard();
        }
    }

    void DeployEnemyBoard()
    {
        int boardIndex = UnityEngine.Random.Range(0, enemyPositioning.Length-1);
        
        for (int i = 0; i < 32; i++)
        {
            if (enemyPositioning[boardIndex].enemyPositioning[i] != null)
            {
                Vector3 spawnPosition = enemyBoard[i].tile.transform.position + Vector3.up;

                GameObject newUnit = Instantiate(baseEnemy, spawnPosition, Quaternion.identity);

                NavMeshHit navHit;
                if (NavMesh.SamplePosition(spawnPosition, out navHit, 5, -1))
                {
                    newUnit.transform.position = navHit.position;
                    newUnit.GetComponent<NavMeshAgent>().enabled = true;
                }

                newUnit.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                EnemyUnit temp = newUnit.GetComponent<EnemyUnit>();
                temp.enemyClass = enemyPositioning[boardIndex].GetEnemyAtIndex(i);
                temp.InitUnit();

                enemyBoard[i].unit = newUnit;
            }
        }
    }

    public GameObject GetBenchFreeSlot()
    {
        foreach (BoardTile tile in bench)
        {
            if (tile.unit == null)
                return tile.tile;
        }
        
        return null;
    }

    public void SetUnitAtSlot(GameObject unit, GameObject tile)
    {
        for (int i = 0; i < bench.Length; i++)
        {
            if (bench[i].tile == tile)
            {
                bench[i].unit = unit;
            }
        }
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i].tile == tile)
            {
                board[i].unit = unit;
            }
        }
    }

    public GameObject IsSlotFree(GameObject tile)
    {
        foreach (BoardTile boardTile in bench)
        {
            return boardTile.unit;
        }

        return null;
    }

    public bool IsUnitBenched(GameObject unit)
    {
        foreach (BoardTile tile in bench)
        {
            if (tile.unit == unit)
                return true;
        }

        return false;
    }

    public void StartEncounter()
    {
        for (int i = 0; i < 32; i++)
        {
            if (board[i].unit != null)
            {
                board[i].unit.GetComponent<PlayerUnit>().isActive = true;
                fightingUnits.Add(board[i].unit);
            }
        }
        
        for (int i = 0; i < 32; i++)
        {
            if (enemyBoard[i].unit != null)
            {
                enemyBoard[i].unit.GetComponent<EnemyUnit>().isActive = true;
                enemyFightingUnits.Add(enemyBoard[i].unit);
            }
        }

        _playerController.isFighting = true;
    }

    public List<GameObject> EnemyList()
    {
        List<GameObject> tempList = new List<GameObject>();
        
        foreach (BoardTile tile in enemyBoard)
        {
            if (tile.unit != null)
                tempList.Add(tile.unit.gameObject);
        }

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
        switch (wasSold)
        {
            case true:
                _playerController.Gold += unit.GetComponent<PlayerUnit>().unitCost;
                _ownedUnits.Remove(unit);
                break;
            case false:
                _ownedUnits.Remove(unit);
                break;
        }
    }

    private void ResetPlayerUnitsPosition()
    {
        if (fightingUnits.Count > 0)
        {
            foreach (GameObject unit in fightingUnits)
            {
                if (unit != null)
                {
                    for (int i = 0; i < 32; i++)
                    {
                        if (unit == board[i].unit)
                        {
                            unit.GetComponent<AIController>().ResetUnit(board[i].tile.transform.position);
                        }
                    }
                }
            }
                
        }
    }
}

