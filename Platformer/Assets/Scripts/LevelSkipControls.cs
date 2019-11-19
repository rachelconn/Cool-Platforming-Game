using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSkipControls : MonoBehaviour
{
    private int selectionNum;
    private bool LeftPressed;
    private bool RightPressed;
    private string[] buttonNames = {"No Button", "Yes Button"};
    public string nextLevelScene;
    public TimeSpan timeToComplete;
    public Transform selection;
    // don't interpret axis movement as button press unless it goes under the threshold before pressing again
    public float inputThreshold;

    void ChangeSelection(int amount) {
        int newSelection = selectionNum + amount;
        if (newSelection >= 0 && newSelection < buttonNames.Length)
            selectionNum = newSelection;
        Debug.Log(selectionNum);
    }

    void Retry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    void Continue() {
        string nextLevelScene = GameObject.Find("Goal").GetComponent<Goal>().nextLevelScene;
        SceneManager.LoadScene(nextLevelScene, LoadSceneMode.Single);
    }

    void Start()
    {
        LeftPressed = true;
        RightPressed = true;
        selectionNum = 0;
    }

    void Update()
    {
        // filter input so user has to press left/right every time selection changes
        float inputDir = Input.GetAxisRaw("Horizontal");
        if (LeftPressed)
            if (inputDir > -inputThreshold)
                LeftPressed = false;
        if (RightPressed)
            if (inputDir < inputThreshold)
                RightPressed = false;
        if (!LeftPressed && inputDir <= -inputThreshold) {
            ChangeSelection(-1);
            LeftPressed = true;
        }
        if (!RightPressed && inputDir >= inputThreshold) {
            ChangeSelection(1);
            RightPressed = true;
        }

        // update position of selection
        selection.position = transform.Find(buttonNames[selectionNum]).position;

        // if player presses jump, confirm selection
        if (InputManager.GetButtonDown("Jump") || InputManager.GetButtonDown("Select")) {
            // unfreeze time
            Time.timeScale = 1;
            switch (selectionNum) {
                // No, don't skip:
                case 0: Retry(); break;
                // Yes, skip:
                case 1: Continue(); break;
            }
        }
    }
}
