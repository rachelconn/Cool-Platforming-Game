using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadGame : MonoBehaviour
{
    string levelNum = "0";
    string tempSaveFile = "autoSave";
    public Dictionary<string, KeyCode> keys = InputManager.getKeys();

    //creates a new save game object
    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        save.levelNum = levelNum;
        //Debug.Log("tempSaveFile = " + tempSaveFile);
        //Debug.Log("Saving Save.saveFile = " + Save.saveFile);
        Save.saveFile = tempSaveFile;

        return save;

    }

    //saves game in the desired slot
    public void SaveGame(string tempLevel)
    {

        //Debug.Log("Level = " + tempLevel);

        //Debug.Log("Saving Save.saveFile before new save created = " + Save.saveFile);

        tempSaveFile = Save.saveFile;
        keys = Save.keys;
        Save save = CreateSaveGameObject();

        //sets all vars to desired values
        Save.keys = keys;
        Save.saveFile = tempSaveFile;
        save.levelNum = tempLevel;
        //Debug.Log("tempSaveFile = " + tempSaveFile);
        //Debug.Log("Saving Save.saveFile = " + Save.saveFile);

        //saves game to computer
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave" + Save.saveFile + ".save");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved");
    }

    //autosaves game
    public void AutosaveGame(string tempLevel)
    {

        //Debug.Log("Level = " + tempLevel);

        keys = Save.keys;

        Save save = CreateSaveGameObject();

        //sets all vars to desired values
        Save.keys = keys;
        Save.saveFile = "autosave";
        save.levelNum = tempLevel;

        //saves game to computer
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave" + Save.saveFile + ".save");
        bf.Serialize(file, save);
        file.Close();

        Save.saveFile = tempSaveFile;

        Debug.Log("Game Saved");
    }

    //Loads the desired saved game
    public void Load(string wantSaveFile)
    {
        //opens game if it exists
        if (File.Exists(Application.persistentDataPath + "/gamesave" + wantSaveFile + ".save"))
        {
            //opens the save file
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave" + wantSaveFile + ".save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            //starts at tutorial if saved there
            if (save.levelNum == "0")
            {
                SceneManager.LoadScene("Level0 (Tutorial)");
            }
            //goes to the saved level
            else
            {
                SceneManager.LoadScene(save.levelNum);
            }
            
            //sets all variables locally
            levelNum = save.levelNum;
            tempSaveFile = wantSaveFile;
            Save.saveFile = tempSaveFile;
            keys = Save.keys;

            //Debug.Log("tempSaveFile = " + tempSaveFile);
            //Debug.Log("Save.saveFile = " + Save.saveFile);
            Debug.Log("Game Loaded");

        }
        //save didn't exist yet
        else
        {
            //sets vars to what is needed to begin the save
            tempSaveFile = wantSaveFile;
            Save.saveFile = tempSaveFile;
            SceneManager.LoadScene("Level0 (Tutorial)");
            //Debug.Log("tempSaveFile = " + tempSaveFile);
            //Debug.Log("Save.saveFile = " + Save.saveFile);
            Debug.Log("No game saved");


        }


    }
}
