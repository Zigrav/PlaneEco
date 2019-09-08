using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSingleElem
{
    public static ScriptableObject Get(SODict sodict)
    {
        // sodict should always have only one key
        if (sodict.v.Count != 1)
        {
            throw new System.Exception("The curr_char SODict count does not equal 1!");
        }

        // Get a single elem from that single key
        SODict single_elem = null;
        foreach (KeyValuePair<string, ScriptableObject> entry in sodict.v)
        {
            single_elem = sodict.v[entry.Key] as SODict;
        }

        return single_elem;
    }

    public static string GetKey(SODict sodict)
    {
        // sodict should always have only one key
        if (sodict.v.Count != 1)
        {
            throw new System.Exception("The curr_char SODict count does not equal 1!");
        }

        // Get a single key from that sodict
        string single_key = null;
        foreach (KeyValuePair<string, ScriptableObject> entry in sodict.v)
        {
            single_key = entry.Key;
        }

        return single_key;
    }
}
