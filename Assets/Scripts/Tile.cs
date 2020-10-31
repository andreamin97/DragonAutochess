using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject selectedOutline;
    private PlayerController _playerController;
    private BoardManager bm;

    private MeshRenderer mr;
    private bool selected;

    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
        bm = FindObjectOfType<BoardManager>();
        _playerController = Camera.main.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (selected)
            selectedOutline.SetActive(true);
        else
            selectedOutline.SetActive(false);
    }

    public void ToggleSelected()
    {
        selected = !selected;
    }
}