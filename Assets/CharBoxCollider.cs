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
            // Pl hit the target
            pl_hit.Invoke();

            Debug.Log("from box 111 collision: hit");
        }
        else
        {
            // Player missed
            pl_missed.Invoke();

            Debug.Log("from box 111 collision: missed");
        }
    }
}