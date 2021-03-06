﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    [Header("Base Unit Stats")] public float attackRange = 2f;

    public float _attackSpeed;
    public float _attackDamage;
    public float movementSpeed = 3f;

    public bool isActive;

    public bool isFighting;
    [SerializeField]public float armor;
    protected float _mRes;
    protected BoardManager boardManager;
    public float currentHealth;
    public float maxHealth;
    public float leech = 0f;

    public float _conditionMaxDuration;
    public float _conditionDuration;
    
    protected MeshFilter meshFilter;

    public string unitName;
    
    public enum Statuses
    {
        None,
        Snared
    }

    public Statuses currentStatus = Statuses.None;

    protected virtual void Awake()
    {
        boardManager = FindObjectOfType<BoardManager>();
        meshFilter = GetComponent<MeshFilter>();
        
    }

    public virtual void TakeDamage(float damage)
    {
        if (damage > 0f)
        {
            currentHealth -= Mathf.Clamp(damage - armor, 1f, maxHealth);
        }
        else if (damage < 0f)
        {
            currentHealth = Mathf.Clamp(currentHealth -= damage, 0f, maxHealth);
        }
    }
}