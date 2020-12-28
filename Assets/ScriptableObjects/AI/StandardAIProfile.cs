﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Base Profile ")]
public class StandardAIProfile : AIProfile
{
    public override GameObject AcquireTarget(List<GameObject> unitList, Vector3 position)
    {
        GameObject _closestUnit = null;
        var distance = float.PositiveInfinity;

        foreach (var unit in unitList)
        {
            _closestUnit = Vector3.Distance(unit.transform.position, position) < distance ? unit : _closestUnit;

            distance = Vector3.Distance(_closestUnit.transform.position, position);
        }

        return _closestUnit;
    }
}