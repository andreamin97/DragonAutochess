using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Singularity_PF : MonoBehaviour
{
    private float _delay = 3f;
    private float _time = 0f;
    private BoardManager _boardManager;
    private SphereCollider _collider;
    private void Start()
    {
        _boardManager = FindObjectOfType<BoardManager>();
        _collider = GetComponent<SphereCollider>();
        Debug.Log("Hello");
    }

    void Update()
    {
        transform.Rotate(Vector3.right, 2.5f);

        if (_time < _delay)
        {
            _time += Time.deltaTime;
        }
        else
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);
            foreach (var coll in colliders)
            {
                if (coll.gameObject.GetComponent<EnemyUnit>())
                {
                    coll.gameObject.GetComponent<NavMeshAgent>().Warp(transform.position);
                    coll.gameObject.GetComponent<EnemyUnit>().TakeDamage(7f);
                } 
            }
            
            Destroy(this.gameObject);
        }
    }
}
