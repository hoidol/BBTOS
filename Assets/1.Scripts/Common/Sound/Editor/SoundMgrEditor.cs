using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(SoundMgr))]

public class SoundMgrEditor : Editor
{
    SoundMgr script;
    public override VisualElement CreateInspectorGUI()
    {
        script = (SoundMgr)target;
        return base.CreateInspectorGUI();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Load Data", GUILayout.Height(50)))
        {
            script.Edit();
            EditorUtility.SetDirty(script);
        }
    }
}
