using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameEventListener))]
public class GameEventListenerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var myScript = target as GameEventListener;

        SerializedObject so = new SerializedObject(target);

        //EditorGUILayout.ObjectField(script, typeof(GameEventListener), false) as MonoScript;

        // Script Field
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((GameEventListener)target), typeof(GameEventListener), false);
        GUI.enabled = true;

        // Event To Be Attached To
        SerializedProperty Event = so.FindProperty("Event");
        EditorGUILayout.PropertyField(Event, GUIContent.none, true); // True means show children

        EditorGUILayout.BeginHorizontal("box");

        // Order Number Of This Listener
        SerializedProperty Order = so.FindProperty("Order");
        EditorGUILayout.PropertyField(Order, GUIContent.none, true); // True means show children

        // UseConditions
        SerializedProperty UseConditions = so.FindProperty("UseConditions");
        EditorGUILayout.PropertyField(UseConditions, new GUIContent("Conditions"), true); // True means show children

        EditorGUILayout.EndHorizontal();

        // Display Only If UseConditions Is True
        if (UseConditions.boolValue)
        {
            // Find variable_variant property And Get Its Chosen Value
            SerializedProperty variable_variant = so.FindProperty("variable_variant");

            switch (variable_variant.enumValueIndex)
            {
                // Corresponds to StringVariable
                case 0:
                {
                    EditorGUILayout.BeginHorizontal("box");

                    // Display variable_variant property
                    EditorGUILayout.PropertyField(variable_variant, GUIContent.none, true); // True means show children

                    // Get Appropriate Set Of Conditions
                    SerializedProperty conditions_1_variant = so.FindProperty("conditions_1_variant");
                    EditorGUILayout.PropertyField(conditions_1_variant, GUIContent.none, true); // True means show children

                    // Get Appropriate Set Of Second Variable
                    SerializedProperty string_variant = so.FindProperty("string_variant");
                    EditorGUILayout.PropertyField(string_variant, GUIContent.none, true); // True means show children

                    EditorGUILayout.EndHorizontal();


                    EditorGUILayout.BeginHorizontal("box");

                    // Disaply First Variable Field
                    SerializedProperty first_string_var = so.FindProperty("first_string_var");
                    EditorGUILayout.PropertyField(first_string_var, GUIContent.none, true); // True means show children

                    // Disaply Second Variable Field Based On Chosen Variant
                    if (string_variant.enumValueIndex == 0)
                    {
                        SerializedProperty second_string_var = so.FindProperty("second_string_var");
                        EditorGUILayout.PropertyField(second_string_var, GUIContent.none, true); // True means show children
                    }
                    else
                    {
                        SerializedProperty second_string = so.FindProperty("second_string");
                        EditorGUILayout.PropertyField(second_string, GUIContent.none, true); // True means show children
                    }

                    EditorGUILayout.EndHorizontal();
                    break;
                }
                // Corresponds to BoolVariable
                case 1:
                {
                    EditorGUILayout.BeginHorizontal("box");

                    // Display variable_variant property
                    EditorGUILayout.PropertyField(variable_variant, GUIContent.none, true); // True means show children

                    // Get Appropriate Set Of Conditions
                    SerializedProperty conditions_1_variant = so.FindProperty("conditions_1_variant");
                    EditorGUILayout.PropertyField(conditions_1_variant, GUIContent.none, true); // True means show children

                    // Get Appropriate Set Of Second Variable
                    SerializedProperty boolean_variant = so.FindProperty("boolean_variant");
                    EditorGUILayout.PropertyField(boolean_variant, GUIContent.none, true); // True means show children

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal("box");

                    // Disaply First Variable Field
                    SerializedProperty first_bool_var = so.FindProperty("first_bool_var");
                    EditorGUILayout.PropertyField(first_bool_var, GUIContent.none, true); // True means show children

                    if (boolean_variant.enumValueIndex == 0)
                    {
                        SerializedProperty second_bool_var = so.FindProperty("second_bool_var");
                        EditorGUILayout.PropertyField(second_bool_var, GUIContent.none, true); // True means show children
                    }
                    else
                    {
                        SerializedProperty second_bool = so.FindProperty("second_bool");
                        EditorGUILayout.PropertyField(second_bool, GUIContent.none, true); // True means show children
                    }
                    EditorGUILayout.EndHorizontal();

                    break;
                }
                // Corresponds to IntVariable
                case 2:
                {
                    EditorGUILayout.BeginHorizontal("box");

                    EditorGUILayout.PropertyField(variable_variant, GUIContent.none, true); // True means show children

                    SerializedProperty conditions_0_variant = so.FindProperty("conditions_0_variant");
                    EditorGUILayout.PropertyField(conditions_0_variant, GUIContent.none, true); // True means show children

                    SerializedProperty int_variant = so.FindProperty("int_variant");
                    EditorGUILayout.PropertyField(int_variant, GUIContent.none, true); // True means show children

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal("box");

                    SerializedProperty first_int_var = so.FindProperty("first_int_var");
                    EditorGUILayout.PropertyField(first_int_var, GUIContent.none, true); // True means show children

                    if (int_variant.enumValueIndex == 0)
                    {
                        SerializedProperty second_int_var = so.FindProperty("second_int_var");
                        EditorGUILayout.PropertyField(second_int_var, GUIContent.none, true); // True means show children
                    }
                    else
                    {
                        SerializedProperty second_int = so.FindProperty("second_int");
                        EditorGUILayout.PropertyField(second_int, GUIContent.none, true); // True means show children
                    }

                    EditorGUILayout.EndHorizontal();
                    break;
                }
                // Corresponds to FloatVariable
                case 3:
                {
                    EditorGUILayout.BeginHorizontal("box");

                    EditorGUILayout.PropertyField(variable_variant, GUIContent.none, true); // True means show children

                    SerializedProperty conditions_0_variant = so.FindProperty("conditions_0_variant");
                    EditorGUILayout.PropertyField(conditions_0_variant, GUIContent.none, true); // True means show children

                    SerializedProperty float_variant = so.FindProperty("float_variant");
                    EditorGUILayout.PropertyField(float_variant, GUIContent.none, true); // True means show children

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal("box");

                    SerializedProperty first_float_var = so.FindProperty("first_float_var");
                    EditorGUILayout.PropertyField(first_float_var, GUIContent.none, true); // True means show children

                    if (float_variant.enumValueIndex == 0)
                    {
                        SerializedProperty second_float_var = so.FindProperty("second_float_var");
                        EditorGUILayout.PropertyField(second_float_var, GUIContent.none, true); // True means show children
                    }
                    else
                    {
                        SerializedProperty second_float = so.FindProperty("second_float");
                        EditorGUILayout.PropertyField(second_float, GUIContent.none, true); // True means show children
                    }

                    EditorGUILayout.EndHorizontal();
                    break;
                    }
            }
        }

        SerializedProperty Response = so.FindProperty("Response");
        EditorGUILayout.PropertyField(Response, GUIContent.none, true); // True means show children

        SerializedProperty delay_frames = so.FindProperty("delay_frames");
        EditorGUILayout.PropertyField(delay_frames, new GUIContent("Delay"), true); // True means show children

        so.ApplyModifiedProperties(); // Remember to apply modified properties
    }
}