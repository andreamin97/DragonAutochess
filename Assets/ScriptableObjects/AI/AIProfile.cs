using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIProfile : ScriptableObject
{
    private Enemy _closestEnemy;

    public abstract GameObject AcquireTarget(List<GameObject> unitList, Vector3 position);

}
