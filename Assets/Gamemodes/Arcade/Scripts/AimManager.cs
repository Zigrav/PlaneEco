using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

// [ExecuteInEditMode]
public class AimManager : MonoBehaviour
{
    public PathCreator aim_spawn_area;
    
    public float aim_walk_path_prolong_angle;
    public int aim_path_points_num;

    public VertexPath vertex_aim_path;
    public float aim_move_speed = 0.0f;

    public float max_deviation;
    public float min_angle;
    public float max_angle;

    public AnimationCurve deviation_from_angle;

    public PathCreator char_fly_path;
    public VertexPath char_fly_vertex_path;

    public float fly_path_height;
    // private float fly_path_fraction = 0.0f;

    public GOVariable curr_target;
    public Transform char_transform;

    private float aim_spawn_area_max_x;
    private float aim_spawn_area_min_x;

    private bool moving_to_target = false;

    public GameObject test_thing;

    // private LineRenderer line_renderer;

    public float aim_start_fraction;
    public float aim_end_fraction;
    public int aim_objects_count;
    public GameObject aim_object;
    private GameObject[] aim_objects;

    // Awake is called before the first frame update
    void Awake()
    {
        // line_renderer = GetComponent<LineRenderer>();
        InstantiateAimObjects();

        aim_spawn_area_max_x = aim_spawn_area.path.bounds.center.x + aim_spawn_area.path.bounds.extents.x;
        aim_spawn_area_min_x = aim_spawn_area.path.bounds.center.x - aim_spawn_area.path.bounds.extents.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // MoveToTarget();

        PaintAimPath();

        ///
        // SetUpAimPath();
        ///
    }

    void MoveToTarget()
    {
        if(moving_to_target)
        {
            // fly_path_fraction += (aim_move_speed / vertex_aim_path.length);

            // char_fly_vertex_path = SetAimAtFraction(fly_path_fraction);

            AdjustObjectsByAim(char_fly_vertex_path);
            // AdjustRendererByAim(char_fly_vertex_path);
        }
    }

    //VertexPath SetAimAtFraction(float fraction)
    //{
    //    Vector3 char_pos = char_transform.position;
    //    Vector3 curr_aim_post = vertex_aim_path.GetPoint(fraction);
    //    Vector3 middle_point = new Vector3(0.0f, 0.0f, 0.0f);

    //    middle_point.x = char_pos.x + (curr_aim_post.x - char_pos.x) / 2;
    //    middle_point.y = char_pos.y + (curr_aim_post.y - char_pos.y) / 2;
    //    middle_point.y += fly_path_height;
    //    middle_point.z = char_pos.z + (curr_aim_post.z - char_pos.z) / 2;

    //    char_fly_path.bezierPath.MovePoint(0, char_pos);
    //    char_fly_path.bezierPath.MovePoint(3, middle_point);
    //    char_fly_path.bezierPath.MovePoint(6, curr_aim_post);

    //    return new VertexPath(char_fly_path.bezierPath);
    //}

    //void AdjustRendererByAim(VertexPath char_fly_vertex_path)
    //{
    //    float fraction_step = aim_render_fraction / char_fly_path_fractions;
    //    Vector3[] line_renderer_points = new Vector3[char_fly_path_fractions];

    //    for (int i = 0; i < char_fly_path_fractions; i++)
    //    {
    //        float fraction = fraction_step * i;

    //        line_renderer_points[i] = char_fly_vertex_path.GetPoint(fraction);
    //    }

    //    line_renderer.SetPositions(line_renderer_points);
    //}

    void InstantiateAimObjects()
    {
        aim_objects = new GameObject[aim_objects_count];

        for (int i = 0; i < aim_objects_count; i++)
        {
            aim_objects[i] = Instantiate(aim_object);
        }
    }

    void AdjustObjectsByAim(VertexPath char_fly_vertex_path)
    {
        // aim_start_fraction = 0.1
        // aim_end_fraction = 0.8
        // aim_objects_count = 4

        float fraction = aim_start_fraction;
        float fraction_step = (aim_end_fraction - aim_start_fraction) / (aim_objects_count - 1);

        for (int i = 0; i < aim_objects_count; i++)
        {
            if (i > 0) fraction += fraction_step;

            Vector3 object_pos = char_fly_vertex_path.GetPoint(fraction);
            Vector3 object_dir = Vector3.Normalize(char_fly_vertex_path.GetPoint(fraction + 0.01f) - object_pos);

            aim_objects[i].transform.position = object_pos;
            aim_objects[i].transform.forward = object_dir;
        }
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
        // fly_path_fraction = 0.0f;
        moving_to_target = true;
    }

    public void StopMovingToTarget()
    {
        moving_to_target = false;
    }

    public Transform test_aim_start_pos;

    //[ContextMenu("SetUpAimPath")]
    //public void SetUpAimPath()
    //{
    //    // Debug.Log(char_transform.position);
    //    // Char position
    //    Vector3 char_pos = char_transform.position;

    //    // Get Aim Random Start Position
    //    Vector3 aim_start_pos = CustomRandomPointOnArea();
    //    //  Vector3 aim_start_pos = test_aim_start_pos.position;
    //    aim_start_pos.y = char_pos.y;

    //    // Get Current Target's Position
    //    Vector3 target_pos = curr_target.go.gameObject.GetComponent<Transform>().position;
    //    // target_pos.y = char_pos.y;
        
    //    // Calc Path Deviation
    //    Vector3 char_pos_to_target = target_pos - char_pos;
    //    Vector3 char_pos_to_aim_start = aim_start_pos - char_pos;

    //    // Calculate angle deviation
    //    float angle_deviation = Vector3.SignedAngle(char_pos_to_target, char_pos_to_aim_start, Vector3.up);
    //    float sign = angle_deviation > 0 ? -1.0f : 1.0f;
    //    angle_deviation = Mathf.Abs(angle_deviation);
    //    angle_deviation = Mathf.Clamp((angle_deviation / max_angle), min_angle / max_angle, 1.0f);
    //    angle_deviation = deviation_from_angle.Evaluate(angle_deviation) * sign;
    //    float deviation = angle_deviation * max_deviation;

    //    vertex_aim_path = GetPathFromDeviation(target_pos, aim_start_pos, deviation);

    //    SetAimAtFraction(0.0f);
    //}

    //private VertexPath GetPathFromDeviation(Vector3 target_pos, Vector3 aim_start_pos, float deviation)
    //{
    //    // Debug.Log("target_pos: " + target_pos + "aim_start_pos: " + aim_start_pos + "deviation: " + deviation);

    //    //Calculate Middle Point
    //    Vector3 middle_point = new Vector3(0.0f, 0.0f, 0.0f);
    //    Vector3 aim_start_to_target = target_pos - aim_start_pos;

    //    middle_point.x = aim_start_pos.x + (aim_start_to_target.x / 2);
    //    middle_point.y = aim_start_pos.y + (aim_start_to_target.y / 2);
    //    middle_point.z = aim_start_pos.z + (aim_start_to_target.z / 2);

    //    // Calculate Circle Center
    //    Vector3 deviation_norm = Vector3.Normalize(Vector3.Cross(Vector3.up, aim_start_to_target));
    //    Vector3 circle_center = middle_point + (deviation_norm * deviation);

    //    Debug.DrawLine(middle_point, circle_center);

    //    // Calculate Angle between points and Angle Step
    //    float sign = deviation > 0 ? 1.0f : -1.0f;
    //    float angle = Vector3.Angle(aim_start_pos - circle_center, target_pos - circle_center) + aim_walk_path_prolong_angle;
    //    float angle_step = angle / aim_path_points_num * sign;

    //    // Prepare Path Points for Creating
    //    Vector3 curr_aim_path_point = aim_start_pos - circle_center;
    //    Vector3[] path_points = new Vector3[aim_path_points_num];

    //    for (int i = 0; i < path_points.Length; i++)
    //    {
    //        //Debug.Log(i + " p: " + (curr_aim_path_point + circle_center));
    //        // Pushing new point to an array
    //        path_points[i] = curr_aim_path_point + circle_center;

    //        // Moving curr_angle each step
    //        curr_aim_path_point = Quaternion.Euler(0.0f, angle_step, 0.0f) * curr_aim_path_point;
    //    }

    //    return new VertexPath( new BezierPath(path_points) );
    //}

    //private Vector3 CustomRandomPointOnArea()
    //{
    //    // Get Aim Random Start Position
    //    Vector3 point = RandPointInPolygon.Get(aim_spawn_area_min_x, aim_spawn_area_max_x, aim_spawn_area.path.vertices);

    //    // Randomly Reflect
    //    bool should_use_reflected = (Random.value > 0.5f);

    //    if (should_use_reflected)
    //    {
    //        point = ReflectAroundCurrPlane(point);
    //    }

    //    return point;
    //}

    //private Vector3 ReflectAroundCurrPlane(Vector3 point)
    //{
    //    // Set up new reflection plane
    //    Vector3 curr_target_pos = GameObject.Find("Pillar (1)").GetComponentInChildren<Transform>().position;
    //    Vector3 curr_platform_pos = GameObject.Find("Pillar").GetComponentInChildren<Transform>().position;

    //    Vector3 from_curr_platform_to_curr_target_norm = Vector3.Normalize(curr_target_pos - curr_platform_pos);
    //    Vector3 reflection_plane_norm = Vector3.Normalize(Vector3.Cross(from_curr_platform_to_curr_target_norm, Vector3.up));
        
    //    Vector3 from_curr_platform_pos_to_probe_pos = point - curr_platform_pos;
    //    float projected_dist = Vector3.Dot(from_curr_platform_pos_to_probe_pos, reflection_plane_norm);
    //    point += 2 * projected_dist * (-reflection_plane_norm);

    //    return point;
    //}
    
    //[ContextMenu("test aim spawner")]
    //public void TestAimSpawner()
    //{
    //    aim_spawn_area_max_x = aim_spawn_area.path.bounds.center.x + aim_spawn_area.path.bounds.extents.x;
    //    aim_spawn_area_min_x = aim_spawn_area.path.bounds.center.x - aim_spawn_area.path.bounds.extents.x;

    //    for (int i = 0; i < 300; i++)
    //    {
    //        Vector3 pos = CustomRandomPointOnArea();

    //        GameObject new_orb = Instantiate(test_thing, pos, Quaternion.identity) as GameObject;
    //    }
    //}

}
