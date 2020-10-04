using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Base Profile ")]
public class AIProfile : ScriptableObject
{
    private Enemy _closestEnemy;
    private float _currentDistance = Mathf.Infinity;
    
    public Enemy FindClosestTarget(BoardManager.BoardTile[] enemyList, Vector3 position)
    {
        foreach (BoardManager.BoardTile tile in enemyList)
        {
            /*if (tile.unit != null)
            {
                if (Mathf.Abs((tile.unit.transform.position - position).magnitude) < _currentDistance)
                {
                    _closestEnemy = tile.unit.GetComponent<Enemy>();
                    _currentDistance = Mathf.Abs((tile.unit.transform.position - position).magnitude);
                }
            }*/

            _closestEnemy = tile.unit.GetComponent<Enemy>();
        }

        return _closestEnemy;
    }
}
