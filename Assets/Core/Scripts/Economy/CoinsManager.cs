using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CoinsManager : MonoBehaviour
{
    [SerializeField]
    private int coins = 0;

    [SerializeField]
    private IntVariable char_coins = null;

    [FoldoutGroup("Get Coins From Difficulty List")]
    [SerializeField]
    private SOList difficulty_list = null;

    public void SetCoins(int val)
    {
        coins = val;
    }

    public void SetCoinsByList(IntVariable levels_passed)
    {
        SODict elem = GetElemByLevel.Get(levels_passed.v - 1, difficulty_list);
        coins = (elem.v["coins"] as IntVariable).v;
    }

    public void AddCoins(int addition)
    {
        coins += addition;
    }

    public void MultiplyCoins(float coeff)
    {
        coins = (int)(coins * coeff);
    }

    public void MultiplyCoins(FloatVariable coeff_var)
    {
        coins = (int)(coins * coeff_var.v);
    }

    // Give coins to character
    public void GiveCoins()
    {
        char_coins.v += coins;
    }

    // Give coins to character
    public int GetCoins()
    {
        return coins;
    }
}
