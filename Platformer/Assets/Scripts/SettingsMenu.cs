using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{

    public AudioMixer audioMixer;

    private void Start() {
        float f;
        audioMixer.GetFloat("volume", out f);
        GameObject.Find("VolumeSlider").GetComponent<UnityEngine.UI.Slider>().value = f;
    }
    // Set the volume
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
}
