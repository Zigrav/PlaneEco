using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshValues : MonoBehaviour
{
    [SerializeField]
    private List<ScriptableObject> exceptions = new List<ScriptableObject>();

    public void Refresh()
    {
        // Refresh All SODs
        foreach (KeyValuePair<string, ScriptableObject> entry in DataContainer.root_data)
        {
            ScriptableObject sod = DataContainer.root_data[entry.Key];

            if (exceptions.Contains(sod))
            {
                continue;
            }

            if (sod is IntVariable int_var)
            {
                int_var.Refresh();
            }
            else if (sod is StringVariable string_var)
            {
                string_var.Refresh();
            }
            else if (sod is BoolVariable bool_var)
            {
                bool_var.Refresh();
            }
            else if (sod is FloatVariable float_var)
            {
                float_var.Refresh();
            }
            else if (sod is SODict sodict)
            {
                sodict.Refresh();
            }
            else if (sod is SOList solist)
            {
                solist.Refresh();
            }
        }

        // Refresh All Golists / GoVars
        for (int i = 0; i < GODataContainer.go_vars_root_data.Count; i++)
        {
            ScriptableObject scriptable = GODataContainer.go_vars_root_data[i];

            if (scriptable is GOVariable go_var)
            {
                go_var.Refresh();
            }
            else if (scriptable is GOList go_list)
            {
                go_list.Refresh();
            }
        }

    }
}
