using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Mig.Core;


[CustomEditor(typeof(MigElementWrapper))]
public class MigElementWrapperEditor : Editor
{
    private MigElementWrapper wrapper;

    private void OnEnable()
    {
        wrapper = target as MigElementWrapper;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Label("Show Element");
        GUILayout.Space(10);
        foreach (var element in wrapper.Elements)
        {
            GUILayout.Label(element.GameObjectPath);
            GUILayout.Label("> " + element.GetType().ToString());
            GUILayout.Label("> " + new GUIContent("OperateCount: " + element.OperateCount));
            GUILayout.Label("> " + new GUIContent("StepGUID   : " + element.StepGUID));
        }
    }
}
