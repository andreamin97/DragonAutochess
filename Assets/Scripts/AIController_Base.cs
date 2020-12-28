using System.Collections.Generic;
using UnityEngine;

public class AIController_Base : MonoBehaviour
{
    public BaseUnit.Range Range;

    public List<abilityStruct> abilityList;
    protected GameObject attackFX;

    private void Awake()
    {
        attackFX = (GameObject) Resources.Load("VFX/FX_BloodExplosion_AB");
        abilityList = new List<abilityStruct>();

        var abilities = GetComponents<Ability>();

        foreach (var abi in abilities)
        {
            Debug.Log(abi.abilityName);
            var temp = new abilityStruct(abi, abi.coolDown, false);
            abilityList.Add(temp);
        }
    }

    public struct abilityStruct
    {
        public Ability ability;
        public float coolDown;
        public bool isCasting;

        public abilityStruct(Ability abi, float cD, bool casting)
        {
            ability = abi;
            coolDown = cD;
            isCasting = casting;
        }
    }
}