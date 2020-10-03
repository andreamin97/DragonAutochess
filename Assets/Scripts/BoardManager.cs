using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public struct BoardTile
    {
        public GameObject tile;
        public GameObject unit;
    }

    public BoardTile[] bench;
    [SerializeField] private LayerMask unitMask;

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
    }

    public GameObject IsSlotFree(GameObject tile)
    {
        foreach (BoardTile boardTile in bench)
        {
            return boardTile.unit;
        }

        return null;
    }
}

