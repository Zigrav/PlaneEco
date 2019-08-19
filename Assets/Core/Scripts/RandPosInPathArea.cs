using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class RandPosInPathArea : MonoBehaviour
{
    public Object g_point;
    public PathGenerator path_area_generator;

    public Object reflect_point_0;
    public Object reflect_point_1;

    private Transform g_point_transform;
    private Transform reflect_point_0_transform;
    private Transform reflect_point_1_transform;

    private float path_area_max_x;
    private float path_area_min_x;

    [HideInInspector]
    public bool corrupted_data = false;

    private bool CollectData()
    {
        corrupted_data = false;

        bool is_go_var_defined = false; bool is_it_go_var = false;
        g_point_transform = ParseForTransform.Parse(g_point, ref is_it_go_var, ref is_go_var_defined);
        if (is_it_go_var && !is_go_var_defined) corrupted_data = true;

        is_go_var_defined = false; is_it_go_var = false;
        reflect_point_0_transform = ParseForTransform.Parse(reflect_point_0, ref is_it_go_var, ref is_go_var_defined);
        if (is_it_go_var && !is_go_var_defined) corrupted_data = true;

        is_go_var_defined = false; is_it_go_var = false;
        reflect_point_1_transform = ParseForTransform.Parse(reflect_point_1, ref is_it_go_var, ref is_go_var_defined);
        if (is_it_go_var && !is_go_var_defined) corrupted_data = true;

        return corrupted_data;
    }

    public void SetRandPos()
    {
        if (CollectData())
        {
            Debug.Log("RandPosInPathArea: " + corrupted_data);
            return;
        }

        Bounds bounds = path_area_generator.vertex_path.bounds;
        Vector3[] vertices = path_area_generator.vertex_path.vertices;

        // Update Min and Max Values of the Path
        path_area_min_x = bounds.center.x - bounds.extents.x;
        path_area_max_x = bounds.center.x + bounds.extents.x;

        //Debug.Log("min: " + path_area_min_x + " max: " + path_area_max_x);
        //Debug.Log("gameobject: " + gameObject);

        //for (int i = 0; i < 3; i++)
        //{
        //    Debug.Log(vertices[i]);
        //}

        g_point_transform.position = RandPointInPolygon.Get(path_area_min_x, path_area_max_x, vertices);

        // Randomly Reflect
        bool should_use_reflected = (Random.value > 0.5f);
        if (should_use_reflected) g_point_transform.position = ReflectAroundCurrPlane(g_point_transform.position);
    }


    [ContextMenu("VerticesPos")]
    public void VerticesPos()
    {
        Vector3[] vertices = path_area_generator.vertex_path.vertices;

        Debug.Log("got path vertices");

        for (int i = 0; i < vertices.Length; i++)
        {
            Debug.Log(vertices[i]);
        }
    }

    private Vector3 ReflectAroundCurrPlane(Vector3 point)
    {
        // Set up new reflection plane
        Vector3 reflect_point_0 = reflect_point_0_transform.position;
        Vector3 reflect_point_1 = reflect_point_1_transform.position;

        Vector3 from_first_point_to_second_point = Vector3.Normalize(reflect_point_1 - reflect_point_0);
        Vector3 reflection_plane_norm = Vector3.Normalize(Vector3.Cross(from_first_point_to_second_point, Vector3.up));

        // Reflect Around Plane
        Vector3 from_reflect_point_0_to_point = point - reflect_point_0;
        float projected_dist = Vector3.Dot(from_reflect_point_0_to_point, reflection_plane_norm);
        point += 2 * projected_dist * (-reflection_plane_norm);

        return point;
    }
}
