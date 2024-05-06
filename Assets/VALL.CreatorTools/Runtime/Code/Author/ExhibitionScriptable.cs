using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "ExhibitionInfo", menuName = "Vall/Exhibition", order = 1)]
public class ExhibitionScriptable : ScriptableObject {

    public const string version = "1.0.0";
    public string ID;
    public string authorID;
    public string sceneFileName; 
    public Sprite img;

    public LocalizedString title;
    public LocalizedString info;
    
}
