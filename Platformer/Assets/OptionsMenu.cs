using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{

    public AudioMixer audioMixer;
    public static GameManager GM;
    public Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    void Start()
    {
        keys["Jump"] = KeyCode.Space;
        keys["Left"] = KeyCode.LeftArrow;
        keys["Right"] = KeyCode.RightArrow;
    }

    public bool GetButtonDown(string name)
    {
        if (keys.ContainsKey(name) == false)
        {
            Debug.LogError("No button named " + name);
            return false;
        }
        return Input.GetKeyDown(keys[name]);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
}
