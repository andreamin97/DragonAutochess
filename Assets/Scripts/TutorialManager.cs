using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    public bool RunTutorial = true;
    public List<GameObject> TutorialWidgets;

    private int tutorialIndex = -1;

    public int TutorialIndex
    {
        get => tutorialIndex;
        set => tutorialIndex = value;
    }

    private void Start()
    {
        PlayTutorial();
    }

    public void NextTutorial()
    {
        tutorialIndex++;
        switch (tutorialIndex)
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
        tutorialIndex = -1;
     
        HideWidgets();
     
        if (RunTutorial)
            NextTutorial();
    }

    private void HideWidgets()
    {
        foreach (var widget in TutorialWidgets)
        {
            widget.SetActive(false);
        }
    }
}