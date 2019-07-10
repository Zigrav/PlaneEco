using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartVFX : MonoBehaviour
{
    public void DrStartVFX()
    {
        GameObject parent = gameObject.transform.parent.gameObject;

        if (parent.name.Contains("platform"))
        {
            PreparePlatform script = gameObject.GetComponentInParent<PreparePlatform>();
            script.StartVFX();
        }
        else
        {
            Debug.Log("Parent Object Of" + gameObject.name + "is not approprate for it");
        }
    }
}
