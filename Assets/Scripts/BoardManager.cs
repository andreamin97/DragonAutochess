using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

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
    public Board enemyPositioning;
    private PlayerController _playerController;

    private List<GameObject> _ownedUnits = new List<GameObject>();

    public GameObject baseEnemy;

    private void Awake()
    {
        _playerController = Camera.main.GetComponent<PlayerController>();
    }

    private void Start()
    {
        for (int i = 0; i < 32; i++)
        {
            if (enemyPositioning.enemyPositioning[i] != null)
            {
                Vector3 spawnPosition = enemyBoard[i].tile.transform.position + Vector3.up;
                
                GameObject newUnit = Instantiate(baseEnemy, spawnPosition, Quaternion.identity); 

                NavMeshHit navHit;
                if (NavMesh.SamplePosition(spawnPosition, out navHit, 5, -1))
                {
                    newUnit.transform.position = navHit.position;
                    newUnit.GetComponent<NavMeshAgent>().enabled = true;
                }
                newUnit.transform.rotation = Quaternion.Euler(0f,180f, 0f);
                EnemyUnit temp = newUnit.GetComponent<EnemyUnit>();
                temp.enemyClass = enemyPositioning.GetEnemyAtIndex(i);
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

        Debug.Log("Bench Full");
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
        foreach (BoardTile tile in board)
        {
            if (tile.unit != null)
            {
                tile.unit.GetComponent<PlayerUnit>().isActive = true;
            }
        }
        foreach (BoardTile tile in enemyBoard)
        {
            if (tile.unit != null)
            {
                tile.unit.GetComponent<EnemyUnit>().isActive = true;
            }
        }
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
}

