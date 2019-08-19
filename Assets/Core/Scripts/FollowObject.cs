using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FollowObject : MonoBehaviour
{
    public UnityEvent on_start;
    public UnityEvent on_fixed_update;

    private Vector3 past_follow_object_pos;

    public void AssignPast(Transform past_transform)
    {
        past_follow_object_pos = past_transform.position;
    }

    public void MoveTo(Transform follow_object_trans)
    {
        transform.position = follow_object_trans.position;
        AssignPast(follow_object_trans);
    }

    public void FollowByDelta(Transform follow_object_trans)
    {
        transform.position += (follow_object_trans.position - past_follow_object_pos);
        AssignPast(follow_object_trans);
    }
}
