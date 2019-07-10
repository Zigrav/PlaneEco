// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public GameEvent Event;

    public int Order = 0;
    public bool UseConditions = false;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent Response;

    public enum variables_variant_enum
    {
        StringVariable,
        BoolVariable,
        IntVariable,
        FloatVariable
    }

    public variables_variant_enum variable_variant;

    public enum string_variants_enum
    {
        StringVariable,
        String
    }

    public string_variants_enum string_variant;

    public enum boolean_variants_enum
    {
        BoolVariable,
        Bool
    }

    public boolean_variants_enum boolean_variant;

    public enum int_variants_enum
    {
        IntVariable,
        Int
    }

    public int_variants_enum int_variant;

    public enum float_variants_enum
    {
        FloatVariable,
        Float
    }

    public float_variants_enum float_variant;

    public enum conditions_0_variants_enum
    {
        Equals,
        NotEquals,
        More,
        MoreEquals,
        Less,
        LessEquals
    }

    public conditions_0_variants_enum conditions_0_variant;

    public enum conditions_1_variants_enum
    {
        Equals,
        NotEquals
    }

    public conditions_1_variants_enum conditions_1_variant;

    public StringVariable first_string_var;
    public BoolVariable first_bool_var;
    public IntVariable first_int_var;
    public FloatVariable first_float_var;

    public StringVariable second_string_var;
    public BoolVariable second_bool_var;
    public IntVariable second_int_var;
    public FloatVariable second_float_var;

    public string second_string;
    public bool second_bool;
    public int second_int;
    public float second_float;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    [ContextMenu("OnEventRaised")]
    public void OnEventRaised()
    {
        if (UseConditions)
        {
            if (TestConditions())
            {
                Response.Invoke();
            }
        }
        else
        {
            Response.Invoke();
        }
    }

    public bool TestConditions()
    {
        switch (variable_variant)
        {
            case variables_variant_enum.StringVariable:
            string string_1 = (string_variant == string_variants_enum.StringVariable) ? second_string_var.v : second_string;
            return CompareStrings(first_string_var.v, string_1);

            case variables_variant_enum.BoolVariable:
            bool bool_1 = (boolean_variant == boolean_variants_enum.BoolVariable) ? second_bool_var.v : second_bool;
            return CompareBooleans(first_bool_var.v, bool_1);

            case variables_variant_enum.IntVariable:
            int int_1 = (int_variant == int_variants_enum.IntVariable) ? second_int_var.v : second_int;
            return CompareInts(first_int_var.v, int_1);

            case variables_variant_enum.FloatVariable:
            float float_1 = (float_variant == float_variants_enum.FloatVariable) ? second_float_var.v : second_float;
            return CompareFloats(first_float_var.v, float_1);

            default:
                return false;
        }

    }

    public bool CompareStrings(string string_0, string string_1)
    {
        switch (conditions_1_variant)
        {
            case conditions_1_variants_enum.Equals:
                return (string_0 == string_1);
            case conditions_1_variants_enum.NotEquals:
                return (string_0 != string_1);
            default:
                return false;
        }
    }

    public bool CompareBooleans(bool bool_0, bool bool_1)
    {
        switch (conditions_1_variant)
        {
            case conditions_1_variants_enum.Equals:
                return (bool_0 == bool_1);
            case conditions_1_variants_enum.NotEquals:
                return (bool_0 != bool_1);
            default:
                return false;
        }
    }

    public bool CompareFloats(float float_0, float float_1)
    {
        switch (conditions_0_variant)
        {
            case conditions_0_variants_enum.Equals:
                return (float_0 == float_1);
            case conditions_0_variants_enum.NotEquals:
                return (float_0 != float_1);
            case conditions_0_variants_enum.More:
                return (float_0 > float_1);
            case conditions_0_variants_enum.MoreEquals:
                return (float_0 >= float_1);
            case conditions_0_variants_enum.Less:
                return (float_0 < float_1);
            case conditions_0_variants_enum.LessEquals:
                return (float_0 <= float_1);
            default:
                return false;
        }
    }

    public bool CompareInts(int int_0, int int_1)
    {
        switch (conditions_0_variant)
        {
            case conditions_0_variants_enum.Equals:
                return (int_0 == int_1);
            case conditions_0_variants_enum.NotEquals:
                return (int_0 != int_1);
            case conditions_0_variants_enum.More:
                return (int_0 > int_1);
            case conditions_0_variants_enum.MoreEquals:
                return (int_0 >= int_1);
            case conditions_0_variants_enum.Less:
                return (int_0 < int_1);
            case conditions_0_variants_enum.LessEquals:
                return (int_0 <= int_1);
            default:
                return false;
        }
    }


}