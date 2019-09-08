using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class GenerateArcPath1 : MonoBehaviour
{
    public VertexPath vertex_path;

    public Object first_point;
    public Object second_point;

    public int aim_path_points_num;
    public float prolong_dist;

    public ValueFromAngle deviation_from_angle;

    private Transform first_point_transform;
    private Transform second_point_transform;

    [SerializeField]
    private bool swap_arc = false;

    [HideInInspector]
    public bool corrupted_data = false;

    private bool CollectData()
    {
        corrupted_data = false;

        bool is_go_var_defined = false; bool is_it_go_var = false;
        first_point_transform = ParseForTransform.Parse(first_point, ref is_it_go_var, ref is_go_var_defined);
        if (is_it_go_var && !is_go_var_defined) corrupted_data = true;

        is_go_var_defined = false; is_it_go_var = false;
        second_point_transform = ParseForTransform.Parse(second_point, ref is_it_go_var, ref is_go_var_defined);
        if (is_it_go_var && !is_go_var_defined) corrupted_data = true;

        return corrupted_data;
    }

    public void CreateArcPath()
    {
        if (CollectData())
        {
            Debug.Log("CreateArcPath: " + corrupted_data);
            return;
        }

        if (aim_path_points_num <= 1)
        {
            Debug.LogError("aim_path_points_num: " + aim_path_points_num);
            return;
        }

        // Calculate the deviation value
        float deviation = deviation_from_angle.GetValueFromAngle();
        if (deviation_from_angle.corrupted_data)
        {
            Debug.Log("CreateArcPath Deviation: " + deviation_from_angle.corrupted_data);
            return;
        }
        deviation = swap_arc ? deviation * -1 : deviation; // Swap the arc if chosen

        // Get point positions
        Vector3 first_point_pos = first_point_transform.position;
        Vector3 second_point_pos = second_point_transform.position;

        //Calculate Middle Point
        Vector3 middle_point = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 first_point_to_second_point = second_point_pos - first_point_pos;

        middle_point.x = first_point_pos.x + (first_point_to_second_point.x / 2);
        middle_point.y = first_point_pos.y + (first_point_to_second_point.y / 2);
        middle_point.z = first_point_pos.z + (first_point_to_second_point.z / 2);

        // Calculate Circle Center
        Vector3 deviation_norm = Vector3.Normalize(Vector3.Cross(Vector3.up, first_point_to_second_point));
        Vector3 circle_center = middle_point + (deviation_norm * deviation);
        float circle_radius = Vector3.Distance(circle_center, first_point_pos);

        Debug.DrawLine(circle_center, middle_point);

        // Calculate Angle between fisrt_point and second_point and Angle Step
        float sign = deviation > 0 ? 1.0f : -1.0f;

        // First we get the angle in radians, and then we tranform them into degrees
        float prolong_angle = prolong_dist / circle_radius * (180 / Mathf.PI);

        float angle = Vector3.Angle(first_point_pos - circle_center, second_point_pos - circle_center) + prolong_angle;
        float angle_step = angle / (aim_path_points_num - 1) * sign;

        Debug.Log("angle: " + angle  + " angle_step: " + angle_step);

        // Prepare Path Points for Creating
        Vector3 curr_aim_path_point = first_point_pos - circle_center;
        Vector3[] path_points = new Vector3[aim_path_points_num];

        for (int i = 0; i < path_points.Length; i++)
        {
            // Pushing new point to an array
            path_points[i] = curr_aim_path_point + circle_center;

            // Moving curr_angle each step
            curr_aim_path_point = Quaternion.Euler(0.0f, angle_step, 0.0f) * curr_aim_path_point;
        }

        vertex_path = new VertexPath(new BezierPath(path_points));
    }

    private void OnDrawGizmos()
    {
        if (vertex_path == null) return; 

        for (int i = 0; i < vertex_path.NumVertices - 1; i++)
        {
            Gizmos.DrawLine(vertex_path.vertices[i], vertex_path.vertices[i + 1]);
        }
    }
}
