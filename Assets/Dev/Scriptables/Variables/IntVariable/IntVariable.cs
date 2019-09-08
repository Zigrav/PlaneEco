using UnityEngine;
using SOD;

[CreateAssetMenu]
public class IntVariable : ScriptableObject
{
    [Header("Build & Editor")]
    [SerializeField]
    private int core_value;
    [SerializeField]
    private int refresh_value = 0;
    public data_changes_enum data_changes = data_changes_enum.persistent;
    [HideInInspector]
    public created_type_enum created_type = created_type_enum.editor_created;

#if UNITY_EDITOR
    [Header("Editor-Only")]
    [Multiline]
    public string description = "";
    public data_changes_enum editor_data_changes = data_changes_enum.nonpersistent;
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

    public int v
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

    public void Refresh()
    {
        if (data_changes == data_changes_enum.nonpersistent) core_value = refresh_value;
    }

}