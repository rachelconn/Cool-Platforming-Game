using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InputManager : MonoBehaviour
{
    private Dictionary<string, KeyCode> keys;

    // Keep the object between scene changes
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (FindObjectsOfType(GetType()).Length != 1)
        {
            Destroy(this.gameObject);
        }
    }

    // Use this for initialization
    void OnEnable()
    {
        keys = new Dictionary<string, KeyCode>();

        keys["Jump"] = KeyCode.Z;
        keys["Left"] = KeyCode.LeftArrow;
        keys["Right"] = KeyCode.RightArrow;
        keys["Dash"] = KeyCode.X;
    }

    // Check to see if the button entered is being pressed.
    public bool GetButtonDown(string name)
    {
        if (!keys.ContainsKey(name))
        {
            Debug.LogError("InputManager::GetButtonDown::No button named " + name);
            return false;
        }
        return Input.GetKeyDown(keys[name]);
    }
    
    // Check to see if the button pressed is released.
    public bool GetButtonUp(string name)
    {
        if (!keys.ContainsKey(name))
        {
            Debug.LogError("InputManager::GetButtonUp::No button named " + name);
            return false;
        }
        return Input.GetKeyUp(keys[name]);
    }

    // Return an array of the names of the buttons
    public string[] GetButtonNames()
    {
        return keys.Keys.ToArray();
    }

    // Takes a button name and returns the value of its corresponding KeyCode in keys
    public string GetKeyNameFor(string name)
    {
        if (!keys.ContainsKey(name))
        {
            Debug.LogError("InputManager::GetKeyName::No button named " + name);
            return "N/A";
        }
        return keys[name].ToString();
    }

    // Set the value buttonName in keys to the new KeyCode
    public void SetButtonForKey(string buttonName, KeyCode keyCode)
    {
        keys[buttonName] = keyCode;
    }
}
