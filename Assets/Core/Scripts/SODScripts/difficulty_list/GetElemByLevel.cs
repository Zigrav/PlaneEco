using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetElemByLevel
{
    public static SODict Get(int level, SOList difficulty_list)
    {
        for (int i = 0, l = difficulty_list.v.Count; i < l; i++)
        {
            SODict level_difficulty_elem = difficulty_list.v[i] as SODict;

            int from_lvl = (level_difficulty_elem.v["from_lvl"] as IntVariable).v;
            int to_lvl = (level_difficulty_elem.v["to_lvl"] as IntVariable).v;

            if (from_lvl <= level && level <= to_lvl)
            {
                return level_difficulty_elem;
            }

            // If none is appropriate, the level is higher than the to_lvl of the last elem
            // So just return the last configured setting
            if(i == l - 1)
            {
                return level_difficulty_elem;
            }
        }

        return null;
    }
}
