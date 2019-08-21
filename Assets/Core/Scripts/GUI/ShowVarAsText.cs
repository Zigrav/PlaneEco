using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowVarAsText : MonoBehaviour
{
    [SerializeField]
    private ScriptableObject variable = null;

    [SerializeField]
    private TextMeshProUGUI text_mesh_pro = null;

    private void Update()
    {
        if(variable is IntVariable int_var)
        {
            text_mesh_pro.text = int_var.v.ToString();
        }
        else if (variable is StringVariable string_var)
        {
            text_mesh_pro.text = string_var.v;
        }
        else if (variable is BoolVariable bool_var)
        {
            text_mesh_pro.text = bool_var.v.ToString();
        }
        else if (variable is FloatVariable float_var)
        {
            text_mesh_pro.text = float_var.v.ToString();
        }
    }

}
