using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private Stopwatch stopwatch;
    private bool levelCompleted;
    public GameObject levelCompletePrefab;
    public string nextLevelScene;

    private void FinishLevel() {
        stopwatch.Stop();
        TimeSpan ts = stopwatch.Elapsed;
        // stop time and reset death counter
        Time.timeScale = 0;
        LevelSkip.numDeaths = 0;
        // create level complete screen and give it the correct complete time text
        GameObject levelCompleteScreen = Instantiate(levelCompletePrefab, Vector3.zero, Quaternion.identity);
        levelCompleteScreen.GetComponent<LevelCompleteControls>().timeToComplete = ts;
        levelCompleteScreen.GetComponent<LevelCompleteControls>().nextLevelScene = nextLevelScene;
    }

    void Start()
    {
        levelCompleted = false;
        stopwatch = new Stopwatch();
        stopwatch.Start();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!levelCompleted) {
            FinishLevel();
            levelCompleted = true;
        }
    }
}
