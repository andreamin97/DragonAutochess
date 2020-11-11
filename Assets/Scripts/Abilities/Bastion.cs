﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bastion : Ability 
{
    private float _cd = 8f;
    private float _duration = 4f;
    [SerializeField]private float _time = 0f;
    private bool isCasting = false;
    private PlayerUnit _unit = null;
    private float _bonusArmor = 0f;
    public GameObject vEffect = null;
    private GameObject effect;
                                   
    private void Start()
    {
        vEffect = (GameObject) Resources.Load("VFX/CFX4 Aura Bubble C");
    }
    public override bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController controller)
    {
        if (_unit == null)
            _unit = controller.GetComponent<PlayerUnit>();
        
        if (!isCasting)
        {
            _bonusArmor = 2f + 1f * _unit.unitLevel;
            _unit.AddArmor(_bonusArmor);
            effect = Instantiate(vEffect, this.gameObject.transform);
            isCasting = true;
        }
        
        if (isCasting && _time < _duration)
        {
            _time += Time.deltaTime;
            controller.SetCondition(Unit.Statuses.Snared, .05f);
            return isCasting = true;
        }
        
        _unit.AddArmor(-(_bonusArmor));
        Destroy(effect);
        _time = 0f;
        currentCd = _cd;
        return isCasting = false;
    }
}