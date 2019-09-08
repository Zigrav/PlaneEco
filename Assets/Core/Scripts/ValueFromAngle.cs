using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueFromAngle : MonoBehaviour
{
    public Object deviated_point;
    public Object target_point;
    public Object origin_point;

    public AnimationCurve deviation_from_angle;

    public float min_angle;
    public float max_angle;

    public float max_deviation;

    private Transform deviated_point_transform;
    private Transform target_point_transform;
    private Transform origin_point_transform;

    [HideInInspector]
    public bool corrupted_data = false;

    private bool CollectData()
    {
        corrupted_data = false;

        bool is_go_var_defined = false; bool is_it_go_var = false;
        deviated_point_transform = ParseForTransform.Parse(deviated_point, ref is_it_go_var, ref is_go_var_defined);
        if (is_it_go_var && !is_go_var_defined) corrupted_data = true;

        is_go_var_defined = false; is_it_go_var = false;
        target_point_transform = ParseForTransform.Parse(target_point, ref is_it_go_var, ref is_go_var_defined);
        if (is_it_go_var && !is_go_var_defined) corrupted_data = true;

        is_go_var_defined = false; is_it_go_var = false;
        origin_point_transform = ParseForTransform.Parse(origin_point, ref is_it_go_var, ref is_go_var_defined);
        if (is_it_go_var && !is_go_var_defined) corrupted_data = true;

        return corrupted_data;
    }

    public float GetValueFromAngle()
    {
        if (CollectData())
        {
            Debug.Log(corrupted_data);
            return 0.0f;
        }

        Vector3 deviated_point_pos = deviated_point_transform.position;
        Vector3 target_point_pos = target_point_transform.position;
        Vector3 origin_point_pos = origin_point_transform.position;

        // Calc Path Deviation
        Vector3 origin_point_to_target_point = target_point_pos - origin_point_pos;
        Vector3 origin_point_to_deviated_point = deviated_point_pos - origin_point_pos;

        // Calculate angle deviation
        float angle_deviation = Vector3.SignedAngle(origin_point_to_target_point, origin_point_to_deviated_point, Vector3.up);
        float sign = angle_deviation > 0 ? -1.0f : 1.0f;
        angle_deviation = Mathf.Abs(angle_deviation);
        angle_deviation = Mathf.Clamp((angle_deviation / max_angle), min_angle / max_angle, 1.0f);
        angle_deviation = deviation_from_angle.Evaluate(angle_deviation) * sign;
        float deviation = angle_deviation * max_deviation;

        return deviation;
    }
}
