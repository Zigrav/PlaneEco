// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;

[CreateAssetMenu]
public class BoolVariable : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif
    public bool bool_value;
    public bool refresh_on_load;
    public bool refresh_value;

    public void OnEnable()
    {
        // Renew The Value Each Time
        if (refresh_on_load)
        {
            bool_value = refresh_value;
        }
    }

    public bool v
    {
        get
        {
            return bool_value;
        }
        set
        {
            bool_value = value;
        }
    }

}


//using UnityEngine;

//[CreateAssetMenu]
//public class FloatVariable : ScriptableObject
//{
//#if UNITY_EDITOR
//    [Multiline]
//    public string DeveloperDescription = "";
//#endif
//    public float Value;

//    public void SetValue(float value)
//    {
//        Value = value;
//    }

//    public void SetValue(FloatVariable value)
//    {
//        Value = value.Value;
//    }

//    public void ApplyChange(float amount)
//    {
//        Value += amount;
//    }

//    public void ApplyChange(FloatVariable amount)
//    {
//        Value += amount.Value;
//    }
//}