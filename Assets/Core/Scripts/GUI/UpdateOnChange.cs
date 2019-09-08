using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateOnChange : MonoBehaviour
{
    [SerializeField]
    private ScriptableObject value = null;

    private int prev_int_value;
    private string prev_string_value;
    private bool prev_bool_value;
    private float prev_float_value;

    [SerializeField]
    private HorizontalLayoutGroup horizontal_layout_group = null;

    private bool add = true;

    public void UpdateIfNeeded()
    {
        bool changed = false;

        if (value is IntVariable int_var)
        {
            if (int_var.v != prev_int_value)
            {
                changed = true;
                prev_int_value = int_var.v;
            }
        }
        else if (value is StringVariable string_var)
        {
            if (string_var.v != prev_string_value)
            {
                changed = true;
                prev_string_value = string_var.v;
            }
        }
        else if (value is BoolVariable bool_var)
        {
            if (bool_var.v != prev_bool_value)
            {
                changed = true;
                prev_bool_value = bool_var.v;
            }
        }
        else if (value is FloatVariable float_var)
        {
            if (float_var.v != prev_float_value)
            {
                changed = true;
                prev_float_value = float_var.v;
            }
        }

        if (changed)
        {
            Canvas.ForceUpdateCanvases();
            horizontal_layout_group.enabled = false;
            horizontal_layout_group.enabled = true;

            if (add)
            {
                horizontal_layout_group.spacing += 0.01f;
            }
            else
            {
                horizontal_layout_group.spacing -= 0.01f;
            }
            add = !add;

            changed = false;
        }
    }

    public void ForceUpdate()
    {
        Canvas.ForceUpdateCanvases();
        horizontal_layout_group.enabled = false;
        horizontal_layout_group.enabled = true;

        if (add)
        {
            horizontal_layout_group.spacing += 0.01f;
        }
        else
        {
            horizontal_layout_group.spacing -= 0.01f;
        }
        add = !add;
    }

}
