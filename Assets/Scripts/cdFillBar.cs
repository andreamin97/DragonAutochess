using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cdFillBar : MonoBehaviour
{
    public Unit unit;
    private AIController controller;
    public Slider _cdSlider;
    private Ability _ability;
    
    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        controller = unit.GetComponent<AIController>();
        _ability = controller.abilityList[0].ability;
        _cdSlider.value = _ability.currentCd / _ability.coolDown;
    }
}
