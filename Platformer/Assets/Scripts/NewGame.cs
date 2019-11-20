using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class NewGame : MonoBehaviour
{
    public GameObject settingsUI;

    public void NextScene(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void SettingsScreen() {
        Instantiate(settingsUI);
    }
}
