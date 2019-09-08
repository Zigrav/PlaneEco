using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class PlanesRefHolder : SerializedMonoBehaviour
{
    [SerializeField]
    private Dictionary<SODict, GameObject> planes_refs = null;

    public GameObject GetCharModelByRef(SODict char_info)
    {
        if (planes_refs.ContainsKey(char_info))
        {
            return planes_refs[char_info];
        }

        throw new System.Exception(char_info.name + " char info does not exist in planes_refs dctonary!");
    }

    public void CopyCharModel(GameObject char_obj, MeshRenderer mesh_renderer, MeshFilter mesh_filter)
    {
        MeshFilter origin_mesh_filter = char_obj.GetComponent<MeshFilter>();
        if (origin_mesh_filter == null)
        {
            throw new System.Exception(char_obj.name + " does not have a MeshFilter Component!");
        }

        MeshRenderer origin_mesh_renderer = char_obj.GetComponent<MeshRenderer>();
        if (origin_mesh_renderer == null)
        {
            throw new System.Exception(char_obj.name + " does not have a MeshRenderer Component!");
        }
        
        // Copy mesh filter mesh
        origin_mesh_filter.mesh = mesh_filter.sharedMesh;

        // Copy Materials to temporary array
        Material[] copied_materials = new Material[mesh_renderer.sharedMaterials.Length];
        for (int i = 0; i < mesh_renderer.sharedMaterials.Length; i++)
        {
            copied_materials[i] = mesh_renderer.sharedMaterials[i];
        }

        origin_mesh_renderer.materials = copied_materials;
    }
}
