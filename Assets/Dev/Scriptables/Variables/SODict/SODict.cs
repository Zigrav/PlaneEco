using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using SOD;

[CreateAssetMenu()]
public class SODict : SerializedScriptableObject
{
    [Header("Build & Editor")]
    [SerializeField]
    private Dictionary<string, ScriptableObject> core_value = new Dictionary<string, ScriptableObject>();
    public data_changes_enum data_changes = data_changes_enum.persistent;
    [HideInInspector]
    public created_type_enum created_type = created_type_enum.editor_created;

#if UNITY_EDITOR
    [Header("Editor-Only")]
    [Multiline]
    public string description = "";
    [SerializeField]
    private data_changes_enum editor_data_changes = data_changes_enum.nonpersistent;
    [SerializeField]
    private Dictionary<string, ScriptableObject> refresh_value = new Dictionary<string, ScriptableObject>();
#endif

    public void OnEnable()
    {
        if (data_changes == data_changes_enum.persistent)
        {
            // Try to load if exists on disk
            DataContainer.LoadFromDisk(this);//
            // Save in any case
            DataContainer.SaveOnDisk(this);
        }

#if UNITY_EDITOR
        if (editor_data_changes == data_changes_enum.nonpersistent) DataContainer.CopySODict(refresh_value, core_value);
#endif
    }

    public void Add(string key, ScriptableObject scriptable)
    {
        core_value.Add(key, scriptable);

        if (data_changes == data_changes_enum.persistent) DataContainer.SaveOnDisk(this);
    }

    public void Remove(string key)
    {
        core_value.Remove(key);

        if (data_changes == data_changes_enum.persistent) DataContainer.SaveOnDisk(this);
    }

    public void Clear()
    {
        core_value.Clear();

        if (data_changes == data_changes_enum.persistent) DataContainer.SaveOnDisk(this);
    }

    [HideInInspector]
    public Dictionary<string, ScriptableObject> v
    {
        get
        {
            return core_value;
        }
    }

}