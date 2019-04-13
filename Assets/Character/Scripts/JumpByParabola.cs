using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.Events;

public class JumpByParabola : MonoBehaviour
{
    public PathCreator char_jump_path;
    public float char_jump_speed;
    private float path_fraction;

    private bool is_pl_jumping = false;

    public TargetVariable curr_target;
    private Rigidbody rigid_body;
    private new Collider collider;

    public UnityEvent pl_missed;

    private bool put_on_start = false;

    private AimManager aim_manager;
    private VertexPath char_fly_vertex_path;

    private void Awake()
    {
        rigid_body = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        aim_manager = GameObject.Find("AimManager").GetComponent<AimManager>();
    }

    private void FixedUpdate()
    {
        // Debug.Log("Fixed Update");
        MoveByPath();

        PutOnStart();
    }

    private void Update()
    {
        // Debug.Log("Update");
    }

    public void StartJumping()
    {
        is_pl_jumping = true;
        path_fraction = 0.0f;
    }

    public void StopJumping()
    {
        // Debug.Log("Stop Jumping");
        is_pl_jumping = false;
        path_fraction = 0.0f;
    }

    public void SchedulePutOnStart()
    {
        put_on_start = true;
    }

    public void PutOnStart()
    {
        if (put_on_start)
        {
            // Debug.Log("Put On Start");
            transform.position = new Vector3(0.0f, 5.3f, 0.0f);
            // Debug.Log("pos: " + transform.position);
            put_on_start = false;
        }
    }

    void MoveByPath()
    {
        if (!is_pl_jumping) return;

        // Debug.Log("In MoveByPath");

        path_fraction += char_jump_speed;

        if(path_fraction >= 1.0f)
        {
            // Debug.Log("> 1: " + path_fraction);
            // Player missed
            pl_missed.Invoke();

            return;
        }

        SetCharAtFraction(path_fraction);
    }

    public void MakePlFreefly()
    {
        StopJumping();

        Collider sphere_collider = GetComponentInChildren<SphereCollider>();
        Collider box_collider = GetComponentInChildren<BoxCollider>();

        box_collider.enabled = false;
        sphere_collider.isTrigger = false;
        rigid_body.isKinematic = false;
    }

    void SetCharAtFraction(float path_fraction)
    {
        // Debug.Log("SetCharAtFraction: " + path_fraction);
        char_fly_vertex_path = aim_manager.char_fly_vertex_path;

        Vector3 curr_aim_post = char_fly_vertex_path.GetPoint(path_fraction);

        rigid_body.MovePosition(curr_aim_post);
    }

}
