using System.Collections;
using System.Collections.Generic;
using EthanZarov.SimpleTools.EZKeyboard;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "kbSettings", menuName = "EZTools/EZKeyboard/KeyboardSettings")]
public class EZKeyboardSettings : ScriptableObject
{
    
    [BoxGroup("Config")] public EZKeyboardLayout layout;
    [BoxGroup("Config")] public EZKeyboardButtonDisplay characterButtonDisplayTemplate;
    [BoxGroup("Config")] public EZKeyboardButtonDisplay backspaceButtonDisplayTemplate;
    
    
    
    [BoxGroup("Layout"), Range(0,2f)] public float keyHeight;
    [BoxGroup("Layout"), Range(0, .1f)] public float yPadding;
    [BoxGroup("Layout"), Range(0, .1f)] public float xPadding;
    [BoxGroup("Layout")] public Vector2 xMargins;
    [BoxGroup("Layout")] public Vector2 yMargins;
    [PropertySpace]
    
    [BoxGroup("Layout"), Range(0,.4f)] public float viewportSizeY;




}
