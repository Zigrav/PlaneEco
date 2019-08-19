using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbeManager : MonoBehaviour
{
    public GOList probes;
    public GOList reflected_probes;

    public GOVariable curr_target;
    public GOVariable curr_platform;
    public GOVariable curr_heliport;

    public bool reflect_probes_z;

    private void Awake()
    {
        SetUpProbes();
        SetUpReflectionProbes();
    }

    public void SetUpProbes()
    {
        foreach (Transform child_transform in GetComponentsInChildren<Transform>())
        {
            GameObject child = child_transform.gameObject;

            if (child == gameObject) continue;
            if (!child.activeSelf) continue;

            probes.Add(child);
        }
    }

    public void SetUpReflectionProbes()
    {
        if (reflect_probes_z)
        {
            for (int i = 0; i < probes.list.Count; i++)
            {
                reflected_probes.Add(Instantiate(probes.list[i], transform));

                Vector3 new_pos = reflected_probes.list[i].transform.localPosition;
                new_pos.x *= -1;

                reflected_probes.list[i].transform.localPosition = new_pos;
            }
        }
    }

    public void TestFunc(GameObject obj)
    {
        Debug.Log(obj);
    }

    //public void MoveToCurrPlatform()
    //{
    //    MoveProbes(curr_heliport.go.transform.position);

    //    RotateProbes(Quaternion.LookRotation(Vector3.Normalize(curr_target.go.transform.position - curr_heliport.go.transform.position)));
    //}

    //public void MoveProbes(Vector3 new_pos)
    //{
    //    transform.position = new_pos;
    //}

    //public void RotateProbes(Quaternion rotation)
    //{
    //    transform.rotation = rotation;
    //}

    // TO DELETE
    // Move Probes to a New Tile
    //public void UpdateProbes()
    //{
    //    // Debug.Log("UpdateProbes");
        
    //    // Move probes to curr target tile

    //    // Set up new reflection plane
    //    Vector3 curr_target_pos = GameObject.Find("Pillar (1)").GetComponentInChildren<Transform>().position;
    //    Vector3 curr_platform_pos = GameObject.Find("Pillar").GetComponentInChildren<Transform>().position;

    //    Vector3 from_curr_platform_to_curr_target_norm = Vector3.Normalize(curr_target_pos - curr_platform_pos);
    //    Vector3 reflection_plane_norm = Vector3.Normalize(Vector3.Cross(from_curr_platform_to_curr_target_norm, Vector3.up));
    //    // Debug.DrawLine(curr_target_pos, curr_target_pos + reflection_plane_norm);

    //    // Move reflected probes
    //    for (int i = 0; i < probes.Length; i++)
    //    {
    //        Vector3 probe_pos = probes[i].pos;

    //        Vector3 from_curr_platform_pos_to_probe_pos = probe_pos - curr_platform_pos;
    //        float projected_dist = Vector3.Dot(from_curr_platform_pos_to_probe_pos, reflection_plane_norm);
    //        probe_pos += 2 * projected_dist * (-reflection_plane_norm);

    //        reflected_probes[i].pos = probe_pos;
    //    }
    //}

    // TO DELETE
    //void OnDrawGizmosSelected()
    //{
    //    // Display the explosion radius when selected
    //    Gizmos.color = Color.white;

    //    for (int i = 0; i < probes.Length; i++)
    //    {
    //        Probe probe = probes[i];

    //        Gizmos.DrawWireSphere(probe.pos, probe.radius);
    //    }

    //    for (int i = 0; i < reflected_probes.Length; i++)
    //    {
    //        Probe reflected_probe = reflected_probes[i];

    //        Gizmos.DrawWireSphere(reflected_probe.pos, reflected_probe.radius);
    //    }
    //}
}
