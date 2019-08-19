using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfrastructureData : MonoBehaviour
{
    public GameObject prefab;

    [ContextMenu("CreateClone")]
    public void CreateClone()
    {
        Debug.Log("creating clone");
        Instantiate(prefab);
    }

}
