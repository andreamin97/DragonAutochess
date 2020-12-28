using System;
using UnityEngine;
using Random = System.Random;

public class ShopManager : MonoBehaviour
{
    public Slot[] slots;
    public GameObject[] commonUnitsList;
    public AnimationCurve commonDropRate;
    public GameObject[] epicUnitsList;
    public AnimationCurve epicDropRate;
    public Canvas canvas;

    private bool isShopOpen;
    private BaseUnit unit;
    private PlayerController _playerController;
    private Random rand;

    private void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
        rand = new Random();
    }

    private void Start()
    {
        canvas.gameObject.SetActive(false);
        RandomizeShop(false);
    }

    public void ToggleShopUI()
    {
        isShopOpen = !isShopOpen;
        canvas.gameObject.SetActive(isShopOpen);
    }

    public void RandomizeShop(bool rerolling)
    {
        
        
        if (rerolling && _playerController.Gold >= 2)
        {
            _playerController.EditGold(-2);

            RandomizeSlots();
        }
        else if (rerolling && _playerController.Gold < 2) {}
        else
        {
            RandomizeSlots();
        }
    }
    
    void RandomizeSlots()
    {
        GameObject unit = null;
        
        foreach (var slot in slots)
        {
            double dropPercent = rand.NextDouble();
    
            if (dropPercent <= commonDropRate.Evaluate(_playerController.level))
            {
                unit = commonUnitsList[rand.Next(0, commonUnitsList.Length)];
            }
            else if (dropPercent <= epicDropRate.Evaluate(_playerController.level))
            {
                unit = epicUnitsList[rand.Next(0, epicUnitsList.Length)];
            }
    
            slot.Enable();
            slot.UpdateSlot(unit);
        }
    }

    public void CloseShop()
    {
        canvas.gameObject.SetActive(false);
        isShopOpen = false;
    }
}