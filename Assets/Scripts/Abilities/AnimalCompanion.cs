using UnityEngine;
using UnityEngine.AI;

public class AnimalCompanion : Ability
{
    private object _fb;
    private float bonus = .1f;
    private readonly float cd = .2f;
    private GameObject obj;
    private float perLevel = 0.1f;
    private GameObject wolf;

    public override bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController controller)
    {
        if (!castOnce)
        {
            if (_fb == null)
                _fb = Resources.Load("Wolf");

            if (wolf == null)
                Spawn(boardManager);
            else
                wolf.GetComponent<PlayerUnit>().currentHealth = wolf.GetComponent<PlayerUnit>().maxHealth;


            foreach (var unit in boardManager.enemyFightingUnits)
            {
                var _controller = unit.GetComponent<EnemyAIController>();

                if (_controller.Target.gameObject == gameObject) _controller.Target = wolf.GetComponent<PlayerUnit>();
            }

            castOnce = true;
        }

        currentCd = cd;
        return true;
    }

    private void Spawn(BoardManager boardManager)
    {
        var ownerIndex = boardManager.GetUnitIndexOnBoard(gameObject);

        if (ownerIndex != -1)
        {
            for (var i = ownerIndex + 1; i < 32; i++)
                if (boardManager.IsBoardSlotFree(boardManager.board[i].tile) == null)
                {
                    // Found a free slot

                    obj = (GameObject) _fb;

                    NavMeshHit hitPosition;
                    NavMesh.SamplePosition(boardManager.board[i].tile.transform.position + Vector3.up, out hitPosition,
                        1f, NavMesh.AllAreas);

                    wolf = Instantiate(obj, hitPosition.position, Quaternion.identity);
                    wolf.GetComponent<PlayerUnit>().InitUnit();

                    wolf.GetComponent<NavMeshAgent>().enabled = true;
                    wolf.GetComponent<PlayerUnit>().isActive = true;
                    boardManager.fightingUnits.Add(wolf);
                    boardManager.summonedUnits.Add(wolf);

                    return;
                }

            if (wolf == null)
                for (var i = ownerIndex - 1; i >= 0; i--)
                    if (boardManager.IsBoardSlotFree(boardManager.board[i].tile) == null)
                    {
                        // Found a free slot

                        obj = (GameObject) _fb;

                        NavMeshHit hitPosition;
                        NavMesh.SamplePosition(boardManager.board[i].tile.transform.position + Vector3.up,
                            out hitPosition, 1f, NavMesh.AllAreas);

                        var wolfUnit = wolf.GetComponent<PlayerUnit>();

                        wolf = Instantiate(obj, hitPosition.position, Quaternion.identity);
                        wolfUnit.unitLevel = gameObject.GetComponent<PlayerUnit>().unitLevel;
                        wolfUnit.InitUnit();

                        wolf.GetComponent<NavMeshAgent>().enabled = true;
                        boardManager.board[i].unit = wolf;
                        wolf.GetComponent<PlayerUnit>().isActive = true;
                        boardManager.fightingUnits.Add(wolf);
                        boardManager._ownedUnits.Add(wolf);

                        return;
                    }
        }
    }
}