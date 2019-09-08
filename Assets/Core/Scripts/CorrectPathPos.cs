using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

[ExecuteInEditMode]
// BANNED BANNED BANNED
public class CorrectPathPos : MonoBehaviour
{
    public PathCreator path_creator;
    public float path_y = 0.0f;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBs");
        path_creator = GetComponent<PathCreator>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < path_creator.path.NumVertices; i++)
        {
            // Set Y to specific value
            path_creator.path.vertices[i].y = path_y;
        }

        for (int i = 0; i < path_creator.bezierPath.NumPoints; i++)
        {
            Vector3 point = path_creator.bezierPath.GetPoint(i);

            point.y = path_y;

            path_creator.bezierPath.MovePoint(i, point);
        }
    }

    //private void MoveByParent()
    //{
    //    parent_transform_pos = transform.parent.position;

    //    for (int i = 0; i < path_creator.path.NumVertices; i++)
    //    {
    //        Debug.Log("parent_pos!!!!!!!!!!!!!!!!!!!!!: " + parent_transform_pos);

    //        // Make path comply with parent transform thing
    //        Debug.Log("vertex pos: " + path_creator.path.vertices[i]);

    //        path_creator.path.vertices[i].x = parent_transform_pos.x + path_creator.path.vertices[i].x;
    //        path_creator.path.vertices[i].y = parent_transform_pos.y + path_creator.path.vertices[i].y;
    //        path_creator.path.vertices[i].z = parent_transform_pos.z + path_creator.path.vertices[i].z;

    //        Debug.Log("vertex pos after: " + path_creator.path.vertices[i]);
    //    }

    //    for (int i = 0; i < path_creator.bezierPath.NumPoints; i++)
    //    {
    //        Vector3 point = path_creator.bezierPath.GetPoint(i);

    //        point.x = parent_transform_pos.x + point.x;
    //        point.y = parent_transform_pos.y + point.y;
    //        point.z = parent_transform_pos.z + point.z;

    //        path_creator.bezierPath.MovePoint(i, point);
    //    }
    //}
}
