using UnityEngine;
using UnityEngine.UI;

public class cdFillBar : MonoBehaviour
{
    public Unit unit;
    public Slider _cdSlider;
    private Ability _ability;
    private AIController controller;

    // Start is called before the first frame update
    private void Awake()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        controller = unit.GetComponent<AIController>();
        _ability = controller.abilityList[0].ability;
        _cdSlider.value = _ability.currentCd / _ability.coolDown;
    }
}