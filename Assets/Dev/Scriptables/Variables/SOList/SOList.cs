using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix;
using Sirenix.OdinInspector;
using SOD;

[CreateAssetMenu()]
public class SOList : SerializedScriptableObject
{
    [Header("Build & Editor")]
    [SerializeField]
    private List<ScriptableObject> core_value = new List<ScriptableObject>();
    public data_changes_enum data_changes = data_changes_enum.persistent;
    public created_type_enum created_type = created_type_enum.editor_created;

#if UNITY_EDITOR
    [Header("Editor-Only")]
    [Multiline]
    public string description = "";
    [SerializeField]
    private data_changes_enum editor_data_changes = data_changes_enum.nonpersistent;
    [SerializeField]
    private List<ScriptableObject> refresh_value = new List<ScriptableObject>();
#endif

    public void OnEnable()
    {
        if (data_changes == data_changes_enum.persistent)
        {
            // Try to load if exists on disk
            DataContainer.LoadFromDisk(this);
            // Save in any case
            DataContainer.SaveOnDisk(this);
        }

#if UNITY_EDITOR
        if (editor_data_changes == data_changes_enum.nonpersistent) DataContainer.CopySOList(refresh_value, core_value);
#endif
    }

    public void Add(ScriptableObject scriptable)
    {
        if (!core_value.Contains(scriptable))
        {
            core_value.Add(scriptable);
        }

        if (data_changes == data_changes_enum.persistent) DataContainer.SaveOnDisk(this);
    }

    public void Remove(ScriptableObject scriptable)
    {
        if (core_value.Contains(scriptable))
        {
            int index = core_value.IndexOf(scriptable);

            // Remove gameobject from the list
            core_value.RemoveAt(index);
        }

        if (data_changes == data_changes_enum.persistent) DataContainer.SaveOnDisk(this);
    }

    public void Clear()
    {
        core_value.Clear();

        if (data_changes == data_changes_enum.persistent) DataContainer.SaveOnDisk(this);
    }

    [HideInInspector]
    public List<ScriptableObject> v
    {
        get
        {
            return core_value;
        }
    }
}
