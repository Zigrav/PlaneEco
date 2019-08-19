//using UnityEngine;
//using UnityEditor;
//using System;

//[CustomEditor(typeof(ProbeManager))]
//public class ProbeManagerEditor : Editor
//{
//    SerializedProperty probes_prop;

//    private void OnEnable()
//    {
//        probes_prop = serializedObject.FindProperty("probes");
//    }

//    // This is called when the object loses focus or the Inspector is closed
//    private void OnDisable()
//    {

//    }

//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        //ProbeManager probe_manager = (ProbeManager)target;
        
//        //// Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
//        //serializedObject.Update();
        
//        //// Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
//        //serializedObject.ApplyModifiedProperties();
//    }

//    protected virtual void OnSceneGUI()
//    {
//        ProbeManager probe_manager = (ProbeManager)target;
        
//        for (int i = 0; i < probe_manager.probes.Length; i++)
//        {
//            EditorGUI.BeginChangeCheck();

//            Vector3 position = Handles.PositionHandle(probe_manager.probes[i].pos, Quaternion.identity);
            
//            SerializedProperty probe = probes_prop.GetArrayElementAtIndex(i);
//            SerializedProperty probe_pos = probe.FindPropertyRelative("pos");

//            if (EditorGUI.EndChangeCheck() == true)
//            {
//                probe_pos.vector3Value = position;

//                serializedObject.ApplyModifiedProperties();
//            }
//        }
//    }

//}