using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToGOList : MonoBehaviour
{
    public GOList go_list;

    private void Awake()
    {
        if (go_list != null)
        {
            go_list.Add(gameObject);
        }
    }
}
