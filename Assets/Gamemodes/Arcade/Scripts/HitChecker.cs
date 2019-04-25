using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathCreation;

public class HitChecker : MonoBehaviour
{
    public UnityEvent will_hit;

    public TargetVariable curr_target;
    
    private AimManager aim_manager;
    private VertexPath char_fly_vertex_path;

    private void Awake()
    {
        aim_manager = GameObject.Find("AimManager").GetComponent<AimManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Collided with: " + other.gameObject.name);

        // Find the GameObject that HitChecker has collided with
        GameObject other_obj = other.gameObject;

        // Find the TargetCollider of the current target
        GameObject TargetCollider = curr_target.v.gameObject.transform.Find("TargetCollider").gameObject;

        if (other_obj == TargetCollider)
        {
            // Debug.Log("Target Will Be Hit!");
            // Pl hit the target
            will_hit.Invoke();
        }
    }

    public void MoveToCharJumpPathEnd()
    {
        char_fly_vertex_path = aim_manager.char_fly_vertex_path;
        
        // Debug.Log("x: " + char_fly_vertex_path.GetPoint(0.999f).x + "z: " + char_fly_vertex_path.GetPoint(0.999f).z);
        transform.position = char_fly_vertex_path.GetPoint(0.999f);
    }

    public void MoveAway()
    {
        transform.position = new Vector3(0.0f, 100.0f, 0.0f);
    }

}