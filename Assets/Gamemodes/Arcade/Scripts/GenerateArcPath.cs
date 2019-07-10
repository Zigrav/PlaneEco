using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class GenerateArcPath : MonoBehaviour
{
    public VertexPath vertex_path;

    public Object first_point;
    public Object second_point;

    public int aim_path_points_num;
    public float prolong_angle;

    public ValueFromAngle deviation_from_angle;

    private Transform first_point_transform;
    private Transform second_point_transform;

    [HideInInspector]
    public bool corrupted_data = false;

    // Start is called before the first frame update
    void Start()
    {
        CollectData();
    }

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

        // Calculate the deviation value
        float deviation = deviation_from_angle.GetValueFromAngle();
        if (deviation_from_angle.corrupted_data)
        {
            Debug.Log("CreateArcPath Deviation: " + deviation_from_angle.corrupted_data);
            return;
        }

        // Get point positions
        Vector3 first_point_pos = first_point_transform.position;
        Vector3 second_point_pos = second_point_transform.position;

        //Calculate Middle Point
        Vector3 middle_point = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 aim_start_to_target = second_point_pos - first_point_pos;

        middle_point.x = first_point_pos.x + (aim_start_to_target.x / 2);
        middle_point.y = first_point_pos.y + (aim_start_to_target.y / 2);
        middle_point.z = first_point_pos.z + (aim_start_to_target.z / 2);

        // Calculate Circle Center
        Vector3 deviation_norm = Vector3.Normalize(Vector3.Cross(Vector3.up, aim_start_to_target));
        Vector3 circle_center = middle_point + (deviation_norm * deviation);

        // Debug.DrawLine(middle_point, circle_center);

        // Calculate Angle between fisrt_point and second_point and Angle Step
        float sign = deviation > 0 ? 1.0f : -1.0f;
        float angle = Vector3.Angle(first_point_pos - circle_center, second_point_pos - circle_center) + prolong_angle;
        float angle_step = angle / aim_path_points_num * sign;

        // Prepare Path Points for Creating
        Vector3 curr_aim_path_point = first_point_pos - circle_center;
        Vector3[] path_points = new Vector3[aim_path_points_num];

        for (int i = 0; i < path_points.Length; i++)
        {
            //Debug.Log(i + " p: " + (curr_aim_path_point + circle_center));
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
