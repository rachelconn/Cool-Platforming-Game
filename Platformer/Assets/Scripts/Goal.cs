using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private Stopwatch stopwatch;
    private bool levelCompleted;

    private void FinishLevel() {
        stopwatch.Stop();
        TimeSpan ts = stopwatch.Elapsed;
        UnityEngine.Debug.Log(String.Format("{0}:{1:00}.{2}", Math.Floor(ts.TotalMinutes), ts.Seconds, ts.Milliseconds));
        Time.timeScale = 0;
    }

    void Start()
    {
        levelCompleted = false;
        stopwatch = new Stopwatch();
        stopwatch.Start();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other) {
        UnityEngine.Debug.Log("a");
        if (!levelCompleted) {
            levelCompleted = true;
            FinishLevel();
        }
    }
}
