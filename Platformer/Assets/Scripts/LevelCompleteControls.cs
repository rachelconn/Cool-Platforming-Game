using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteControls : MonoBehaviour
{
    private int selectionNum;
    private bool LeftPressed;
    private bool RightPressed;
    private string[] buttonNames = {"Retry Button", "Continue Button", "Exit Button"};
    public Transform selection;
    // don't interpret axis movement as button press unless it goes under the threshold before pressing again
    public float inputThreshold;

    void ChangeSelection(int amount) {
        int newSelection = selectionNum + amount;
        if (newSelection >= 0 && newSelection < buttonNames.Length)
            selectionNum = newSelection;
    }

    void Start()
    {
        selectionNum = 1;
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
        Debug.Log(selectionNum);
        selection.position = transform.Find(buttonNames[selectionNum]).position;
    }
}
