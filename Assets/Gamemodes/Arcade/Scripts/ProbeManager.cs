using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbeManager : MonoBehaviour
{
    public Probe[] probes;
    public Probe[] reflected_probes;

    [System.Serializable]
    public class Probe
    {
        public Probe(Vector3 pos, float radius)
        {
            this.pos = pos;
            this.radius = radius;
        }

        public Vector3 pos;
        public float radius;
    }

    private void Awake()
    {
        // Debug.Log("SetUpReflectionProbes");

        SetUpReflectionProbes();
    }

    public void SetUpReflectionProbes()
    {
        reflected_probes = new Probe[probes.Length];

        for (int i = 0; i < probes.Length; i++)
        {
            reflected_probes[i] = new Probe(probes[i].pos, probes[i].radius);
        }

        UpdateProbes();
    }

    // Move Probes to a New Tile
    public void UpdateProbes()
    {
        // Debug.Log("UpdateProbes");
        
        // Move probes to curr target tile

        // Set up new reflection plane
        Vector3 curr_target_pos = GameObject.Find("Pillar (1)").GetComponentInChildren<Transform>().position;
        Vector3 curr_platform_pos = GameObject.Find("Pillar").GetComponentInChildren<Transform>().position;

        Vector3 from_curr_platform_to_curr_target_norm = Vector3.Normalize(curr_target_pos - curr_platform_pos);
        Vector3 reflection_plane_norm = Vector3.Normalize(Vector3.Cross(from_curr_platform_to_curr_target_norm, Vector3.up));
        // Debug.DrawLine(curr_target_pos, curr_target_pos + reflection_plane_norm);

        // Move reflected probes
        for (int i = 0; i < probes.Length; i++)
        {
            Vector3 probe_pos = probes[i].pos;

            Vector3 from_curr_platform_pos_to_probe_pos = probe_pos - curr_platform_pos;
            float projected_dist = Vector3.Dot(from_curr_platform_pos_to_probe_pos, reflection_plane_norm);
            probe_pos += 2 * projected_dist * (-reflection_plane_norm);

            reflected_probes[i].pos = probe_pos;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.white;

        for (int i = 0; i < probes.Length; i++)
        {
            Probe probe = probes[i];

            Gizmos.DrawWireSphere(probe.pos, probe.radius);
        }

        for (int i = 0; i < reflected_probes.Length; i++)
        {
            Probe reflected_probe = reflected_probes[i];

            Gizmos.DrawWireSphere(reflected_probe.pos, reflected_probe.radius);
        }
    }
}
