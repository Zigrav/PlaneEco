using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.Events;

public class PathGenerator : MonoBehaviour
{
    public PathCreator path;

    [HideInInspector]
    public VertexPath vertex_path;

    public PathAnimator path_anim;

    public bool is_path_closed = false;

    [HideInInspector]
    public bool corrupted_path = false;

    public UnityEvent OnStart = new UnityEvent();
    public UnityEvent OnLateUpdate = new UnityEvent();

    private List<Anchor> anchors;
    private bool initiated = false;

    [SerializeField]
    private float max_angle_error = 0.3f;

    [SerializeField]
    private float min_vertex_dist = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        OnStart.Invoke();
    }

    private void LateUpdate()
    {
        OnLateUpdate.Invoke();
    }

    public void StartPathGenerator()
    {
        AddAnchorsToPath();
        UpdateAnchors();
    }

    public void UpdateAnchors()
    {
        bool corrupted_data = false;


        //if(this.name == "CameraFlyPath")
        //{
        //    Debug.Log("CameraFlyPath Updating Anchors");
        //}
        //else if (this.name == "FocusPointPath")
        //{
        //    Debug.Log("FocusPointPath Updating Anchors");
        //}

        // Debug.Log("anchor points 3: " + path.bezierPath.NumAnchorPoints);

        for (int i = 0, a = 0; i < anchors.Count; i++, a += 3)
        {
            anchors[i].UpdateAnchor();

            if (anchors[i].corrupted_data)
            {
                Debug.LogError("The " + i + " anchor is corrupted! " + gameObject.name);
                corrupted_data = true;
                continue;
            }

            // Debug.Log(anchors[i].anchor_pos);

            path.bezierPath.MovePoint(a, anchors[i].anchor_pos);
        }

        // Debug.Log("anchor points 4: " + path.bezierPath.NumAnchorPoints);

        // Vertex Path is updated only when path.path.vertices is called through ContextMenu in Editor
        // So we create new VertexPath Each time to be keep it up to date :/
        vertex_path = new VertexPath(path.bezierPath, max_angle_error, min_vertex_dist);

        if(vertex_path.vertices.Length == 1)
        {
            Debug.LogError("The path on " + gameObject.name + " has only 1 vertex!");
            corrupted_data = true;
        }

        corrupted_path = corrupted_data;

        if (path_anim != null)
        {
            path_anim.corrupted_path = corrupted_path;
        }
    }

    [ContextMenu("VerticesPos")]
    public void VerticesPos()
    {
        // Debug.Log("PathGenerator");

        Vector3[] vertices = GetPathVertices();

        for (int i = 0; i < vertices.Length; i++)
        {
            Debug.Log(vertices[i]);
        }
    }

    public Vector3[] GetPathVertices()
    {
        // Debug.Log("PathGenerator getting vertices");
        for (int i = 0; i < 4; i++)
        {
            Debug.Log(vertex_path.vertices[i]);
        }

        return vertex_path.vertices;
    }

    public void AddAnchorsToPath()
    {
        // Debug.Log("Called: " + gameObject.name); //
        if (initiated) return;

        // Debug.Log("Entered: " + gameObject.name);

        bool is_closed = is_path_closed;
        path.bezierPath.IsClosed = false;

        anchors = new List<Anchor>(GetComponentsInChildren<Anchor>());

        // Debug.Log("achors_count 0: " + anchors.Count);
        for (int i = 0; i < anchors.Count; i++)
        {
            anchors[i].InitAnchor();
        }

        // Debug.Log("achors_count 1: " + anchors.Count);
        for (int i = 2; i < anchors.Count; i++)
        {
            path.bezierPath.AddSegmentToEnd(anchors[i].anchor_pos);
        }

        // Debug.Log("anchor points: " + path.bezierPath.NumAnchorPoints);

        path.bezierPath.IsClosed = is_closed;
        initiated = true;
    }

}
