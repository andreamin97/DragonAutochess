using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public AIProfile profile;
    private BoardManager _boardManager;
    private NavMeshAgent _navMeshAgent;
    private EnemyUnit _target;
    private PlayerUnit _unit;
    private float distance = float.PositiveInfinity;
    private float nextAttack;
    private GoogleSheetsForUnity _sheetsForUnity;

    private void Start()
    {
        _sheetsForUnity = FindObjectOfType<GoogleSheetsForUnity>();
        _boardManager = FindObjectOfType<BoardManager>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _unit = GetComponent<PlayerUnit>();
    }

    private void Update()
    {
        if (!_navMeshAgent)
            _navMeshAgent = GetComponent<NavMeshAgent>();

        if (_unit.isActive)
        {
            if (_target == null)
                _target = profile.AcquireTarget(_boardManager.EnemyList(), transform.position)
                    .GetComponent<EnemyUnit>();

            distance = Vector3.Distance(_target.transform.position, transform.position);

            if (distance <= _unit.attackRange)
            {
                _navMeshAgent.SetDestination(transform.position);
                if (nextAttack <= 0f)
                {
                    Debug.Log("Attacking " + _target.name);
                    _target.TakeDamage(_unit._attackDamage);
                    nextAttack = _unit._attackSpeed;
                }
            }
            else if (distance > _unit.attackRange)
            {
                _navMeshAgent.SetDestination(_target.transform.position);
            }
            
            transform.LookAt(_target.transform.position);
            transform.Rotate(new Vector3(0f, -90f, 0f));
            
        }

        nextAttack -= Time.deltaTime;
    }

    public void ResetUnit(Vector3 position)
    {
        _navMeshAgent.Warp(position);
    }
}