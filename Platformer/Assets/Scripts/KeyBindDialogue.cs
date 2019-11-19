using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class KeyBindDialogue : MonoBehaviour
{
    public GameObject keyItemPrefab;
    string buttonToRebind = null;
    Dictionary<string, Text> buttonToLabel;

    // Start is called before the first frame update
    void Start()
    {
        string[] names = InputManager.GetButtonNames();
        buttonToLabel = new Dictionary<string, Text>();

        // Loop through the dictionary in inputManager to 
        // create a key item for each button in inputManager
        foreach (string bn in names)
        {

            GameObject go = (GameObject) Instantiate(keyItemPrefab, gameObject.transform);
            go.transform.localScale = Vector3.one;

            // Set the button name
            Text Name = go.transform.Find("Name").GetComponent<Text>();
            Name.text = bn;

            // Set the key value
            Text keyName = go.transform.Find("Button/Key").GetComponent<Text>();
            keyName.text = ProcessName(InputManager.GetKeyNameFor(bn));
            buttonToLabel[bn] = keyName;

            // Adds an action listener to the button
            UnityEngine.UI.Button bindButton = go.transform.Find("Button").GetComponent<UnityEngine.UI.Button>();
            bindButton.onClick.AddListener(() => { RebindFor(bn); });
        }


    }

    void Update()
    {
        if (buttonToRebind != null)
        {
            // Find which key is pressed
            if (Input.anyKeyDown)
            {
                Array kcs = Enum.GetValues(typeof(KeyCode));

                // Loop through the KeyCodes to see which button is pressed
                foreach (KeyCode kc in kcs)
                {
                    // If found rebind the key, then break from the loop
                    if (Input.GetKeyDown(kc))
                    {
                        InputManager.SetButtonForKey(buttonToRebind, kc);
                        buttonToLabel[buttonToRebind].text = kc.ToString();
                        buttonToRebind = null;
                        break;
                    }
                }
            }
        }
    }

    void RebindFor(string name)
    {
        buttonToRebind = name;
    }

    public static string ProcessName(string name)
    {
        return name.Replace("Arrow", " Arrow");
    }

    /// <summary>
    /// call a static function in player to restart time system
    /// </summary>
    public void Unpause()
    {
        Player.Unpause();
    }
}
