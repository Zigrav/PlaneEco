using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

static class Extensions
{
    public static bool IsWithin(this float value, float one_float, float other_float)
    {
        float minimum = Mathf.Min(one_float, other_float);
        float maximum = Mathf.Max(one_float, other_float);

        return value >= minimum && value <= maximum;
    }

    public static T GetCopyOf<T>(this Component to_component, T from_component) where T : Component
    {
        Type type = to_component.GetType();
        if (type != from_component.GetType())
        {
            Debug.LogError("Typ mis-match");
            return null; // type mis-match
        }

        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(to_component, pinfo.GetValue(from_component, null), null);
                }
                catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }

        FieldInfo[] finfos = type.GetFields(flags);

        foreach (var finfo in finfos)
        {
            finfo.SetValue(to_component, finfo.GetValue(from_component));
        }

        return to_component as T;
    }
}

public class RandPointInPolygon
{
    public static Vector3 Get(float min_x, float max_x, Vector3[] polygon_vertices)
    {
        // Set orb position
        float rand_x = UnityEngine.Random.Range(min_x, max_x);

        // Get intersection points
        List<Vector3> points = GetLineAndPolygonIntersectionPoints(rand_x, polygon_vertices);

        // Generate random z coordinate between intersection points
        float rand_z = UnityEngine.Random.Range(points[0].z, points[1].z);
        float point_y = (points[0].y + points[1].y) / 2;

        // Return Random Point
        return new Vector3(rand_x, point_y, rand_z);
    }

    private static List<Vector3> GetLineAndPolygonIntersectionPoints(float rand_x, Vector3[] polygon_vertices)
    {
        List<Vector3> points = new List<Vector3>();
        int l = polygon_vertices.Length;

        for (int first_index = 0; first_index < l; first_index++)
        {
            int second_index = first_index == l - 1 ? 0 : first_index + 1;

            // Special case when line is parallel to x axis
            if (rand_x == polygon_vertices[first_index].x && rand_x == polygon_vertices[second_index].x) continue;

            if (rand_x.IsWithin(polygon_vertices[first_index].x, polygon_vertices[second_index].x))
            {
                points.Add(GetPointFromX(rand_x, polygon_vertices[first_index], polygon_vertices[second_index]));
            }

            // No more than two intersection points
            if (points.Count == 2) break;
        }

        return points;
    }

    private static Vector3 GetPointFromX(float x, Vector3 from, Vector3 to)
    {
        Vector3 vect = to - from;
        float ratio_x = (x - from.x) / vect.x;
        vect *= ratio_x;

        Vector3 point = from + vect;

        return point;
    }
}