using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignToGOVar : MonoBehaviour
{
    public GOVariable go_var;

    public void Assign()
    {
        if(go_var != null)
        {
            go_var.go = gameObject;
        }
    }
}
