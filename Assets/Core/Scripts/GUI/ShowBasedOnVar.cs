using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBasedOnVar : MonoBehaviour
{
    private enum script_mode_enum { by_bool, by_list_inclusion };

    [SerializeField, EnumToggleButtons, HideLabel]
    private script_mode_enum script_mode = script_mode_enum.by_bool;

    [SerializeField, ShowIf("script_mode", script_mode_enum.by_bool)]
    private BoolVariable bool_var = null;

    [SerializeField, ShowIf("script_mode", script_mode_enum.by_bool)]
    private bool reverse_behaviour = false;

    [SerializeField, ShowIf("script_mode", script_mode_enum.by_list_inclusion)]
    private ScriptableObject test_elem = null;

    [SerializeField, ShowIf("script_mode", script_mode_enum.by_list_inclusion)]
    private SOList test_list = null;

    [SerializeField]
    private List<GameObject> elements = null;

    public void ShowHideByBool()
    {
        if(script_mode != script_mode_enum.by_bool)
        {
            throw new System.Exception("script_mode should be set to by_bool, when using ShowHideByBool method");
        }

        bool test_value = reverse_behaviour ? !bool_var.v : bool_var.v;

        if (test_value)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].GetComponent<PageController>().ShowPage();
            }
        }
        else
        {
            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].GetComponent<PageController>().HidePage();
            }
        }
    }

    public void ShowHideByList()
    {
        if (script_mode != script_mode_enum.by_list_inclusion)
        {
            throw new System.Exception("script_mode should be set to by_list_inclusion, when using ShowHideByList method");
        }

        if (test_list.v.Contains(test_elem))
        {
            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].GetComponent<PageController>().ShowPage();
            }
        }
        else
        {
            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].GetComponent<PageController>().HidePage();
            }
        }
    }
}
