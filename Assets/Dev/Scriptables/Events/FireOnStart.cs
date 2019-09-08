using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireOnStart : MonoBehaviour
{
    public UnityEvent OnAwake;

    private void Start()
    {
        OnAwake.Invoke();
    }
}
