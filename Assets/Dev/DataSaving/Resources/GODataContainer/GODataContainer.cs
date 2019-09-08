using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using UnityEditor;
using System;

[CreateAssetMenu]
public class GODataContainer : SerializedScriptableObject
{
    private static List<ScriptableObject> go_vars_root_data_value;

    public static List<ScriptableObject> go_vars_root_data
    {
        get
        {
            // Assign if needed
            if (go_vars_root_data_value == null)
            {
                go_vars_root_data_value = Resources.Load<GODataContainer>(godata_container_path).inspector_go_vars_root_data;
            }

            return go_vars_root_data_value;
        }
        set
        {
            // If it is already assigned - return
            if (go_vars_root_data_value != null) return;

            go_vars_root_data_value = value;
        }
    }

    [SerializeField]
    private List<ScriptableObject> inspector_go_vars_root_data = null;

    [ShowInInspector, ReadOnly]
    private static readonly string godata_container_path = "GODataContainer/GODataContainer";

    [SerializeField]
    private string scriptables_path = "Assets/Core/Modulars";

#if UNITY_EDITOR

    [ButtonGroup("Button Group 1")]
    [Button]
    private void Validate()
    {
        if (inspector_go_vars_root_data == null)
        {
            Debug.LogError("root_data is null!");
            return;
        }

        List<ScriptableObject> scriptables = GetAllScriptables(scriptables_path);

        for (int i = 0; i < scriptables.Count; i++)
        {
            ScriptableObject scriptable = scriptables[i];
            if (!inspector_go_vars_root_data.Contains(scriptable))
            {
                string path = AssetDatabase.GetAssetPath(scriptable);

                Debug.LogError(path + " scriptable is not included in dictionary!");
            }
        }

        for (int i = 0; i < inspector_go_vars_root_data.Count; i++)
        {
            ScriptableObject scriptable = inspector_go_vars_root_data[i];

            string path = AssetDatabase.GetAssetPath(scriptable);

            if (scriptable == null)
            {
                Debug.LogError("Element #" + i + " does not lead to an asset!");
                continue;
            }

            int count = GetScrtipablesCount(inspector_go_vars_root_data, scriptable);
            if (count > 1)
            {
                Debug.LogError(path + " scriptable # " + i + " is included in dictionary " + count + " times!");
                continue;
            }

            if (!IsFileBelowDirectory(path, scriptables_path, "/"))
            {
                Debug.LogError(path + " scriptable # " + i + " is not in the " + scriptables_path + " folder!");
                continue;
            }
        }
    }


    public static int GetScrtipablesCount(List<ScriptableObject> list, ScriptableObject scriptable)
    {
        int count = 0;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == scriptable)
            {
                count++;
            }
        }

        return count;
    }

    public static bool IsFileBelowDirectory(string file_path, string dir_path, string separator)
    {
        var modifired_dit_path = string.Format("{0}{1}", dir_path, dir_path.EndsWith(separator) ? "" : separator);
        return file_path.StartsWith(modifired_dit_path, StringComparison.OrdinalIgnoreCase);
    }

    [ButtonGroup("Button Group 1")]
    [Button]
    private void FillScriptables()
    {
        List<ScriptableObject> scriptables = GetAllScriptables(scriptables_path);

        for (int i = 0; i < scriptables.Count; i++)
        {
            if (!go_vars_root_data.Contains(scriptables[i]))
            {
                go_vars_root_data.Add(scriptables[i]);
            }
        }
    }

    private List<ScriptableObject> GetAllScriptables(string scriptables_path)
    {
        List<string> guids = new List<string>(AssetDatabase.FindAssets("t: ScriptableObject", new string[] { scriptables_path }));

        List<ScriptableObject> scriptables = new List<ScriptableObject>();
        for (int i = 0; i < guids.Count; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);

            ScriptableObject scriptable = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

            if (scriptable is GOVariable go_var)
            {
                scriptables.Add(scriptable);
            }
            else if (scriptable is GOList go_list)
            {
                scriptables.Add(scriptable);
            }
        }

        return scriptables;
    }

#endif

}
