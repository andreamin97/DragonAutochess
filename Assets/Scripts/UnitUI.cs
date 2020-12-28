using UnityEngine;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour
{
    public Canvas _canvas;
    public Slider _slider;
    public Unit _unit;
    public Text status;
    public Text name;
    public Image statusDuration;
    public GameObject FloatingText;
    private BoardManager _boardManager;

    private AIController_Base controller;
    private string unitName;

    // Start is called before the first frame update
    private void Start()
    {
        //_canvas = GetComponent<Canvas>();
        //_slider = GetComponent<Slider>();
        //_unit = GetComponent<PlayerUnit>();
        _boardManager = FindObjectOfType<BoardManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        _canvas.transform.rotation = Quaternion.Euler(30f, 90f, 0f);
        _slider.value = _unit.currentHealth / _unit.maxHealth;

        if (_boardManager.IsUnitBenched(gameObject) && !_unit.isActive)
        {
            if (unitName != null)
                unitName = gameObject.GetComponent<PlayerUnit>().UnitClass.name;

            name.text = unitName;
        }
        else
        {
            switch (_unit.currentStatus)
            {
                case Unit.Statuses.None:
                    status.text = " ";
                    statusDuration.fillAmount = 0f;
                    break;
                case Unit.Statuses.Snared:
                    status.text = "Snared";
                    statusDuration.fillAmount = _unit._conditionDuration / _unit._conditionMaxDuration;
                    break;
            }
        }
    }

    public void SpawnFloatingText(string text, Color color)
    {
        var dmgFloat = Instantiate(FloatingText, transform);
        dmgFloat.GetComponent<Text>().text = text;
        dmgFloat.GetComponent<Text>().color = color;
    }
}