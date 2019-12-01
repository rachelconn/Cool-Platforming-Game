using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTutorialText : MonoBehaviour
{
    // make variable for each message that has a button name in it
    public GameObject moveReminder;
    public GameObject jumpReminder;
    public GameObject dashReminder;

    // update text of tutorial message prompts based on keybindings
    void Update()
    {
        string jumpButton = InputManager.GetKeyNameFor("Jump");
        string dashButton = InputManager.GetKeyNameFor("Dash");
        string leftButton = InputManager.GetKeyNameFor("Left");
        string rightButton = InputManager.GetKeyNameFor("Right");
        string settingsButton = InputManager.GetKeyNameFor("Settings");
        moveReminder.GetComponent<ContextualReminder>().SetMessage(
            string.Format("You can move using the {0} and {1} buttons.",
            leftButton, rightButton));
        jumpReminder.GetComponent<ContextualReminder>().SetMessage(
            string.Format("Jump by pressing {0}.", jumpButton));
        dashReminder.GetComponent<ContextualReminder>().SetMessage(
            string.Format("Try dashing by pressing {0} and holding a direction",
            dashButton));
    }
}
