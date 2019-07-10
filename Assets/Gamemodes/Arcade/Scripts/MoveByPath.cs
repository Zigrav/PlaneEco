using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PathCreation;

public class MoveByPath : MonoBehaviour
{
    public PathAnimator path_animator;

    public Rigidbody rigid_body;

    public UnityEvent OnFixedUpdate;

    public float rotation_speed = 1.0f;

    private void FixedUpdate()
    {
        OnFixedUpdate.Invoke();
    }

    public void SetToStartOfPathAnim()
    {
        if (!path_animator.corrupted_path)
        {
            // Debug.Log(path_animator.calc_position);
            MoveToPoint(path_animator.calc_position);
            // Debug.Log(path_animator.path_forward);
            Rotate(path_animator.path_forward);
        }
    }

    public void MoveByPathAnimator()
    {
        if (!path_animator.corrupted_path && path_animator.is_anim_running)
        {
            MoveToPoint(path_animator.calc_position);
        }
    }

    private void MoveToPoint(Vector3 position)
    {
        if(rigid_body == null)
        {
            transform.position = position;
        }
        else
        {
            // rigid_body.MovePosition(position);
            transform.position = position;
        }
    }

    public void AlignByPathAnimator()
    {
        if (!path_animator.corrupted_path) //  && path_animator.is_anim_running
        {
            AlignByRotation(path_animator.path_forward);
        }
    }

    public void RotateByPathAnimator()
    {
        if (!path_animator.corrupted_path) //  && path_animator.is_anim_running
        {
            Rotate(path_animator.path_forward);
        }
    }

    private void Rotate(Vector3 forward)
    {
        if (rigid_body == null)
        {
            transform.forward = forward;
        }
        else
        {
            // MoveRotation Does Not Work
            // rigid_body.MoveRotation(rigid_body.rotation * rotation);
            Quaternion target_rot = Quaternion.LookRotation(forward);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, target_rot, Time.fixedDeltaTime * rotation_speed);
        }
    }

    private void AlignByRotation(Vector3 forward)
    {
        if (rigid_body == null)
        {
            transform.forward = forward;
        }
        else
        {
            // rigid_body.MoveRotation(rigid_body.rotation * rotation);
            transform.rotation = Quaternion.LookRotation(forward);
        }
    }
}
