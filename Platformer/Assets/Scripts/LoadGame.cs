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

    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        save.levelNum = levelNum;
        //Debug.Log("tempSaveFile = " + tempSaveFile);
        //Debug.Log("Saving Save.saveFile = " + Save.saveFile);
        Save.saveFile = tempSaveFile;

        return save;

    }

    public void SaveGame(string tempLevel)
    {

        //Debug.Log("Level = " + tempLevel);

        //Debug.Log("Saving Save.saveFile before new save created = " + Save.saveFile);

        tempSaveFile = Save.saveFile;

        Save save = CreateSaveGameObject();

        Save.saveFile = tempSaveFile;
        save.levelNum = tempLevel;
        //Debug.Log("tempSaveFile = " + tempSaveFile);
        //Debug.Log("Saving Save.saveFile = " + Save.saveFile);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave" + Save.saveFile + ".save");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved");
    }

    public void AutosaveGame(string tempLevel)
    {

        //Debug.Log("Level = " + tempLevel);

        Save save = CreateSaveGameObject();

        Save.saveFile = "autosave";
        save.levelNum = tempLevel;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave" + Save.saveFile + ".save");
        bf.Serialize(file, save);
        file.Close();

        Save.saveFile = tempSaveFile;

        Debug.Log("Game Saved");
    }

    public void Load(string wantSaveFile)
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave" + wantSaveFile + ".save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave" + wantSaveFile + ".save", FileMode.Open);
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
            tempSaveFile = wantSaveFile;
            Save.saveFile = tempSaveFile;
            keys = save.keys;

            //Debug.Log("tempSaveFile = " + tempSaveFile);
            //Debug.Log("Save.saveFile = " + Save.saveFile);
            Debug.Log("Game Loaded");

        }
        else
        {
            tempSaveFile = wantSaveFile;
            Save.saveFile = tempSaveFile;
            SceneManager.LoadScene("Level0 (Tutorial)");
            //Debug.Log("tempSaveFile = " + tempSaveFile);
            //Debug.Log("Save.saveFile = " + Save.saveFile);
            Debug.Log("No game saved");


        }


    }
}
