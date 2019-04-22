using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharSphereCollider : MonoBehaviour
{
    // public UnityEvent pl_missed;

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Sphere Collied");

        // Player missed
        // pl_missed.Invoke();
    }
}
