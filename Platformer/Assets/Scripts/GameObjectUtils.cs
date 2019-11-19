using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    }
}
