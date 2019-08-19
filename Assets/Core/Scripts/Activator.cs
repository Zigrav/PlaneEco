using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    private List<GameObject> children;

    // Start is called before the first frame update
    void Start()
    {
        CollectChildren();
    }

    private void CollectChildren()
    {
        children = new List<GameObject>();

        Transform[] children_transforms = GetComponentsInChildren<Transform>(true);
        foreach (Transform child_transform in children_transforms)
        {
            if(child_transform.gameObject != gameObject)
            {
                children.Add(child_transform.gameObject);
            }
        }
    }

    public void ActivateChildren()
    {
        for (int i = 0; i < children.Count; i++)
        {
            GameObject child = children[i];

            child.SetActive(true);
        }
    }

    public void DeactivateChildren()
    {
        for (int i = 0; i < children.Count; i++)
        {
            GameObject child = children[i];

            child.SetActive(false);
        }
    }
}
