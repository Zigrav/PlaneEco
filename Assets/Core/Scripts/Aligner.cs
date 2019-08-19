using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class Aligner : MonoBehaviour
{
    //// Start is called before the first frame update
    //void Start()
    //{
    //    Debug.Log("Number: " + (4.0f / 0.866025f));
    //    Debug.Log("Number: " + Mathf.Round(4.0f / 0.866025f) );
    //    Debug.Log("Number: " + Mathf.Round(4.0f / 0.866025f) * 0.866025f);
    //}

    [ContextMenu("Align")]
    void Align()
    {
        foreach (Transform child in transform)
        {
            //Debug.Log(child.localPosition);
            float aligned_x = Mathf.Round(child.localPosition.x / 0.8660254f) * 0.8660254f;
            float aligned_z = Mathf.Round(child.localPosition.z / 0.5f) * 0.5f;
            //Debug.Log(aligned_z);
            child.localPosition = new Vector3(aligned_x, child.localPosition.y, aligned_z);
            //Debug.Log(child.localPosition);
        }
    }
}
