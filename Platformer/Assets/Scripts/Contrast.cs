using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class Contrast : MonoBehaviour
{
    public PostProcessProfile ppp;
    private ColorGrading color;
    public static float contrastValue;
    public Slider contrastSlider;

    // Start is called before the first frame update
    void Awake()
    {
        ppp.TryGetSettings(out color);
        contrastSlider.value = contrastValue;
    }

    // Set contrast to the value on the contrast slider
    void Update()
    {
        contrastValue = contrastSlider.value;
        color.contrast.value = contrastValue;
    }
}
