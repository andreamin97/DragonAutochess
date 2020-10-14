using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    public BaseEnemy enemyClass;
    private EnemyAIController _aiController;

    protected override void Awake()
    {
        base.Awake();
        _aiController = GetComponent<EnemyAIController>();
    }
    
    public void InitUnit()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = enemyClass.Mesh;
        maxHealth = enemyClass.Health;
        currentHealth = maxHealth;
        armor = enemyClass.Armor;
        _attackDamage = enemyClass.AttackDamage;
        _attackSpeed = enemyClass.AttackSpeed;
        attackRange = enemyClass.AttackRange;
       _aiController.profile = enemyClass._aiProfile;
    }

    public override void TakeDamage(float damage)
    {
        if(currentHealth-damage <=0 )
            boardManager.enemyFightingUnits.Remove((this.gameObject));
        base.TakeDamage(damage);
        
    }
}
