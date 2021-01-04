﻿using UnityEngine;
using UnityEngine.AI;

public class Enrage : Ability
{
    private Unit _unit;
    private float healthPercent = 1f;

    public override bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController_Base controller)
    {
        if (_unit == null)
            _unit = controller.GetComponent<Unit>();

        healthPercent = _unit.currentHealth / _unit.maxHealth;
        _unit.leech = Mathf.Clamp(1f - healthPercent, 0f, .7f);

        //it's a passive, always returns true
        return true;
    }
}