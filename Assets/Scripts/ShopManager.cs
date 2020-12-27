using System;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public Slot[] slots;
    public GameObject[] unitsList;
    public Canvas canvas;

    private bool isShopOpen;
    private BaseUnit unit;
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
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
        var rand = new System.Random();
        
        if (rerolling && _playerController.Gold >= 2)
        {
            _playerController.EditGold(-2);
            foreach (var slot in slots)
            {
                var unit = unitsList[rand.Next(0, unitsList.Length)];
                slot.Enable();
                slot.UpdateSlot(unit);
            }
        }
        else if (rerolling && _playerController.Gold < 2) {}
        else
        {
            foreach (var slot in slots)
            {
                var unit = unitsList[rand.Next(0, unitsList.Length)];
                slot.Enable();
                slot.UpdateSlot(unit);
            }
        }
    }

    public void CloseShop()
    {
        canvas.gameObject.SetActive(false);
        isShopOpen = false;
    }
}