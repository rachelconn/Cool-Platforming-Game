using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSkipControls : MonoBehaviour
{
    private int selectionNum;
    private bool LeftPressed;
    private bool RightPressed;
    private string[] buttonNames = {"No Button", "Yes Button", "Never Button"};
    public string nextLevelScene;
    public TimeSpan timeToComplete;
    // public Transform selection;
    // don't interpret axis movement as button press unless it goes under the threshold before pressing again
    public float inputThreshold;

    public Sprite spr_selected;
    public Sprite spr_deselected;

    private Image lastSelected = null;

    void ChangeSelection(int amount) {
        int newSelection = selectionNum + amount;
        if (newSelection >= 0 && newSelection < buttonNames.Length)
            selectionNum = newSelection;
        Debug.Log(selectionNum);
    }

    public void Retry() {
        // unfreeze time
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void Continue() {
        // unfreeze time
        Time.timeScale = 1;
        string nextLevelScene = GameObject.Find("Goal").GetComponent<Goal>().nextLevelScene;
        SceneManager.LoadScene(nextLevelScene, LoadSceneMode.Single);
    }

    public void DisableSkipDialogue()
    {
        InputManager.Pref_ShowSkipDialogue = false;
        Retry();
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
        // selection.position = transform.Find(buttonNames[selectionNum]).position;
        if (lastSelected != null)
        {
            lastSelected.sprite = spr_deselected;
        }
        lastSelected = GameObject.Find(buttonNames[selectionNum]).GetComponent<Image>();
        lastSelected.sprite = spr_selected;

        // if player presses jump, confirm selection
        if (InputManager.GetButtonDown("Jump") || InputManager.GetButtonDown("Select")) {
            switch (selectionNum) {
                // No, don't skip:
                case 0: Retry(); break;
                // Yes, skip:
                case 1: Continue(); break;
                case 2: DisableSkipDialogue(); break;
            }
        }
    }
}
