using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbeProperties : MonoBehaviour
{
    private List<GameObject> probes;
    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        probes = GetAllSiblings();
    }

    private List<GameObject> GetAllSiblings()
    {
        if (transform.parent == null) return null;

        List<GameObject> list = new List<GameObject>();

        GameObject parent = transform.parent.gameObject;
        Transform[] siblings_transforms = parent.GetComponentsInChildren<Transform>();

        foreach (Transform sibling_transform in siblings_transforms)
        {
            GameObject sibling = sibling_transform.gameObject;

            if (sibling == parent) continue;

            list.Add(sibling_transform.gameObject);
        }

        return list;
    }

    void OnDrawGizmosSelected()
    {
        probes = GetAllSiblings();

        if (probes == null) return;

        // Display the explosion radius when selected
        Gizmos.color = Color.white;

        for (int i = 0; i < probes.Count; i++)
        {
            GameObject probe = probes[i];

            Gizmos.DrawWireSphere(probe.transform.position, probe.GetComponent<ProbeProperties>().radius);
        }
    }
}
