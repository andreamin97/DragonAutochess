using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public BaseEnemy enemy;
    
    private string unitName;
    private MeshFilter meshFilter;
    public AIController profile;

    private float maxHealth;
    private float currentHealth;
    private float armor;

    private void Start()
    {
        InitUnit();
    }

    public void InitUnit()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = enemy.Mesh;
        maxHealth = enemy.Health;
        currentHealth = maxHealth;
        armor = enemy.Armor;
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth -= (damage-armor);
        
        if(currentHealth<=0f)
            Destroy(this.gameObject);
    }
}
