using UnityEngine;

[CreateAssetMenu(menuName = "Board")]
public class Board : ScriptableObject
{
    public GameObject[] enemyPositioning;

    public GameObject GetEnemyAtIndex(int i)
    {
        return enemyPositioning[i];
    }
}