using UnityEngine;
using UnityEngine.AI;

public class Bastion : Ability
{
    [SerializeField] private float _time;
    public GameObject vEffect;
    private float _bonusArmor;
    private readonly float _cd = 8f;
    private readonly float _duration = 4f;
    private PlayerUnit _unit;
    private GameObject effect;
    private bool isCasting;

    private void Start()
    {
        vEffect = (GameObject) Resources.Load("VFX/CFX4 Aura Bubble C");
    }

    public override bool Cast(NavMeshAgent navMeshAgent, BoardManager boardManager, AIController_Base controller)
    {
        if (_unit == null)
            _unit = controller.GetComponent<PlayerUnit>();

        if (!isCasting)
        {
            _bonusArmor = 2f + 1f * _unit.unitLevel;
            _unit.AddArmor(_bonusArmor);
            effect = Instantiate(vEffect, gameObject.transform);
            isCasting = true;
        }

        if (isCasting && _time < _duration)
        {
            _time += Time.deltaTime;
            controller.SetCondition(Unit.Statuses.Snared, .05f);
            return isCasting = true;
        }

        _unit.AddArmor(-_bonusArmor);
        Destroy(effect);
        _time = 0f;
        currentCd = _cd;
        return isCasting = false;
    }
}