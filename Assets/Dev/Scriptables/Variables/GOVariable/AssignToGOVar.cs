using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignToGOVar : MonoBehaviour
{
    public GOVariable go_var;

    private void Awake()
    {
        if(go_var != null)
        {
            go_var.go = gameObject;
        }
    }
}
