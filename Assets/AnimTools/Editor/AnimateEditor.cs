using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using AnimTools;

[CustomEditor(typeof(Animate), true)]
public class AnimateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Animate anim = (Animate)target;
        if (GUILayout.Button("Play"))
        {
            anim.Play();
        }
        Color color = GUI.color;
        GUI.color = Color.red;
        if (GUILayout.Button("Reset"))
        {
            anim.BeginAnimation();
        }
        GUI.color = color;
    }

    Tool lastTool = Tool.None;

    void OnEnable()
    {
        lastTool = Tools.current;
        Tools.current = Tool.None;
    }

    void OnDisable()
    {
        Tools.current = lastTool;
    }
}
