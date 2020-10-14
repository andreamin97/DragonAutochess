using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    [Header("Base Unit Stats")]
    public float attackRange = 2f;
    public float _attackSpeed;
    public float _attackDamage;
    
    public bool isActive = false;
    
    protected MeshFilter meshFilter;
    protected BoardManager boardManager;

    protected string unitName;
    protected float maxHealth;
    protected float currentHealth;
    protected float armor;
    
    public bool isFighting = false;

    protected virtual void Awake()
    {
        boardManager = FindObjectOfType<BoardManager>();
        meshFilter = GetComponent<MeshFilter>();
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= (damage-armor);
        
        if(currentHealth<=0f)
            Destroy(this.gameObject);
    }
}
