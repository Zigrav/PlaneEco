using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : MonoBehaviour
{
    public bool is_average;

    public List<Object> average_list;
    public Object point;

    public Vector3 offset;

    public bool draw_anchor;

    [HideInInspector]
    public bool corrupted_data = false;

    private List<Transform> transforms;
    public Vector3 anchor_pos;

    void LateUpdate()
    {
        UpdateAnchor();
    }

    public void InitAnchor()
    {
        InitTransforms();
        UpdateAnchor();
    }

    public void UpdateAnchor()
    {
        // Debug.Log("called UpdateAnchor");
        UpdateTransforms();
        // Debug.Log("after UpdateTransforms: " + corrupted_data);
        anchor_pos = GetBenchPoint() + offset;
    }

    private void InitTransforms()
    {
        transforms = new List<Transform>();

        if (is_average)
        {
            for (int i = 0; i < average_list.Count; i++)
            {
                transforms.Add(null);
            }
        }
        else
        {
            transforms.Add(null);
        }
    }

    private void UpdateTransforms()
    {
        corrupted_data = false;

        if (is_average)
        {
            bool is_go_var_defined = false; bool is_it_go_var = false;
            for (int i = 0; i < average_list.Count; i++)
            {
                transforms[i] = ParseForTransform.Parse(average_list[i], ref is_it_go_var, ref is_go_var_defined);

                if (is_it_go_var && !is_go_var_defined)
                {
                    // Debug.Log("object: " + i + " is corrupted");
                    corrupted_data = true;
                    return;
                }
            }
        }
        else
        {
            bool is_go_var_defined = false; bool is_it_go_var = false;
            transforms[0] = ParseForTransform.Parse(point, ref is_it_go_var, ref is_go_var_defined);
            
            // Just delete this line (and write an if statement about only 1 vertex in vertex_path)
            // Vector3 point_pos = (point as GameObject).transform.position;
            // Debug.Log("in UpdateTRansforms: " + transforms[0].position + "through gameobject: " + point_pos);

            if (is_it_go_var && !is_go_var_defined)
            {
                corrupted_data = true;
                return;
            }
        }
    }

    private Vector3 GetBenchPoint()
    {
        // Debug.Log("is corrupted: " + corrupted_data);
        if (corrupted_data) return new Vector3(0.0f, 0.0f, 0.0f);

        Vector3 sum_vect = new Vector3();

        for (int i = 0; i < transforms.Count; i++)
        {
            // Debug.Log("transforms: " + transforms[i].position);
            sum_vect += transforms[i].position;
        }

        return sum_vect / transforms.Count;
    }

    void OnDrawGizmosSelected()
    {
        if (!draw_anchor) return;
        
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(anchor_pos, 0.3f);
    }

}
