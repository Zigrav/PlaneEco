using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RotationSmoother : MonoBehaviour
{
    public GOVariable focus_point;
    public GOVariable sphere_focus_point;

    public UnityEvent on_start;
    public UnityEvent on_fixed_update;

    public float soft_zone_angle;
    public AnimationCurve rotation_speed_curve;
    public float rotation_speed_coeff;

    public float curve_min_rotation;
    public float curve_max_rotation;

    public float sphere_radius = 1.0f;

    public bool paint_sphere = false;

    public bool is_initiated = false;

    private void Start()
    {
        // Debug.Log("on start cam rot:" + transform.rotation.eulerAngles);
        on_start.Invoke();
    }

    private void FixedUpdate()
    {
        // Debug.Log("fixed:" + transform.rotation.eulerAngles);
        // Debug.Log("before rotation: " + transform.rotation.eulerAngles + " sphere_focus_point: " + sphere_focus_point.go.transform.position + " cam pos: " + transform.position);
        on_fixed_update.Invoke();
        // Debug.Log("after rotation: " + transform.rotation.eulerAngles + " sphere_focus_point: " + sphere_focus_point.go.transform.position + " cam pos: " + transform.position);
    }

    public void Smooth()
    {
        Transform focus_point_trans = focus_point.go.transform;
        Transform sphere_focus_point_trans = sphere_focus_point.go.transform;

        if (!is_initiated)
        {
            ProjectOnSphere(sphere_focus_point_trans, focus_point_trans.position);
            is_initiated = true;
            return;
        }

        Vector3 focus_dir = Vector3.Normalize(focus_point_trans.position - transform.position);
        Vector3 sphere_focus_dir = Vector3.Normalize(sphere_focus_point_trans.position - transform.position);

        Quaternion focus_rot = Quaternion.LookRotation(focus_dir);
        Quaternion sphere_focus_rot = Quaternion.LookRotation(sphere_focus_dir);

        float difference_angle = Quaternion.Angle(focus_rot, sphere_focus_rot);

        if (difference_angle > soft_zone_angle)
        {
            float curve_evaluate_angle = Mathf.Min(curve_max_rotation, difference_angle);
            curve_evaluate_angle = Mathf.Max(curve_min_rotation, difference_angle);

            float rotation_speed = rotation_speed_curve.Evaluate(curve_evaluate_angle) * rotation_speed_coeff;
            Quaternion evaluated_rotation = Quaternion.RotateTowards(sphere_focus_rot, focus_rot, Time.fixedDeltaTime * rotation_speed);

            sphere_focus_point_trans.rotation = evaluated_rotation;
            sphere_focus_point_trans.position = transform.position + (sphere_focus_point_trans.forward * sphere_radius);
        }
        else
        {
            ProjectOnSphere(sphere_focus_point_trans, focus_point_trans.position);
        }
    }

    public void ProjectOnSphere(Transform trans, Vector3 projected_point)
    {
        Vector3 projection_dir = Vector3.Normalize(projected_point - transform.position);
        Vector3 new_pos = transform.position + (projection_dir * sphere_radius);
        trans.position = new_pos;
    }

    public void ProjectSphereFocusPoint()
    {
        ProjectOnSphere(sphere_focus_point.go.transform, focus_point.go.transform.position);
    }

    private void OnDrawGizmosSelected()
    {
        if (!paint_sphere) return;

        Gizmos.DrawWireSphere(transform.position, sphere_radius);
    }
}
