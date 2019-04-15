using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharBoxCollider : MonoBehaviour
{
    public UnityEvent pl_missed;
    public UnityEvent pl_hit;

    public TargetVariable curr_target;

    void OnTriggerEnter(Collider other)
    {
        GameObject other_obj = other.gameObject;

        if (other_obj == curr_target.v.gameObject)
        {
            Debug.Log("from box 111 collision: hit");
            // Pl hit the target
            pl_hit.Invoke();
        }
        else
        {
            Debug.Log("from box 111 collision: missed");

            // Player missed
            pl_missed.Invoke();
        }
    }
}