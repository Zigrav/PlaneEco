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
}
