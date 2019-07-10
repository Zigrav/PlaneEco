using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

public class PrepareMap : MonoBehaviour
{
    public GameObject placeholder_prefab;

    [ContextMenu("Prepare")]
    public void Prepare()
    {
        List<Collider2D> colliders = new List<Collider2D>(GetComponentsInChildren<Collider2D>());
        GOBounds go_bounds = GetComponent<GOBounds>();
        Vector3 pivot = new Vector3(0.0f, 0.0f, 0.0f);

        // Get Map's Bounds
        go_bounds.SetBounds();

        // Rotate its center around pivot on Z axis
        go_bounds.bounds.center = RotatePointAroundPivot(go_bounds.bounds.center, pivot, new Vector3(-90.0f, 0.0f, 0.0f));

        // "Rotate" BB's Size by 90 along Z axis
        go_bounds.bounds.size = new Vector3( go_bounds.bounds.size.x, go_bounds.bounds.size.z, go_bounds.bounds.size.y );

        for (int i = 0; i < colliders.Count; i++)
        {
            GameObject map_go = colliders[i].gameObject;

            // Create placeholder and rotate it to XY Plane
            GameObject placeholder = Instantiate(placeholder_prefab, map_go.transform.position, map_go.transform.rotation, gameObject.transform);

            // Rotate placeholder by 90 along Z axis
            placeholder.transform.RotateAround(pivot, new Vector3(1.0f, 0.0f, 0.0f), -90.0f);

            // Placeholders hold 2D Colliders and we need to modify their rotation correctly
            placeholder.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, map_go.transform.rotation.eulerAngles.y));

            // During Rotation The Position on Z is messed up for some reason
            placeholder.transform.position = new Vector3(placeholder.transform.position.x, placeholder.transform.position.y, 0.0f);

            // Assign What Prefab This Placeholder Holds //
            string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(map_go);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            placeholder.GetComponent<InfrastructureData>().prefab = prefab;

            // Copy Collider2D Component From map_go //
            Collider2D compo = map_go.GetComponent<Collider2D>();

            // Get The Component Of The Appropriate Derived Type ( derived from Collider2D )
            Component placeholder_collider2D = placeholder.AddComponent( map_go.GetComponent<Collider2D>().GetType() );

            // Copy content from map_go Collider2D to placeholder_collider2D
            placeholder_collider2D.GetCopyOf( map_go.GetComponent<Collider2D>() );
        }

    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }

}