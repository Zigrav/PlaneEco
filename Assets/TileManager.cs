using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
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
        pillars.items[1].GetComponentInChildren<Target>().enabled = true;
    }
}
