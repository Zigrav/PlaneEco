using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using System;
using System.IO;
using System.Linq;
using SOD;

namespace SOD
{
    public enum created_type_enum { code_created, editor_created }
    public enum data_changes_enum { persistent, nonpersistent }

    public struct MetaData
    {
        public MetaData(string guid, string type_string, data_changes_enum data_changes)
        {
            this.guid = guid;
            this.type_string = type_string;
            this.data_changes = data_changes;
        }

        public string guid;
        public string type_string;
        public data_changes_enum data_changes;
    }

    public struct ArchiveData
    {
        // path variable is here, to keep record the variable name / path, unity_guid does not tell you a lot :)
        // it's not used anywhere in code
        
        public ArchiveData(string path, string unity_guid)
        {
            this.path = path;
            this.unity_guid = unity_guid;
        }

        public string path;
        public string unity_guid;
    }
}

[CreateAssetMenu]
public class DataContainer : SerializedScriptableObject
{
    private static Dictionary<string, ScriptableObject> root_data_value;

    public static Dictionary<string, ScriptableObject> root_data
    {
        get
        {
            // Assign if needed
            if (root_data_value == null)
            {
                root_data_value = Resources.Load<DataContainer>(data_container_path).inspector_root_data;
            }

            return root_data_value;
        }
        set
        {
            // If it is already assigned - return
            if (root_data_value != null) return;

            root_data_value = value;
        }
    }

    [ShowInInspector, ReadOnly]
    private static readonly string data_container_path = "DataContainer/DataContainer";

    private static string main_data_path_value = "main.data";
    private static string main_data_path
    {
        get
        {
            if(!ES3.FileExists(main_data_path_value))
            {
                ES3.Save<string>("testkey", "just to create a file", main_data_path_value);
            }

            return main_data_path_value;
        }
    }

    [SerializeField]
    private Dictionary<string, ScriptableObject> inspector_root_data;

    [InlineButton("NewRandomGUID", label: "New")]
    [SerializeField, LabelWidth(83)]
    private string RandomGUID;

    private void NewRandomGUID()
    {
        RandomGUID = NewGUID();
    }

    public static string NewGUID()
    {
        return Guid.NewGuid().ToString("N");
    }

    [OnValueChanged("SetGuidsPath")]
    [ShowInInspector, LabelWidth(80)]
    [FolderPath(ParentFolder = "Assets", RequireExistingPath = true)]
    private string archive_path = "";

    [OnValueChanged("SetGuidsPath")]
    [ShowInInspector, LabelWidth(80)]
    private string version = "";

    [SerializeField, ReadOnly, LabelWidth(80)]
    private string guids_path;

    private void SetGuidsPath()
    {
        guids_path = Application.dataPath + "/" + archive_path + "/" + version + ".guids";
    }

    [SerializeField]
    private string scriptables_path = "Assets/Dev/Scriptables";

#if UNITY_EDITOR

    [ButtonGroup("Button Group 0")]
    [Button]
    private void SaveRootData()
    {
        if (guids_path == "") Debug.LogError("Defined guids_path!");

        Dictionary<string, ArchiveData> root_data_info = new Dictionary<string, ArchiveData>();

        foreach (KeyValuePair<string, ScriptableObject> entry in inspector_root_data)
        {
            string path = AssetDatabase.GetAssetPath(entry.Value);
            string unity_guid = AssetDatabase.AssetPathToGUID(path);

            ArchiveData archive_data = new ArchiveData(path, unity_guid);

            root_data_info.Add(entry.Key, archive_data);
        }

        ES3.Save<Dictionary<string, ArchiveData>>("guids", root_data_info, guids_path);
        AssetDatabase.Refresh();
    }

    [ButtonGroup("Button Group 0")]
    [Button]
    private void LoadRootData()
    {
        if (guids_path == "") Debug.LogError("Defined guids_path!");

        inspector_root_data = new Dictionary<string, ScriptableObject>();
        Dictionary<string, ArchiveData> root_data_info = ES3.Load<Dictionary<string, ArchiveData>>("guids", guids_path);

        foreach (KeyValuePair<string, ArchiveData> entry in root_data_info)
        {
            string guid = entry.Key;
            ArchiveData archive_data = entry.Value;
            string unity_guid = archive_data.unity_guid;
            
            string path = AssetDatabase.GUIDToAssetPath(unity_guid);
            ScriptableObject scriptable = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

            inspector_root_data.Add(guid, scriptable);
        }
    }
    
    [ButtonGroup("Button Group 1")]
    [Button]
    private void Validate()
    {
        if(inspector_root_data == null)
        {
            Debug.LogError("root_data is null!");
            return;
        }

        List<ScriptableObject> scriptables = GetAllScriptables(scriptables_path);

        for (int i = 0; i < scriptables.Count; i++)
        {
            ScriptableObject scriptable = scriptables[i];
            if (!inspector_root_data.ContainsValue(scriptable))
            {
                string path = AssetDatabase.GetAssetPath(scriptable);

                Debug.LogError(path + " scriptable is not included in dictionary!");
            }
        }

        int index = -1;
        foreach (KeyValuePair<string, ScriptableObject> entry in inspector_root_data)
        {
            ScriptableObject scriptable = entry.Value;
            index++;

            string path = AssetDatabase.GetAssetPath(scriptable);

            if (scriptable == null)
            {
                Debug.LogError(entry.Key + " guid #" + index + " does not lead to an asset!");
                continue;
            }

            int count = GetScrtipablesCount(inspector_root_data, scriptable);
            if (count > 1)
            {
                Debug.LogError(path + " scriptable # " + index + " is included in dictionary " + count + " times!");
                continue;
            }

            if (!IsFileBelowDirectory(path, scriptables_path, "/"))
            {
                Debug.LogError(path + " scriptable # " + index + " is not in the " + scriptables_path + " folder!");
                continue;
            }
        }
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
            if (!inspector_root_data.ContainsValue(scriptables[i]))
            {
                inspector_root_data.Add(NewGUID(), scriptables[i]);
            }
        }
    }


    [TitleGroup("Testing With Editor", Alignment = TitleAlignments.Centered, GroupID = "Title 0")]
    [ButtonGroup("Title 0/Button Group 2")]
    [Button]
    private void SaveOnDisk()
    {
        List<ScriptableObject> scriptables = GetAllScriptables(scriptables_path);

        for (int i = 0; i < scriptables.Count; i++)
        {
            SaveOnDisk(scriptables[i]);
        }
    }

    [ButtonGroup("Title 0/Button Group 2")]
    [Button]
    private void LoadFromDisk()
    {
        List<ScriptableObject> scriptables = GetAllScriptables(scriptables_path);

        for (int i = 0; i < scriptables.Count; i++)
        {
            LoadFromDisk(scriptables[i]);
        }
    }

    private List<ScriptableObject> GetAllScriptables(string scriptables_path)
    {
        List<string> guids = new List<string>(AssetDatabase.FindAssets("t: ScriptableObject", new string[] { scriptables_path }));
        List<ScriptableObject> scriptables = new List<ScriptableObject>();
        for (int i = 0; i < guids.Count; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            scriptables.Add(AssetDatabase.LoadAssetAtPath<ScriptableObject>(path));
        }

        return scriptables;
    }

#endif

    // This method assumes that the given "data" is saved under the "guid" key on disk & in root_data. If not - return
    public static void LoadFromDisk(ScriptableObject data)
    {
        string guid = GetFirstGUIDByScriptable(root_data, data);

        // This will filter away scriptables that are not in the scriptables_path folder
        if (guid == "") return;

        if (!ES3.KeyExists(guid, main_data_path)) return;

        if (data is IntVariable int_var)
        {
            int_var.v = ES3.Load<int>(guid, main_data_path);
        }
        else if (data is StringVariable string_var)
        {
            string_var.v = ES3.Load<string>(guid, main_data_path);
        }
        else if (data is SODict sodict)
        {
            // We will use dictionary methods staright from "sodict.v" instead of "sodict" to avoid calling SaveOnDisk
            sodict.v.Clear();

            Dictionary<string, MetaData> sodict_metadata = ES3.Load<Dictionary<string, MetaData>>(guid, main_data_path);

            foreach (KeyValuePair<string, MetaData> entry in sodict_metadata)
            {
                MetaData meta_data = entry.Value;

                if (root_data.ContainsKey(meta_data.guid))
                {
                    // So that editor_created stuff will have up-to-date names
                    string root_data_name = root_data[meta_data.guid].name;
                    // It is editor_created / code_created that was
                    // already initialized with the "else" clause below
                    sodict.v.Add(root_data_name, root_data[meta_data.guid] );
                }
                else
                {
                    string disk_saved_name = entry.Key;
                    // It is code_created that was not yet initialized
                    sodict.v.Add( entry.Key, LoadSOD(meta_data.type_string, meta_data.guid, meta_data.data_changes) );
                }
            }
        }
        else if (data is SOList solist)
        {
            // We will use list methods staright from "solist.v" instead of "solist" to avoid calling SaveOnDisk
            solist.v.Clear();

            List<MetaData> solist_metadata = ES3.Load< List<MetaData> >(guid, main_data_path);

            for (int i = 0; i < solist_metadata.Count; i++)
            {
                MetaData meta_data = solist_metadata[i];

                if (root_data.ContainsKey(meta_data.guid))
                {
                    // It is editor_created / code_created that was
                    // already initialized with the "else" clause below
                    solist.v.Add(root_data[meta_data.guid]);
                }
                else
                {
                    // It is code_created that was not yet initialized
                    solist.v.Add(LoadSOD(meta_data.type_string, meta_data.guid, meta_data.data_changes));
                }
            }
        }
    }

    // Saves data in the appropriate format by the given guid
    public static void SaveOnDisk(ScriptableObject data)
    {
        string guid = GetFirstGUIDByScriptable(root_data, data);

        // This will filter away scriptables that are not in the scriptables_path folder
        if (guid == "") return;

        if (data is IntVariable int_var)
        {
            ES3.Save<int>(guid, int_var.v, main_data_path);
        }
        else if (data is StringVariable string_var)
        {
            ES3.Save<string>(guid, string_var.v, main_data_path);
        }
        else if (data is SODict sodict)
        {
            Dictionary<string, MetaData> sodict_metadata = new Dictionary<string, MetaData>();

            foreach (KeyValuePair<string, ScriptableObject> entry in sodict.v)
            {
                ScriptableObject scriptable = entry.Value;

                string type_string = scriptable.GetType().ToString();
                string scriptable_guid = GetFirstGUIDByScriptable(root_data, scriptable);
                data_changes_enum data_changes = GetDataChanges(scriptable);

                if (scriptable_guid == "")
                {
                    // It should not happen under any circumstances, because
                    // editor_created stuff is always in root_data
                    // code_created stuff goes through CreateSOD
                    // and so all data inside SODict is added in root_data before this method

                    Debug.LogError(scriptable.name + " scriptable does not exist in root_data!");
                }

                sodict_metadata.Add( entry.Key, new MetaData(scriptable_guid, type_string, data_changes) );
            }

            ES3.Save<Dictionary<string, MetaData>>(guid, sodict_metadata, main_data_path);
        }
        else if (data is SOList solist)
        {
            List<MetaData> solist_metadata = new List<MetaData>();

            for (int i = 0; i < solist.v.Count; i++)
            {
                ScriptableObject scriptable = solist.v[i];

                string type_string = scriptable.GetType().ToString();
                string scriptable_guid = GetFirstGUIDByScriptable(root_data, scriptable);
                data_changes_enum data_changes = GetDataChanges(scriptable);

                if (scriptable_guid == "")
                {
                    // It should not happen under any circumstances, because
                    // editor_created stuff is always in root_data
                    // code_created stuff goes through CreateSOD
                    // and so all data inside SODict is added in root_data before this method

                    Debug.LogError(scriptable.name + " scriptable does not exist in root_data!");
                }

                solist_metadata.Add(new MetaData(scriptable_guid, type_string, data_changes));
            }

            ES3.Save<List<MetaData>>(guid, solist_metadata, main_data_path);
        }

    }

    public static string GetFirstGUIDByScriptable(Dictionary<string, ScriptableObject> dict, ScriptableObject scriptable)
    {
        foreach (KeyValuePair<string, ScriptableObject> entry in dict)
        {
            if(entry.Value == scriptable)
            {
                return entry.Key;
            }
        }

        return "";
    }

    public static int GetScrtipablesCount(Dictionary<string, ScriptableObject> dict, ScriptableObject scriptable)
    {
        int count = 0;

        foreach (KeyValuePair<string, ScriptableObject> entry in dict)
        {
            if (entry.Value == scriptable)
            {
                count++;
            }
        }

        return count;
    }

    public static data_changes_enum GetDataChanges(ScriptableObject scriptable)
    {
        if (scriptable is IntVariable int_var)
        {
            return int_var.data_changes;
        }
        else if (scriptable is StringVariable string_var)
        {
            return string_var.data_changes;
        }
        else if (scriptable is SODict sodict)
        {
            return sodict.data_changes;
        }
        else if (scriptable is SOList solist)
        {
            return solist.data_changes;
        }
        else
        {
            throw new Exception(scriptable.name + " is not supported type!");
        }
    }

    public static void SetDataChanges(ScriptableObject scriptable, data_changes_enum new_data_changes)
    {
        if (scriptable is IntVariable int_var)
        {
            int_var.data_changes = new_data_changes;
        }
        else if (scriptable is StringVariable string_var)
        {
            string_var.data_changes = new_data_changes;
        }
        else if (scriptable is SODict sodict)
        {
            sodict.data_changes = new_data_changes;
        }
        else if (scriptable is SOList solist)
        {
            solist.data_changes = new_data_changes;
        }
        else
        {
            throw new Exception(scriptable.name + " is not supported type!");
        }
    }

    public static void SetCreatedType(ScriptableObject scriptable, created_type_enum new_created_type)
    {
        if (scriptable is IntVariable int_var)
        {
            int_var.created_type = new_created_type;
        }
        else if (scriptable is StringVariable string_var)
        {
            string_var.created_type = new_created_type;
        }
        else if (scriptable is SODict sodict)
        {
            sodict.created_type = new_created_type;
        }
        else if (scriptable is SOList solist)
        {
            solist.created_type = new_created_type;
        }
        else
        {
            throw new Exception(scriptable.name + " is not supported type!");
        }
    }

    public static ScriptableObject LoadSOD(string type_string, string guid, data_changes_enum data_changes)
    {
        if (!ES3.KeyExists(guid, main_data_path)) return null;

        ScriptableObject new_scriptable = ScriptableObject.CreateInstance(type_string);
        SetDataChanges(new_scriptable, data_changes);
        SetCreatedType(new_scriptable, created_type_enum.code_created);
        root_data.Add(guid, new_scriptable);

        LoadFromDisk(new_scriptable);

        return new_scriptable;
    }

    public static void CopySOList(List<ScriptableObject> from, List<ScriptableObject> to)
    {
        to.Clear();
        for (int i = 0; i < from.Count; i++)
        {
            to.Add(from[i]);
        }
    }

    public static void CopySODict(Dictionary<string, ScriptableObject> from, Dictionary<string, ScriptableObject> to)
    {
        to.Clear();
        foreach (KeyValuePair<string, ScriptableObject> entry in from)
        {
            to.Add(entry.Key, entry.Value);
        }
    }

    private static void SetStartValue(ScriptableObject scriptable, object value)
    {
        if (scriptable is IntVariable int_var)
        {
            int_var.v = (int)value;
        }
        else if (scriptable is StringVariable string_var)
        {
            string_var.v = (string)value;
        }
        else if (scriptable is SODict sodict)
        {
            CopySODict(sodict.v, (Dictionary<string, ScriptableObject>) value);
        }
        else if (scriptable is SOList solist)
        {
            CopySOList(solist.v, (List<ScriptableObject>) value);
        }
    }

    // Start data for non-persistent stuff
    public static ScriptableObject CreateSOD(string type_string, data_changes_enum data_changes, object start_value)
    {
        string guid = NewGUID();

        ScriptableObject new_scriptable = ScriptableObject.CreateInstance(type_string);
        SetDataChanges(new_scriptable, data_changes);
        SetCreatedType(new_scriptable, created_type_enum.code_created);
        root_data.Add(guid, new_scriptable);

        SetStartValue(new_scriptable, start_value);

        // Make sure that we save new_scriptable on disk
        SaveOnDisk(new_scriptable);

        return new_scriptable;
    }

}