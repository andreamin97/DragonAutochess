using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Singularity_PF : MonoBehaviour
{
    private float _delay = 1.5f;
    private float _time = 0f;
    private BoardManager _boardManager;
    private SphereCollider _collider;
    private float pullTimer = 0f;
    private bool ispulling = false;
    private Collider[] colliders;
    private void Start()
    {
        _boardManager = FindObjectOfType<BoardManager>();
        _collider = GetComponent<SphereCollider>();
        _time = 0f;
    }

    void Update()
    {
        // transform.Rotate(Vector3.right, 2.5f);

        if (_time < _delay)
        {
            _time += Time.deltaTime;
            
            colliders = Physics.OverlapSphere(transform.position, 5f);

            foreach (var coll in colliders)
            {
                if (coll.GetComponent<EnemyUnit>() != null)
                {
                    coll.transform.position = Vector3.MoveTowards(coll.transform.position, this.gameObject.transform.position, 1f*Time.deltaTime);
                    coll.GetComponent<EnemyAIController>().SetCondition(Unit.Statuses.Snared, .1f);
                } 
            }
            
        }
        else
        {
            
            foreach (var coll in colliders)
            {
                if (coll.GetComponent<EnemyUnit>() != null)
                {
                    coll.GetComponent<Unit>().TakeDamage(7f);
                } 
            }
            Destroy(this.gameObject);
        }
        
    }
}