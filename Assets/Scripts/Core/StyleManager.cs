using System;
using UnityEngine;

public class StyleManager : MonoBehaviour
{
    public ButtonStyle[] ButtonStyles;
}
[Serializable]
public struct ButtonStyle
{
    public Texture2D icon;
    public string ButtonTex;

    [HideInInspector]
    public GUIStyle NodeStyle;

    public GameObject PrefabObject;
}