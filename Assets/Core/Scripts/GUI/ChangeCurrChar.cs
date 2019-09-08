using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCurrChar : MonoBehaviour
{
    [SerializeField]
    private SODict curr_char = null;

    [SerializeField]
    private SOList owned_characters = null;

    [SerializeField]
    private GameEvent curr_char_updated = null;

    public void Change(SODict new_char_info)
    {
        if (owned_characters.v.Contains(new_char_info))
        {
            string single_key = GetSingleElem.GetKey(curr_char);
            curr_char.ChangeValue(single_key, new_char_info);

            curr_char_updated.Raise();
        }
    }
}
