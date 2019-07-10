// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;

[CreateAssetMenu]
public class StringVariable : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif
    public string string_value;
    public bool refresh_on_load;
    public string refresh_value;

    public void OnEnable()
    {
        // Refresh The Value Each Time
        if (refresh_on_load)
        {
            string_value = refresh_value;
        }
    }

    public string v
    {
        get
        {
            return string_value;
        }
        set
        {
            string_value = value;
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