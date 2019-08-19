using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlaneCollider : MonoBehaviour
{
    public Object plane_object;
    private Transform plane_object_trans;

    public UnityEvent on_start;
    public UnityEvent on_fixed_update;

    public UnityEvent is_up;
    public UnityEvent is_down;

    public UnityEvent moved_up;
    public UnityEvent moved_down;

    private int last_frame_status = -1;
    private int curr_frame_status = -1;

    [HideInInspector]
    public bool corrupted_data = false;

    public void CollectData()
    {
        corrupted_data = false;

        plane_object_trans = ParseObject(plane_object);
    }

    private Transform ParseObject(Object obj)
    {
        bool is_go_var_defined = false; bool is_it_go_var = false;
        Transform parsed_transform = ParseForTransform.Parse(obj, ref is_it_go_var, ref is_go_var_defined);
        if (is_it_go_var && !is_go_var_defined) corrupted_data = true;

        return parsed_transform;
    }

    private void Start()
    {
        on_start.Invoke();
    }

    private void FixedUpdate()
    {
        on_fixed_update.Invoke();
    }

    // Forget where the object was last frame: up or below this plane
    public void ResetStatuses()
    {
        last_frame_status = -1;
        curr_frame_status = -1;
    }

    public void CheckCollision()
    {
        // Transform Gameobject's Position Into The Local Space Of The plane_object
        Vector3 object_local_pos = plane_object_trans.InverseTransformPoint(transform.position);

        if (object_local_pos.y >= 0.0f)
        {
            curr_frame_status = 1;
            is_up.Invoke();
        }
        else
        {
            curr_frame_status = 0;
            is_down.Invoke();
        }

        // Debug.Log("checking collision plane collider: " + last_frame_status + " curr: " + curr_frame_status);
        // Debug.Log("object_local_pos: " + object_local_pos + " transform position: " + transform.position);

        // If Last Frame Status Is Present
        if(last_frame_status != -1)
        {
            if(curr_frame_status == 0 && last_frame_status == 1)
            {
                // If Moved Down
                moved_down.Invoke();
            }
            else if(curr_frame_status == 1 && last_frame_status == 0)
            {
                // If Moved Up
                moved_up.Invoke();
            }
        }

        last_frame_status = curr_frame_status;
    }

}
