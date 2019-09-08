using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.Events;

[ExecuteInEditMode]
public class PathAnimator : MonoBehaviour
{
    public GenerateArcPath component_with_vertex_path;

    public VertexPath vertex_path;

    public PathGenerator path_generator;

    // We define AnimationCurve to avoid NullReferenceExceptions when creating script
    public AnimationCurve speed_curve = new AnimationCurve(new Keyframe(0.0f, 1.0f), new Keyframe(1.0f, 1.0f));
    
    public float speed_coeff = 1.0f;
    public float period = 1.0f;

    public bool MoveInWorldCoordinates = false;

    [Range(0.01f, 1.0f)]
    public float min_speed = 0.5f;

    [Range(0.0f, 1.0f)]
    public float stop_fraction = 0.99f;

    public EndOfPathInstruction end_of_path_instruction = EndOfPathInstruction.Stop;

    // This flag is not needed though, cause we don't show it in a Custom Editor
    [HideInInspector]
    public bool is_anim_running = false;
    private float curr_fraction = 0.0f;

    // This flag is not needed though, cause we don't show it in a Custom Editor
    [HideInInspector]
    public bool corrupted_path = false;

    // This flag is not needed though, cause we don't show it in a Custom Editor
    [HideInInspector]
    public Vector3 calc_position = new Vector3(0.0f, 0.0f, 0.0f);

    [HideInInspector]
    public Vector3 path_forward = new Vector3(0.0f, 0.0f, 0.0f);

    public UnityEvent OnStart;
    public UnityEvent OnFixedUpdate;
    public UnityEvent OnFixedUpdateAnim;
    public UnityEvent OnPathEnd;

    private bool is_set_on_start = false;

    // Original saved_coeff that was put into Inspector
    private float original_speed_coeff = 1.0f;

    // Is animation being slowed down slowely
    private bool is_gradually_stopping_anim = false;
    // Fraction by which we will be slowing the anim
    private float gradual_stop_fraction = 0;

    // How long should it take for animation to stop gradually (in frames)
    [SerializeField]
    private int gradual_stop_length = 0;

    private void Start()
    {
        original_speed_coeff = speed_coeff;

        OnStart.Invoke();
    }

    public void StartPathAnimator()
    {
        // Debug.Log("StartPathAnimator: " + gameObject.name);

        InitPathAnimator();
        CalcStartData();
        StopAnimation();
    }

    public void InitPathAnimator()
    {
        if(path_generator != null)
        {
            vertex_path = path_generator.vertex_path;
        }
        else if(component_with_vertex_path.vertex_path != null)
        {
            vertex_path = component_with_vertex_path.vertex_path;
        }
        else
        {
            Debug.Log("PathCreator or GenerateArcPath must be given!");
        }
    }

    public void SetFractionToStart()
    {
        curr_fraction = 0.0f;
    }

    public void CalcStartData()
    {
        if (!is_set_on_start && !corrupted_path)
        {
            calc_position = vertex_path.GetPoint(curr_fraction, end_of_path_instruction);
            // Debug.Log(" pos : " + calc_position + " curr_fraction: " + curr_fraction + "  " + gameObject.name);

            path_forward = Vector3.Normalize(vertex_path.GetPoint(curr_fraction + 0.01f, end_of_path_instruction) - vertex_path.GetPoint(curr_fraction, end_of_path_instruction));
            is_set_on_start = true;
        }

        // Debug.Log(vertex_path.GetPoint(curr_fraction, end_of_path_instruction) + " name: " + gameObject.name);
    }

    public void SetOnCurrFraction()
    {
        calc_position = vertex_path.GetPoint(curr_fraction, end_of_path_instruction);
        path_forward = vertex_path.GetPoint(curr_fraction + 0.01f, end_of_path_instruction) - vertex_path.GetPoint(curr_fraction, end_of_path_instruction);
    }

    private void FixedUpdate()
    {
        if (is_anim_running)
        {
            OnFixedUpdateAnim.Invoke();
        }
        else
        {
            OnFixedUpdate.Invoke();
        }

        // If we are slowing down this animation
        if (is_gradually_stopping_anim)
        {
            speed_coeff -= gradual_stop_fraction;
            if (speed_coeff <= 0.0f)
            {
                StopAnimation();
                speed_coeff = original_speed_coeff;
                is_gradually_stopping_anim = false;
            }

        }

        FrameMove(end_of_path_instruction);
        FrameRotate(end_of_path_instruction);
    }

    public void FrameMove(EndOfPathInstruction end_of_path_instruction)
    {
        if (is_anim_running)
        {
            InitPathAnimator();

            float delta_fraction = CalcSpeed(curr_fraction, speed_coeff) * Time.fixedDeltaTime;

            if (MoveInWorldCoordinates)
            {
                // In order to make point move equally in all paths we need to adjust it by path length
                delta_fraction /= vertex_path.length;
            }

            curr_fraction += delta_fraction;

            // Debug.Log(curr_fraction + "  " + gameObject.name);

            if (curr_fraction >= stop_fraction)
            {
                if (end_of_path_instruction == EndOfPathInstruction.Stop)
                {
                    curr_fraction = stop_fraction;
                    // SetOnCurrFraction();
                    StopAnimation();
                }
                else if (end_of_path_instruction == EndOfPathInstruction.Loop)
                {
                    curr_fraction = 0.0f;
                }

                OnPathEnd.Invoke();
            }

            calc_position = vertex_path.GetPoint(curr_fraction, end_of_path_instruction);
        }
    }

    public void FrameRotate(EndOfPathInstruction end_of_path_instruction)
    {
        InitPathAnimator();
        path_forward = vertex_path.GetPoint(curr_fraction + 0.01f, end_of_path_instruction) - vertex_path.GetPoint(curr_fraction, end_of_path_instruction);
    }

    public void StartAnimation()
    {
        curr_fraction = 0.0f;
        InitPathAnimator();

        is_anim_running = true;
        // Debug.Log("start");

        SetOnCurrFraction();
        // Debug.Log("after setoncurrfraction: " + curr_fraction + "  " + gameObject.name);
        // FrameMove(end_of_path_instruction);
        // FrameRotate(end_of_path_instruction);
    }

    public void StopAnimation()
    {
        is_anim_running = false;
    }

    public void GraduallyStopAnimation()
    {
        if (gradual_stop_length <= 0)
        {
            throw new System.Exception("gradual_stop_length equals " + gradual_stop_length + ", while trying to stop gradually! " + this.name);
        }

        gradual_stop_fraction = speed_coeff / gradual_stop_length;
        is_gradually_stopping_anim = true;
    }

    public float CalcSpeed(float fraction, float l_speed_coeff)
    {
        float norm_speed = speed_curve.Evaluate(fraction);

        if(norm_speed <= min_speed)
        {
            norm_speed = min_speed;
        }

        return norm_speed * l_speed_coeff;
    }

    public float CalcPeriod(float time_step, float l_speed_coeff)
    {
        float l_period = 0.0f;
        float curr_fraction = 0.0f;

        while(curr_fraction < 1.0f)
        {
            curr_fraction = (CalcSpeed(curr_fraction, l_speed_coeff) * time_step) + curr_fraction;
            l_period += time_step;
        }

        return l_period;
    }

    public float CalcSpeedCoeff(float time_step, float bench_period)
    {
        float l_speed_coeff = 1.0f;
        float l_period = CalcPeriod(time_step, l_speed_coeff);
        l_speed_coeff *= l_period / bench_period;

        return l_speed_coeff;
    }
}
