using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshUpdater : MonoBehaviour
{
    [SerializeField]
    private PlanesRefHolder planes_ref_holder = null;

    [SerializeField]
    private SODict curr_char = null;

    [ContextMenu("UpdateMesh")]
    public void UpdateMesh()
    {
        SODict curr_char_info = GetSingleElem.Get(curr_char) as SODict;

        GameObject char_model = planes_ref_holder.GetCharModelByRef(curr_char_info);

        MeshRenderer char_model_mesh_renderer = char_model.GetComponentInChildren<MeshRenderer>();
        MeshFilter char_model_mesh_filter = char_model.GetComponentInChildren<MeshFilter>();

        planes_ref_holder.CopyCharModel(transform.gameObject, char_model_mesh_renderer, char_model_mesh_filter);
    }


}
