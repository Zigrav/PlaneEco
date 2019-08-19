using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParseForTransform
{
    public static Transform Parse(UnityEngine.Object obj, ref bool is_it_go_var, ref bool is_go_var_defined)
    {
        is_it_go_var = false;

        if (obj is Transform t)
        {
            return t;
        }
        else if (obj is GameObject go)
        {
            return go.transform;
        }
        else if (obj is GOVariable go_var)
        {
            is_it_go_var = true;

            if (go_var.go == null)
            {
                is_go_var_defined = false;
                return null;
            }
            else
            {
                is_go_var_defined = true;
            }

            return go_var.go.transform;
        }
        else
        {
            Debug.LogError("Given Object Cannot Be Parsed Or It Is Null");
            return null;
        }
    }
}