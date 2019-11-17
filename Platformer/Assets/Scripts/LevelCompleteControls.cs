using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelCompleteControls : MonoBehaviour
{
    private int selectionNum;
    private bool LeftPressed;
    private bool RightPressed;
    private string[] buttonNames = {"Retry Button", "Continue Button", "Exit Button"};
    public string nextLevelScene;
    public TimeSpan timeToComplete;
    public Transform selection;
    // don't interpret axis movement as button press unless it goes under the threshold before pressing again
    public float inputThreshold;

    void ChangeSelection(int amount) {
        int newSelection = selectionNum + amount;
        if (newSelection >= 0 && newSelection < buttonNames.Length)
            selectionNum = newSelection;
    }

    void Retry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    void Continue() {
        SceneManager.LoadScene(nextLevelScene, LoadSceneMode.Single);
    }

    void Exit() {
        Application.Quit();
    }

    void Start()
    {
        LeftPressed = true;
        RightPressed = true;
        selectionNum = 1;
        // set completion time text to the proper value
        TextMeshProUGUI text = transform.Find("Completion Time Text").gameObject.GetComponent<TextMeshProUGUI>();
        text.SetText(String.Format("Time to complete:\n{0}:{1:00}.{2}",
                     Math.Floor(timeToComplete.TotalMinutes),
                     timeToComplete.Seconds,
                     timeToComplete.Milliseconds));
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
        if (Input.GetButtonDown("Jump")) {
            // unfreeze time
            Time.timeScale = 1;
            switch (selectionNum) {
                case 0: Retry(); break;
                case 1: Continue(); break;
                case 2: Exit(); break;
            }
        }
    }
}
