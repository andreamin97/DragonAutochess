using UnityEngine;

[CreateAssetMenu(menuName = "Board")]
public class Board : ScriptableObject
{
    public BaseEnemy[] enemyPositioning;

    public BaseEnemy GetEnemyAtIndex(int i)
    {
        return enemyPositioning[i];
    }
}