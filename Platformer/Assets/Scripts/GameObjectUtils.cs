using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameObjectUtils : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void SkipLevel()
    {
        // unfreeze time
        Time.timeScale = 1;
        string nextLevelScene = GameObject.Find("Goal").GetComponent<Goal>().nextLevelScene;
        SceneManager.LoadScene(nextLevelScene, LoadSceneMode.Single);
        LoadGame.setLevel(nextLevelScene);
    }


    private Slider volumeSlider = null;
    public AudioMixer mixer;

    /// <summary>
    /// this should only be called by a slider on the same GameObject as this script
    /// </summary>
    public void VolumeChanged()
    {
        if (volumeSlider == null)
        {
            volumeSlider = GetComponent<Slider>();
            if (volumeSlider == null)
            {
                Debug.LogError(string.Format("GameObjectUtils on Object {0} had VolumeChanged called but has no volume slider!", gameObject.name));
            }
        }
        if (mixer == null)
        {
            Debug.LogError(string.Format("GameObjectUtils on Object {0} has no AudioMixer set!", gameObject.name));
        }
        mixer.SetFloat("volume", volumeSlider.value);
        // Debug.Log(string.Format("volume set to {0}!", Player.volumeLevel));
    }
}
