using System.Collections.Generic;
using UnityEngine;

public abstract class AIProfile : ScriptableObject
{
    public abstract GameObject AcquireTarget(List<GameObject> unitList, Vector3 position);
}