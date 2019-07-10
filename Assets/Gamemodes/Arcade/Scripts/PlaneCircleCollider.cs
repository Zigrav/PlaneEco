using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlaneCircleCollider : MonoBehaviour
{
    public Object plane_circle_object;
    private Transform plane_circle_object_trans;

    public float radius = 1.0f;

    public UnityEvent on_start;
    public UnityEvent on_fixed_update;

    public UnityEvent is_inside;
    public UnityEvent is_outside;

    public UnityEvent moved_inside;
    public UnityEvent moved_outside;

    private int last_frame_status = -1;
    private int curr_frame_status = -1;

    [HideInInspector]
    public bool corrupted_data = false;

    public void CollectData()
    {
        corrupted_data = false;

        plane_circle_object_trans = ParseObject(plane_circle_object);
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

    public void ResetStatuses()
    {
        last_frame_status = -1;
        curr_frame_status = -1;
    }

    public void CheckCollision()
    {
        // Transform Gameobject's Position Into The Local Space Of The plane_circle_object
        Vector3 object_local_pos = plane_circle_object_trans.InverseTransformPoint(transform.position);

        // We Don't Need Local Y Here
        object_local_pos.y = 0.0f;

        // The Pos Is In Local Space So We Just Need Magnitude
        float distance = object_local_pos.magnitude;

        if (distance <= radius)
        {
            curr_frame_status = 1;
            is_inside.Invoke();
        }
        else
        {
            curr_frame_status = 0;
            is_outside.Invoke();
        }

        // Debug.Log("plane circle: " + object_local_pos + "curr: " + curr_frame_status + "previous: " + last_frame_status + "dist: " + distance);

        // If Last Frame Status Is Present
        if (last_frame_status != -1)
        {
            if (curr_frame_status == 0 && last_frame_status == 1)
            {
                // Moved Outside
                moved_outside.Invoke();
            }
            else if (curr_frame_status == 1 && last_frame_status == 0)
            {
                // Moved Inside
                moved_inside.Invoke();
            }
        }

        last_frame_status = curr_frame_status;
    }
}
