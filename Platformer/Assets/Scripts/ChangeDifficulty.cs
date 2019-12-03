using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeDifficulty : MonoBehaviour
{
    private Dropdown myDropDown;

    private static bool HEURISTIC_BROKEN = false;

    // Start is called before the first frame update
    void Start()
    {
        myDropDown = this.GetComponent<Dropdown>();

        // If the dropdown menu is found call the ChangeDiff() function
        if (myDropDown != null)
        {
            myDropDown.onValueChanged.AddListener(delegate { ChangeDiff(); });
        }
        else
        {
            Debug.LogError("myDropDown not fixed yet");
            ChangeDifficulty.HEURISTIC_BROKEN = true;
            return;
        }

        // Change the value of the dropdown so it shows the right difficulty the next 
        // time the settings menu is open.
        if (BarrelSpeed.speed == 4)
        {
            // Easy Mode
            myDropDown.value = 0;
        }
        else if (BarrelSpeed.speed == 6)
        {
            // Normal Mode
            myDropDown.value = 1;
        }
        else if (BarrelSpeed.speed == 8)
        {
            // Hard Mode
            myDropDown.value = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Change the barrel speeds based off of the difficulty chosen
    void ChangeDiff()
    {
        if (ChangeDifficulty.HEURISTIC_BROKEN)
        {
            return;
        }
        if (myDropDown.value == 0)
        {
            // Easy Mode
            BarrelSpeed.speed = 4;
        }
        else if (myDropDown.value == 1)
        {
            // Normal Mode
            BarrelSpeed.speed = 6;
        }
        else if (myDropDown.value == 2)
        {
            // Hard Mode
            BarrelSpeed.speed = 8;
        }

        Debug.Log("BarrelSpeed: " + BarrelSpeed.speed);
    }
}
