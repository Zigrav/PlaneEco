using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAnimator : MonoBehaviour
{
    public Animator animator;

    public GOVariable first_object;
    public GOVariable second_object;

    public bool is_local = false;

    public Vector3 pos_offset;

    FloatVariable curr_bool_var;

    private string curr_param;

    public void SetParam(string par_name)
    {
        curr_param = par_name;
    }

    public void SetFloat(float value)
    {
        animator.SetFloat(curr_param, value);
    }

    public void SetInteger(int value)
    {
        animator.SetInteger(curr_param, value);
    }

    public void SetBool(bool value)
    {
        animator.SetBool(curr_param, value);
    }

    public void SetTrigger(string param)
    {
        animator.SetTrigger(param);
    }

    public void SetBoolVariable(FloatVariable var)
    {
        curr_bool_var = var;
        Debug.Log(curr_bool_var);
    }

    public void MoveToObjects()
    {
        if (is_local)
        {
            transform.position = first_object.go.transform.position;
            transform.position += (pos_offset.x * first_object.go.transform.right);
            transform.position += (pos_offset.y * first_object.go.transform.up);
            transform.position += (pos_offset.z * first_object.go.transform.forward);
        }
        else
        {
            transform.position = first_object.go.transform.position;
            transform.position += pos_offset;
        }

        if (second_object == null)
        {
            transform.rotation = first_object.go.transform.rotation;
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(Vector3.Normalize(second_object.go.transform.position - first_object.go.transform.position));
        }
    }

}
