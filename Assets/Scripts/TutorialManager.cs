using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public bool RunTutorial = true;
    public List<GameObject> TutorialWidgets;

    public int TutorialIndex { get; set; } = -1;

    private void Start()
    {
        PlayTutorial();
    }

    public void NextTutorial()
    {
        TutorialIndex++;
        switch (TutorialIndex)
        {
            case 0:
                TutorialWidgets[0].SetActive(true);
                break;
            case 1:
                TutorialWidgets[0].SetActive(false);
                TutorialWidgets[1].SetActive(true);
                break;
            case 2:
                TutorialWidgets[1].SetActive(false);
                TutorialWidgets[2].SetActive(true);
                break;
            case 3:
                TutorialWidgets[2].SetActive(false);
                TutorialWidgets[3].SetActive(true);
                break;
            case 4:
                TutorialWidgets[3].SetActive(false);
                TutorialWidgets[4].SetActive(true);
                break;
            default:
                HideWidgets();
                PlayerPrefs.SetInt("hasPlayedTutorial", 1);
                break;
        }
    }

    public void PlayTutorial()
    {
        switch (PlayerPrefs.GetInt("hasPlayedTutorial"))
        {
            case 0:
                RunTutorial = true;
                break;
            case 1:
                RunTutorial = false;
                break;
            default:
                RunTutorial = true;
                break;
        }

        PlayerPrefs.GetInt("hasPlayedTutorial");
        TutorialIndex = -1;

        HideWidgets();

        if (RunTutorial)
            NextTutorial();
    }

    private void HideWidgets()
    {
        foreach (var widget in TutorialWidgets) widget.SetActive(false);
    }
}