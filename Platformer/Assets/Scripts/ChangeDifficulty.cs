using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeDifficulty : MonoBehaviour
{
    private Dropdown myDropDown;

    // Start is called before the first frame update
    void Start()
    {
        myDropDown = this.GetComponent<Dropdown>();
        myDropDown.onValueChanged.AddListener(delegate { ChangeDiff(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeDiff()
    {
        
        if (myDropDown.value == 0)
        {
            // Easy Mode
            BarrelSpeed.speed = 6;
        }
        else if (myDropDown.value == 1)
        {
            // Normal Mode
            BarrelSpeed.speed = 8;
        }
        else if (myDropDown.value == 2)
        {
            // Hard Mode
            BarrelSpeed.speed = 10;
        }

        Debug.Log("BarrelSpeed: " + BarrelSpeed.speed);
    }
}
