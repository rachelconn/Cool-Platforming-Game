using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class KeyBindDialogue : MonoBehaviour
{
    InputManager inputManager;
    public GameObject keyItemPrefab;
    public GameObject keyList;
    string buttonToRebind = null;
    Dictionary<string, Text> buttonToLabel;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameObject.FindObjectOfType<InputManager>();
        string[] names = inputManager.GetButtonNames();
        buttonToLabel = new Dictionary<string, Text>();

        // Loop through the dictionary in inputManager to 
        // create a key item for each button in inputManager
        foreach (string bn in names)
        {

            GameObject go = (GameObject) Instantiate(keyItemPrefab);
            Instantiate(keyItemPrefab);
            go.transform.SetParent(keyList.transform);
            go.transform.localScale = Vector3.one;

            // Set the button name
            Text Name = go.transform.Find("Name").GetComponent<Text>();
            Name.text = bn;

            // Set the key value
            Text keyName = go.transform.Find("Button/Key").GetComponent<Text>();
            keyName.text = inputManager.GetKeyNameFor(bn);
            buttonToLabel[bn] = keyName;

            // Adds an action listener to the button
            Button bindButton = go.transform.Find("Button").GetComponent<Button>();
            bindButton.onClick.AddListener( () => { RebindFor(bn); } );
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
                        inputManager.SetButtonForKey(buttonToRebind, kc);
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
}
