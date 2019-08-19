using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GOBounds : MonoBehaviour
{
    public Bounds bounds;
    public bool show_bounds = false;

    public UnityEvent OnStart;

    private void Start()
    {
        OnStart.Invoke();
    }

    [ContextMenu("SetBounds")]
    public void SetBounds()
    {
        List<MeshRenderer> meshes = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>(true));
        List<GOBounds> gos_bounds = new List<GOBounds>(GetComponentsInChildren<GOBounds>());
        // List<Collider2D> colliders2d_bounds = new List<Collider2D>(GetComponentsInChildren<Collider2D>());

        bounds = new Bounds(transform.position, new Vector3(0.0f, 0.0f, 0.0f));

        // Debug.Log("before evertything: " + bounds);

        // Debug.Log("m: " + meshes.Count);
        // Debug.Log("gos bounds: " + gos_bounds.Count);
        // Debug.Log("colliders2d: " + colliders2d_bounds.Count);

        for (int i = 0; i < meshes.Count; i++)
        {
            // Debug.Log("mesh_bounds: " + meshes[i].bounds);
            bounds.Encapsulate(meshes[i].bounds);
        }

        for (int i = 0; i < gos_bounds.Count; i++)
        {
            // gos_bounds[i].SetBounds();
            // Debug.Log("gos_bounds: " + gos_bounds[i].bounds);

            bounds.Encapsulate(gos_bounds[i].bounds);
            // Debug.Log("after gos_bounds: " + bounds);
        }
    }

    public void SetBounds(Bounds new_bounds)
    {
        // Debug.Log("goes");
        bounds = new_bounds;
    }

    private void OnDrawGizmos()
    {
        if (!show_bounds) return;

        Gizmos.DrawWireCube(bounds.center, bounds.size);
        Gizmos.DrawWireSphere(bounds.center, 0.3f);
    }
}
