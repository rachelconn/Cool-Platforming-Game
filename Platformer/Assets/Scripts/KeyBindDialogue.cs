using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindDialogue : MonoBehaviour
{
    InputManager inputManager;
    public GameObject keyItemPrefab;
    public GameObject keyList;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameObject.FindObjectOfType<InputManager>();
        string[] names = inputManager.GetButtonNames();

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

            /*
            * To Do:
            *      This is only being set to z, fix it
            */
            Text keyName = go.transform.Find("Button/Key").GetComponent<Text>();
            keyName.text = inputManager.GetKeyNameFor(bn);

            // Adds an action listener to the button
            Button bindButton = go.transform.Find("Button").GetComponent<Button>();

            /*
             * To Do:
             *      The action listener doesn't work, find out why
            */
            //bindButton.onClick.AddListener( () => { RebindFor(bn); } );
        }
        

    }

    void RebindFor(string name)
    {
        Debug.LogError("test");
    }
}
