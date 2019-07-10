using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathAnimator))]
public class PathAnimatorEditor : Editor
{
    private string[] options = new string[] { "Speed Control", "Period Control" };
    private bool is_speed_control = true;

    public override void OnInspectorGUI()
    {
        var myScript = target as PathAnimator;
        SerializedObject so = new SerializedObject(target);

        // Script Field
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((PathAnimator)target), typeof(PathAnimator), false);
        GUI.enabled = true;

        SerializedProperty component_with_vertex_path = so.FindProperty("component_with_vertex_path");
        EditorGUILayout.PropertyField(component_with_vertex_path, true); // True means show children

        SerializedProperty path_generator = so.FindProperty("path_generator");
        EditorGUILayout.PropertyField(path_generator, true); // True means show children

        if (GUILayout.Button("Switch Control Mode"))
        {
            is_speed_control = !is_speed_control;
            Debug.Log(is_speed_control);
        }

        SerializedProperty speed_curve = so.FindProperty("speed_curve");
        EditorGUILayout.PropertyField(speed_curve, true); // True means show children

        SerializedProperty speed_coeff = so.FindProperty("speed_coeff");
        SerializedProperty period = so.FindProperty("period");

        if (is_speed_control)
        {
            EditorGUILayout.PropertyField(speed_coeff, true); // True means show children
            EditorGUILayout.LabelField("Period: ", period.floatValue.ToString());

            if (speed_coeff.floatValue <= 0.0f)
            {
                speed_coeff.floatValue = 0.01f;
            }

            period.floatValue = myScript.CalcPeriod(0.01f, speed_coeff.floatValue);
        }
        else
        {
            EditorGUILayout.LabelField("Speed Coefficient: ", speed_coeff.floatValue.ToString());
            EditorGUILayout.PropertyField(period, true); // True means show children

            if (period.floatValue <= 0.0f)
            {
                period.floatValue = 0.01f;
            }

            speed_coeff.floatValue = myScript.CalcSpeedCoeff(0.01f, period.floatValue);
        }

        SerializedProperty MoveInWorldCoordinates = so.FindProperty("MoveInWorldCoordinates");
        EditorGUILayout.PropertyField(MoveInWorldCoordinates, true); // True means show children

        SerializedProperty min_speed = so.FindProperty("min_speed");
        EditorGUILayout.PropertyField(min_speed, true); // True means show children

        SerializedProperty stop_fraction = so.FindProperty("stop_fraction");
        EditorGUILayout.PropertyField(stop_fraction, true); // True means show children

        SerializedProperty end_of_path_instruction = so.FindProperty("end_of_path_instruction");
        EditorGUILayout.PropertyField(end_of_path_instruction, true); // True means show children

        SerializedProperty OnStart = so.FindProperty("OnStart");
        EditorGUILayout.PropertyField(OnStart, true); // True means show children

        SerializedProperty OnFixedUpdate = so.FindProperty("OnFixedUpdate");
        EditorGUILayout.PropertyField(OnFixedUpdate, true); // True means show children

        SerializedProperty OnFixedUpdateAnim = so.FindProperty("OnFixedUpdateAnim");
        EditorGUILayout.PropertyField(OnFixedUpdateAnim, true); // True means show children

        so.ApplyModifiedProperties(); // Remember to apply modified properties
    }
}