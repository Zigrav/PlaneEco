using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDebuger : MonoBehaviour
{
    public void log(string text)
    {
        Debug.Log(text + ": " + gameObject.name);
    }

    public void log(StringVariable string_var)
    {
        Debug.Log(string_var.v + ": " + gameObject.name);
    }

    public void log(BoolVariable bool_var)
    {
        Debug.Log(bool_var.v + ": " + gameObject.name);
    }

    public void log(IntVariable int_var)
    {
        Debug.Log(int_var.v + ": " + gameObject.name);
    }

    public void log(FloatVariable float_var)
    {
        Debug.Log(float_var.v + ": " + gameObject.name);
    }
}
