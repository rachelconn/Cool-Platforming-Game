﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Linq;

[System.Serializable]
public class Save
{

    public string levelNum = "0";
    public static string saveFile = "autosave";

    public static Dictionary<string, KeyCode> keys = InputManager.getKeys();

}
