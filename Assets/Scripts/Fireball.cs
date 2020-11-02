using System;
using System.Collections;
using System.Collections.Generic;
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
        if ((unit = other.gameObject.GetComponent<EnemyUnit>()) && unit.isActive==true)
        {
            unit.TakeDamage(50f);
            Debug.Log(other.gameObject.GetComponent<EnemyUnit>().enemyClass + "took 50 damage.");
        }

        float f = 0f;
        while (f < 1f)
            f += Time.deltaTime;
        
        Destroy(this.gameObject);
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

}
