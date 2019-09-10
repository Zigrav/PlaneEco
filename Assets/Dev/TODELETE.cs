using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PathCreation;
using UnityEngine.UI;
using UnityEngine.Events;

public class TODELETE : MonoBehaviour
{
    [SerializeField]
    private PathCreator path = null;
    [SerializeField]
    private VertexPath vertex_path = null;

    public void CreateNewVertexPath()
    {
        vertex_path = new VertexPath(path.bezierPath);
    }

    public void Update()
    {
        //  Debug.Log("SDF");
        CreateNewVertexPath();
    }
}