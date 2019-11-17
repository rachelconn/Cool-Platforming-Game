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

    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        save.levelNum = levelNum;
        save.saveFile = saveFile;

        return save;

    }

    public void SaveGame()
    {
        Save save = CreateSaveGameObject();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave" + save.saveFile + ".save");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved");
    }

    public void AutosaveGame()
    {
        Save save = CreateSaveGameObject();

        save.saveFile = "autosave";

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
                SceneManager.LoadScene("Tutorial");
            }
            else
            {
                SceneManager.LoadScene(save.levelNum);
            }
            
            levelNum = save.levelNum;
            saveFile = save.saveFile;

            Debug.Log("Game Loaded");

        }
        else
        {
            SceneManager.LoadScene("Tutorial");
            Debug.Log("No game saved");

            levelNum = "0";
            saveFile = tempSaveFile;
        }


    }
}
