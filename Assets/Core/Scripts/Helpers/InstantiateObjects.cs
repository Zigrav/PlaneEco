using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InstantiateObjects : MonoBehaviour
{
    public GameObject obj;
    public int count = 1;

    public UnityEvent on_start;
    public UnityEvent on_fixed_update;

    private void Start()
    {
        on_start.Invoke();
    }

    private void FixedUpdate()
    {
        on_fixed_update.Invoke();
    }

    public void Instantiate()
    {
        for (int i = 0; i < count; i++)
        {
            Instantiate(obj);
        }
    }
}
