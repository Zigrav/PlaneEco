﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecRes : MonoBehaviour
{
    [SerializeField]
    private GOVariable resurrecter = null;

    public void Res()
    {
        // Debug.Log("exec res");

        // Execute Res method of the Resurrecter object in the scene
        resurrecter.go.GetComponent<Resurrect>().Res();
    }

    public void PrepareRes()
    {

        // Debug.Log("exec prepare res");
        // Execute MoveCamToStart method of the Resurrecter object in the scene
        resurrecter.go.GetComponent<Resurrect>().PrepareRes();
    }

}
