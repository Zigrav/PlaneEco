using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGroupManager : MonoBehaviour
{
    public Cinemachine.CinemachineTargetGroup target_group;

    public GOVariable target;
    public GOVariable heliport;

    public void UpdateTargetGroup()
    {
        target_group.m_Targets[0].target = heliport.go.transform;
        target_group.m_Targets[1].target = target.go.transform;
    }
}
