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
    string saveFile = "autoSave";
    public Dictionary<string, KeyCode> keys = InputManager.getKeys();

    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        save.levelNum = levelNum;
        save.saveFile = saveFile;

        return save;

    }

    public void SaveGame(string tempLevel)
    {

        Debug.Log("Level = " + tempLevel);

        Save save = CreateSaveGameObject();

        save.levelNum = tempLevel;
        //save.saveFile = Save.saveFile;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave" + save.saveFile + ".save");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved");
    }

    public void AutosaveGame(string tempLevel)
    {

        Debug.Log("Level = " + tempLevel);

        Save save = CreateSaveGameObject();

        save.saveFile = "autosave";
        save.levelNum = tempLevel;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave" + save.saveFile + ".save");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved");
    }

    public void Load(string tempSaveFile)
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave" + tempSaveFile + ".save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave" + tempSaveFile + ".save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            if (save.levelNum == "0")
            {
                SceneManager.LoadScene("Level0 (Tutorial)");
            }
            else
            {
                SceneManager.LoadScene(save.levelNum);
            }
            
            levelNum = save.levelNum;
            saveFile = save.saveFile;
            keys = save.keys;

            Debug.Log("Game Loaded");

        }
        else
        {
            SceneManager.LoadScene("Level0 (Tutorial)");
            Debug.Log("No game saved");


        }


    }
}
