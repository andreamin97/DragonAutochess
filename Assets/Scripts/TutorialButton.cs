using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    private TutorialManager _tutorialManager;
    public int NextTutorialIndex;
    private void Awake()
    {

        _tutorialManager = FindObjectOfType<TutorialManager>();

    }

    public void NextTutorial()
    {
        
        if (_tutorialManager.TutorialIndex == NextTutorialIndex - 1)
        {
            _tutorialManager.NextTutorial();
        }
    }
}
