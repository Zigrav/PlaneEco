using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Anchor))]
public class AnchorEditor : Editor
{
    private string[] options = new string[] { "Average", "Single" };
    public int index = 0;

    public override void OnInspectorGUI()
    {
        var myScript = target as Anchor;

        SerializedObject so = new SerializedObject(target);
        
        myScript.is_average = GUILayout.Toggle(myScript.is_average, " Use Average");
        // index = EditorGUILayout.Popup(index, options);

        if (myScript.is_average)
        {
            SerializedProperty average_list = so.FindProperty("average_list");
            EditorGUILayout.PropertyField(average_list, true); // True means show children
        }
        else
        {
            SerializedProperty point = so.FindProperty("point");
            EditorGUILayout.PropertyField(point, true); // True means show children
        }

        SerializedProperty offset = so.FindProperty("offset");
        EditorGUILayout.PropertyField(offset, true); // True means show children

        SerializedProperty draw_anchor = so.FindProperty("draw_anchor");
        EditorGUILayout.PropertyField(draw_anchor, true); // True means show children

        so.ApplyModifiedProperties(); // Remember to apply modified properties

    }
}