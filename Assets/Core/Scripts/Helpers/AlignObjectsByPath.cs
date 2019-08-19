using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathCreation;

public class AlignObjectsByPath : MonoBehaviour
{
    public GOList objects_golist;
    public PathGenerator path;

    public float start_fraction = 0.1f;
    public float end_fraction = 0.8f;

    public UnityEvent on_start;
    public UnityEvent on_fixed_update;

    private void Start()
    {
        on_start.Invoke();
    }

    private void FixedUpdate()
    {
        on_fixed_update.Invoke();
    }

    public void Align()
    {
        VertexPath vertex_path = path.vertex_path;

        int objects_count = objects_golist.list.Count;

        float fraction = start_fraction;
        float fraction_step = (end_fraction - start_fraction) / (objects_count - 1);

        for (int i = 0; i < objects_count; i++)
        {
            if (i > 0) fraction += fraction_step;

            Vector3 object_pos = vertex_path.GetPoint(fraction);
            Vector3 object_dir = Vector3.Normalize(vertex_path.GetPoint(fraction + 0.01f) - object_pos);

            objects_golist.Get(i).transform.position = object_pos;
            objects_golist.Get(i).transform.forward = object_dir;
        }
    }
}
