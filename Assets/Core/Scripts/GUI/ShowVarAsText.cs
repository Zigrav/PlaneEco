using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class ShowVarAsText : MonoBehaviour
{
    private enum show_var_mode_enum { sod, script };

    [SerializeField, EnumToggleButtons, HideLabel]
    private show_var_mode_enum show_var_mode = show_var_mode_enum.sod;

    [SerializeField, ShowIf("show_var_mode", show_var_mode_enum.sod)]
    private ScriptableObject variable = null;

    [SerializeField]
    private TextMeshProUGUI text_mesh_pro = null;

    private void Update()
    {
        if(show_var_mode == show_var_mode_enum.sod)
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

    public void UpdateByCoinsManager(CoinsManager coins_manager)
    {
        text_mesh_pro.text = coins_manager.GetCoins().ToString();
    }
}
