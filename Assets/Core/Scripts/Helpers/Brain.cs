using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class Brain : MonoBehaviour
{
    [SerializeField, FoldoutGroup("Awake")]
    private UnityEvent OnAwake = null;

    [SerializeField, FoldoutGroup("Start")]
    private UnityEvent OnStart = null;

    [SerializeField, FoldoutGroup("Update")]
    private UnityEvent OnUpdate = null;

    [SerializeField, FoldoutGroup("Fixed Update")]
    private UnityEvent OnFixedUpdate = null;

    private void Awake()
    {
        OnAwake.Invoke();
    }

    void Start()
    {
        OnStart.Invoke();
    }

    void Update()
    {
        OnUpdate.Invoke();
    }

    void FixedUpdate()
    {
        OnFixedUpdate.Invoke();
    }
}
