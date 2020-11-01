using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private EnemyUnit unit;
    private GameObject sphere;
    private SphereCollider _collider;
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Rigidbody _rigidbody;

    private void OnCollisionEnter(Collision other)
    {
        if (unit = other.gameObject.GetComponent<EnemyUnit>())
        {
            unit.TakeDamage(50f);
        }
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

}
