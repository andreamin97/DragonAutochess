using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private EnemyUnit unit;
    private GameObject sphere;
    private SphereCollider _collider;
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Rigidbody _rigidbody;

    private void Start()
    {
        Physics.IgnoreLayerCollision(9, 9);
    }

    private void OnCollisionEnter(Collision other)
    {
        if ((other.gameObject.CompareTag("Ground")));
        {
            var colls = Physics.OverlapSphere(other.gameObject.transform.position, 3f);

            foreach (var coll in colls)
            {
                if (coll.GetComponent<EnemyUnit>() != null)
                {
                    coll.GetComponent<EnemyUnit>().TakeDamage(50f);
                    Debug.Log("HIT");
                }
            }
            
            Destroy(this.gameObject);
        }
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

}