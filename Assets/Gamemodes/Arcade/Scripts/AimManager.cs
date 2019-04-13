using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class AimManager : MonoBehaviour
{
    public PathCreator aim_spawn_area;
    public float aim_walk_path_max_deviation;
    public float aim_walk_path_prolong_dist;
    public VertexPath vertex_aim_path;
    public float aim_move_speed = 0.0f;

    public PathCreator char_fly_path;
    public VertexPath char_fly_vertex_path;

    public float fly_path_height;
    private float fly_path_fraction = 0.0f;

    public PillarsRuntimeSet pillars;

    public TargetVariable curr_target;
    public Transform char_transform;

    private float aim_spawn_area_max_x;
    private float aim_spawn_area_min_x;

    private bool moving_to_target = false;

    public float aim_render_fraction = 0.8f;

    public GameObject test_thing;

    private LineRenderer line_renderer;

    // Awake is called before the first frame update
    void Awake()
    {
        line_renderer = GetComponent<LineRenderer>();

        aim_spawn_area_max_x = aim_spawn_area.path.bounds.center.x + aim_spawn_area.path.bounds.extents.x;
        aim_spawn_area_min_x = aim_spawn_area.path.bounds.center.x - aim_spawn_area.path.bounds.extents.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveToTarget();

        PaintAimPath();
    }

    void MoveToTarget()
    {
        if(moving_to_target)
        {
            fly_path_fraction += aim_move_speed;

            char_fly_vertex_path = SetAimAtFraction(fly_path_fraction);
            AdjustRendererByAim(char_fly_vertex_path);
        }
    }

    VertexPath SetAimAtFraction(float fraction)
    {
        Vector3 char_pos = char_transform.position;
        Vector3 curr_aim_post = vertex_aim_path.GetPoint(fraction);
        Vector3 middle_point = new Vector3(0.0f, 0.0f, 0.0f);

        middle_point.x = char_pos.x + (curr_aim_post.x - char_pos.x) / 2;
        middle_point.y = char_pos.y + (curr_aim_post.y - char_pos.y) / 2;
        middle_point.y += fly_path_height;
        middle_point.z = char_pos.z + (curr_aim_post.z - char_pos.z) / 2;

        char_fly_path.bezierPath.MovePoint(0, char_pos);
        char_fly_path.bezierPath.MovePoint(3, middle_point);
        char_fly_path.bezierPath.MovePoint(6, curr_aim_post);

        return new VertexPath(char_fly_path.bezierPath);
    }

    void AdjustRendererByAim(VertexPath char_fly_vertex_path)
    {
        float fraction_step = aim_render_fraction / 10.0f;
        Vector3[] line_renderer_points = new Vector3[10];

        for (int i = 0; i < 10; i++)
        {
            float fraction = fraction_step * i;

            line_renderer_points[i] = char_fly_vertex_path.GetPoint(fraction);
        }

        line_renderer.SetPositions(line_renderer_points);
    }

    void PaintAimPath()
    {
        if(vertex_aim_path != null)
        {
            for (int first_index = 0, l = (int)vertex_aim_path.vertices.Length; first_index < l; first_index++)
            {
                int second_index = first_index == l - 1 ? 0 : first_index + 1;

                Debug.DrawLine(vertex_aim_path.vertices[first_index], vertex_aim_path.vertices[second_index], new Color(1.0f, 1.0f, 1.0f));
            }
        }
    }

    public void StartMovingToTarget()
    {
        moving_to_target = true;
    }

    public void StopMovingToTarget()
    {
        moving_to_target = false;
    }
    
    [ContextMenu("SetUpAimPath")]
    public void SetUpAimPath()
    {
        // Get Aim Random Start Position
        Vector3 aim_start_pos = RandPointInPolygon.Get(aim_spawn_area_min_x, aim_spawn_area_max_x, aim_spawn_area.path.vertices);
        aim_start_pos.y = char_transform.position.y;

        // Get Current Target's Position
        Vector3 target_pos = curr_target.v.gameObject.GetComponent<Transform>().position;

        //Calculate Middle Point
        Vector3 middle_point = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 dest_vect = target_pos - aim_start_pos;

        middle_point.x = aim_start_pos.x + (dest_vect.x / 2);
        middle_point.y = aim_start_pos.y + (dest_vect.y / 2);
        middle_point.z = aim_start_pos.z + (dest_vect.z / 2);

        // Calc Path Random Deviation
        float deviation = Random.Range(-aim_walk_path_max_deviation, aim_walk_path_max_deviation);
        Vector3 deviation_norm = Vector3.Normalize(Vector3.Cross(new Vector3(0.0f, 1.0f, 0.0f), dest_vect));

        middle_point = middle_point + (deviation_norm * deviation);

        Vector3[] points = { aim_start_pos, middle_point, target_pos };

        BezierPath bezier_aim_path  = new BezierPath(points);
        vertex_aim_path = new VertexPath(bezier_aim_path);

        // Get path's forward vector
        Vector3 path_end_forward_vect = vertex_aim_path.GetDirection(0.99f);
        Vector3 path_end_point = vertex_aim_path.GetPoint(0.99f);
        path_end_point += path_end_forward_vect * aim_walk_path_prolong_dist;

        // Modify bezier path
        bezier_aim_path.AddSegmentToEnd(path_end_point);

        // Create new modified vertex path
        vertex_aim_path = new VertexPath(bezier_aim_path);

        SetAimAtFraction(0.0f);
    }
    
    [ContextMenu("test aim spawner")]
    public void TestAimSpawner()
    {
        aim_spawn_area_max_x = aim_spawn_area.path.bounds.center.x + aim_spawn_area.path.bounds.extents.x;
        aim_spawn_area_min_x = aim_spawn_area.path.bounds.center.x - aim_spawn_area.path.bounds.extents.x;

        for (int i = 0; i < 100; i++)
        {
            Vector3 pos = RandPointInPolygon.Get(aim_spawn_area_min_x, aim_spawn_area_max_x, aim_spawn_area.path.vertices);
            GameObject new_orb = Instantiate(test_thing, pos, Quaternion.identity) as GameObject;
        }
    }

}
