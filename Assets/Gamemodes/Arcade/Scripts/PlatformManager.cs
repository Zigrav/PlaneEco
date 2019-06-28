using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public PillarsRuntimeSet pillars;
    public TargetVariable curr_target;

    // Start is called before the first frame update
    void Start()
    {
        SetTarget();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetTarget()
    {
        GameObject.Find("Pillar (1)").GetComponentInChildren<Target>().enabled = true;
    }
}
