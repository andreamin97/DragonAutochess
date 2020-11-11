using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class Projectile_Base : MonoBehaviour
{
    public EnemyUnit target;
    private float _speed = 0.05f;
    public float damage;
    public GameObject VFX;

    private float destroyTimer = 0f;

    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position + Vector3.up, _speed);
            destroyTimer = 0f;
        }
        else
        {
            if (destroyTimer >= 0.3f)
            {
                Destroy(this.gameObject);
            }
            else
            {
                destroyTimer += Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target.gameObject)
        {
            target.TakeDamage(damage);
            Instantiate(VFX, target.transform);
            Destroy(this.gameObject);
        }
    }
}
