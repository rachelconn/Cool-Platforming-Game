using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class InputManager
{
    public static bool Pref_ShowSkipDialogue = true;

    // Store the KeyCode for each keybind button
    private static Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>()
    {
        {"Jump", KeyCode.Z },
        {"Left", KeyCode.LeftArrow },
        {"Right", KeyCode.RightArrow },
        {"Dash", KeyCode.X },
        {"Up", KeyCode.UpArrow },
        {"Down", KeyCode.DownArrow },
        {"Select", KeyCode.Return },
        {"Settings", KeyCode.P },
    };

    // Check to see if the button entered is being pressed.
    public static bool GetButtonDown(string name)
    {
        if (!keys.ContainsKey(name))
        {
            Debug.LogError("InputManager::GetButtonDown::No button named " + name);
            return false;
        }
        return Input.GetKeyDown(keys[name]);
    }

    // Return the whole keys dictionary
    public static Dictionary<string, KeyCode> getKeys()
    {
        return keys;
    }

    // Return the KeyCode of the name placed into the parameter
    public static bool GetButton(string name)
    {
        if (!keys.ContainsKey(name))
        {
            Debug.LogError("InputManager::GetButtonDown::No button named " + name);
            return false;
        }
        return Input.GetKey(keys[name]);
    }

    // Get the direction of the player's movement input
    public static float GetAxisRaw(string axis)
    {
        if (axis == "Horizontal")
        {
            return (GetButton("Right") ? 1.0f : 0.0f) - (GetButton("Left") ? 1.0f : 0.0f);
        }
        else if (axis == "Vertical")
        {
            return (GetButton("Up") ? 1.0f : 0.0f) - (GetButton("Down") ? 1.0f : 0.0f);
        }
        else
        {
            throw new System.Exception(string.Format("Axis not supported: {0}", axis));
        }
    }

    // Check to see if the button pressed is released.
    public static bool GetButtonUp(string name)
    {
        if (!keys.ContainsKey(name))
        {
            Debug.LogError("InputManager::GetButtonUp::No button named " + name);
            return false;
        }
        return Input.GetKeyUp(keys[name]);
    }

    // Return an array of the names of the buttons
    public static string[] GetButtonNames()
    {
        return keys.Keys.ToArray();
    }

    // Takes a button name and returns the value of its corresponding KeyCode in keys
    public static string GetKeyNameFor(string name)
    {
        if (!keys.ContainsKey(name))
        {
            Debug.LogError("InputManager::GetKeyName::No button named " + name);
            return "N/A";
        }
        return keys[name].ToString();
    }

    // Set the value buttonName in keys to the new KeyCode
    public static void SetButtonForKey(string buttonName, KeyCode keyCode)
    {
        keys[buttonName] = keyCode;
    }
}
