using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.WSA;

public class AIController : MonoBehaviour
{
    enum State { Idle, Moving, Attacking };

    [SerializeField] private State _state = State.Idle;
    private BoardManager _boardManager;
    private NavMeshAgent _navMeshAgent;
    private Enemy _target;
        
    public AIProfile profile;

    private void Awake()
    {
        _boardManager = FindObjectOfType<BoardManager>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        switch (_state)
        {
            case (State.Moving):
            {
                _target = profile.FindClosestTarget(_boardManager.enemyBoard, transform.position);
                _navMeshAgent.SetDestination(_target.transform.position);
                _state = State.Attacking;
                break;
            }
            case (State.Attacking):
                break;
            case  (State.Idle):
                break;
        }
    }

    public void Activate()
    {
        _state = State.Moving;
    }
}
