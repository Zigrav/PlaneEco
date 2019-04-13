// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System;

[Serializable]
public class FloatReference
{
    public bool UseLocal = true;
    public float LocalValue;
    public FloatVariable Variable;

    public FloatReference()
    { }

    public FloatReference(float value)
    {
        UseLocal = true;
        LocalValue = value;
    }

    public float v
    {
        get
        {
            if (UseLocal)
            {
                return LocalValue;
            }
            else
            {
                return Variable.Value;
            }
        }
        set
        {
            if(UseLocal)
            {
                LocalValue = value;
            }
            else
            {
                Variable.Value = value;
            }
        }
    }
}