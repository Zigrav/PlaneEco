using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class BuyChar : MonoBehaviour
{
    [SerializeField]
    private SODict char_info = null;

    [SerializeField]
    private SOList owned_characters = null;

    [SerializeField]
    private IntVariable coins = null;

    [FoldoutGroup("Events", Expanded = false)]
    [SerializeField]
    private UnityEvent OnSuccess = null;

    [FoldoutGroup("Events", Expanded = false)]
    [SerializeField]
    private UnityEvent OnFailure = null;

    public void Buy()
    {
        int char_price = (char_info.v["price"] as IntVariable).v;

        // If not enough coins
        if (coins.v < char_price)
        {
            OnFailure.Invoke();
            return;
        }

        // If the char is already bought
        if (owned_characters.v.Contains(char_info as ScriptableObject))
        {
            OnFailure.Invoke();
            return;
        }

        owned_characters.Add(char_info);
        coins.v -= char_price;

        OnSuccess.Invoke();
    }

}
