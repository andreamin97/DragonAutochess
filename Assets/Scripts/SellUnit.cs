using UnityEngine;

public class SellUnit : MonoBehaviour
{
    private PlayerController _playerController;
    private BoardManager boardManager;

    private void Awake()
    {
        boardManager = FindObjectOfType<BoardManager>();
        _playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (Physics.CheckBox(transform.position, transform.localScale * 1.5f, Quaternion.identity,
            LayerMask.GetMask("Minis")) && !_playerController.isDragging)
            boardManager.RemoveUnit(_playerController.selectedUnit, true);
    }
}