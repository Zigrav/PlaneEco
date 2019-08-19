using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UploadInfrastructure : MonoBehaviour
{
    public GOVariable platform_collider_govar;

    public List<GameObject> Upload()
    {
        if (platform_collider_govar.go == null)
        {
            Debug.LogError("Curr Platform Collider Is Not Defined Yet!");
            return null;
        }

        GameObject platform_collider = platform_collider_govar.go;
        PolygonCollider2D platform_polygon_collider = platform_collider.GetComponentInChildren<PolygonCollider2D>();
        EdgeCollider2D platform_edge_collider = platform_collider.GetComponentInChildren<EdgeCollider2D>();

        List<Collider2D> infrastructure_colliders2D = new List<Collider2D>(GetComponentsInChildren<Collider2D>());

        List<GameObject> infrastructure = new List<GameObject>();
        for (int i = 0; i < infrastructure_colliders2D.Count; i++)
        {
            Collider2D collider2D = infrastructure_colliders2D[i];

            int state = Colliders2DState(collider2D, platform_polygon_collider, platform_edge_collider);

            if (state == 0)
            {
                // Create Infrastructure From Placeholder
                InfrastructureData infrastructure_data = collider2D.GetComponent<InfrastructureData>();

                GameObject infrastructure_obj = Instantiate(infrastructure_data.prefab, collider2D.transform.position, Quaternion.identity);
                Transform infrastructure_transform = infrastructure_obj.transform;

                infrastructure_transform.transform.RotateAround(infrastructure_transform.position, new Vector3(1.0f, 0.0f, 0.0f), -90.0f);
                infrastructure_transform.transform.RotateAround(infrastructure_transform.position, infrastructure_transform.up, collider2D.transform.rotation.eulerAngles.z);

                infrastructure.Add(infrastructure_obj);
            }
        }

        // in world coordinates
        return infrastructure;
    }

    public int Colliders2DState(Collider2D inner_collider, Collider2D outer_collider, Collider2D border_collider)
    {
        bool is_touching = inner_collider.IsTouching(outer_collider);
        bool is_touching_border = inner_collider.IsTouching(border_collider);

        if(is_touching && !is_touching_border)
        {
            // Completely Inside
            return 0;
        }
        else if (is_touching && is_touching_border)
        {
            // Inside While Colliding With Border
            return 1;
        }
        else if (!is_touching && is_touching_border)
        {
            // Touching Only Border
            return 2;
        }
        else if (!is_touching && !is_touching_border)
        {
            // Completely Outside
            return 3;
        }

        return -1;
    }

}



//PolygonCollider2D hex_collider = hex.GetComponent<PolygonCollider2D>();
//EdgeCollider2D edge_collider = edge.GetComponent<EdgeCollider2D>();
//PolygonCollider2D complex_collider = complex.GetComponent<PolygonCollider2D>();

//Collider2D[] results = new Collider2D[5];

//hex_collider.OverlapCollider(new ContactFilter2D(), results);

//        Debug.Log("hex");
//        for (int i = 0; i<results.Length; i++)
//        {
//            Debug.Log(results[i]);
//        }
//        results = new Collider2D[10];

//        edge_collider.OverlapCollider(new ContactFilter2D(), results);

//        Debug.Log("edge");
//        for (int i = 0; i<results.Length; i++)
//        {
//            Debug.Log(results[i]);
//        }
//        results = new Collider2D[10];

//        complex_collider.OverlapCollider(new ContactFilter2D(), results);

//        Debug.Log("complex");
//        for (int i = 0; i<results.Length; i++)
//        {
//            Debug.Log(results[i]);
//        }
//        results = new Collider2D[10];