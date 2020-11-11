using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour
{
    public Canvas _canvas;
    public Slider _slider;
    public Unit _unit;
    public Text status;
    public Text name;
    public Image statusDuration;

    private AIController_Base controller;
    private BoardManager _boardManager;
    private string unitName = null;

    // Start is called before the first frame update
    void Start()
    {
        //_canvas = GetComponent<Canvas>();
        //_slider = GetComponent<Slider>();
        //_unit = GetComponent<PlayerUnit>();
        _boardManager = FindObjectOfType<BoardManager>();
    }

    // Update is called once per frame
    void Update()
    {
        _canvas.transform.rotation = Quaternion.Euler(30f, 90f, 0f);
        _slider.value = _unit.currentHealth / _unit.maxHealth;
        
        if (_boardManager.IsUnitBenched(this.gameObject) && !_unit.isActive)
        {
            if (unitName != null)
                unitName = this.gameObject.GetComponent<PlayerUnit>().UnitClass.name;
            
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
}