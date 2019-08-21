using UnityEngine;
using SOD;

[CreateAssetMenu]
public class StringVariable : ScriptableObject
{
    [Header("Build & Editor")]
    [SerializeField]
    private string core_value;
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
    private string refresh_value = "";
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
        if (editor_data_changes == data_changes_enum.nonpersistent) core_value = refresh_value;
#endif
    }

    public string v
    {
        get
        {
            return core_value;

        }
        set
        {
            core_value = value;

            if (data_changes == data_changes_enum.persistent) DataContainer.SaveOnDisk(this);
        }
    }
}