using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownTime : MonoBehaviour
{
    public float slowing = 0.15f;

    private void Start()
    {
        Debug.Log("time slowed down");
        Time.timeScale = slowing;
        Time.fixedDeltaTime = slowing * 0.05f;
    }

    private void FixedUpdate()
    {
        
        Time.timeScale = slowing;
        Time.fixedDeltaTime = slowing * 0.05f;
    }
}