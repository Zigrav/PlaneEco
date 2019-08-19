using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TODELETE : MonoBehaviour
{
    public GameEvent test_event_3;
    public GameObject obj_0;

    public FloatVariable mana_cost;
    public GameObject cam_obj;
    public Camera cam;

    //private void OnValidate()
    //{
    //    if (fl == null)
    //    {
    //        fl = ScriptableObject.CreateInstance("FloatVariable") as FloatVariable;
    //    }
    //}

    public void Start()
    {
        mana_cost = ScriptableObject.CreateInstance("FloatVariable") as FloatVariable;
    }

    private void Update()
    {
        Debug.Log("go: " + mana_cost.v);
    }

    //private void Start()
    //{
    //    // DebugList(test_event_3.eventListeners);

    //    GameEventListener listener_0 = gameObject.AddComponent<GameEventListener>();
    //    listener_0.Order = 3;
    //    test_event_3.RegisterListener(listener_0);

    //    Debug.Log("registered");

    //    // DebugList(test_event_3.eventListeners);

    //    GameEventListener listener_1 = gameObject.AddComponent<GameEventListener>();
    //    listener_1.Order = 0;
    //    test_event_3.RegisterListener(listener_1);

    //    Debug.Log("registered");

    //    // DebugList(test_event_3.eventListeners);

    //    GameEventListener listener_2 = gameObject.AddComponent<GameEventListener>();
    //    listener_2.Order = 3;
    //    test_event_3.RegisterListener(listener_2);

    //    Debug.Log("registered");

    //    // DebugList(test_event_3.eventListeners);

    //    GameEventListener listener_3 = gameObject.AddComponent<GameEventListener>();
    //    listener_3.Order = 2;
    //    test_event_3.RegisterListener(listener_3);

    //    Debug.Log("registered");

    //    // DebugList(test_event_3.eventListeners);

    //    GameEventListener listener_4 = gameObject.AddComponent<GameEventListener>();
    //    listener_4.Order = 7;
    //    test_event_3.RegisterListener(listener_4);

    //    Debug.Log("registered");

    //    // DebugList(test_event_3.eventListeners);

    //    GameEventListener listener_5 = gameObject.AddComponent<GameEventListener>();
    //    listener_5.Order = 7;
    //    test_event_3.RegisterListener(listener_5);

    //    Debug.Log("registered");

    //    // DebugList(test_event_3.eventListeners);
    //}

    //[ContextMenu("DebugList")]
    //public void DebugList(List<GameEventListener> list)
    //{
    //    for (int i = 0; i < list.Count; i++)
    //    {
    //        Debug.Log("listener order: " + list[i].Order);
    //    }
    //}

}
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//public class PathAnimator : MonoBehaviour
//{
//    // We define AnimationCurve to avoid NullReferenceExceptions when creating script
//    public AnimationCurve speed_curve = new AnimationCurve(new Keyframe(0.0f, 1.0f), new Keyframe(1.0f, 1.0f));

//    public bool is_speed_control = true;

//    public float speed_coeff = 1.0f;
//    public float period = 1.0f;

//    private void OnValidate()
//    {
//        if (is_speed_control)
//        {
//            period = CalcPeriod(0.01f, speed_coeff);
//        }
//        else
//        {
//            speed_coeff = CalcSpeedCoeff(0.01f, period);
//        }
//    }

//    // Start is called before the first frame update
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    public float CalcSpeed(float fraction, float l_speed_coeff)
//    {
//        if(speed_curve == null)
//        {
//            Debug.Log("null");
//            return 0.0f;
//        }

//        return speed_curve.Evaluate(fraction) * l_speed_coeff;
//    }

//    public float CalcPeriod(float time_step, float l_speed_coeff)
//    {
//        if (speed_curve == null)
//        {
//            Debug.Log("null");
//            return 0.0f;
//        }

//        float l_period = 0.0f;
//        for (float curr_fraction = 0.0f; curr_fraction < 1.0f;)
//        {
//            curr_fraction = (CalcSpeed(curr_fraction, l_speed_coeff) * time_step) + curr_fraction;
//            l_period += time_step;
//        }

//        return l_period;
//    }

//    public float CalcSpeedCoeff(float time_step, float bench_period)
//    {
//        if (speed_curve == null)
//        {
//            Debug.Log("null");
//            return 0.0f;
//        }

//        float l_speed_coeff = 1.0f;
//        float l_period = CalcPeriod(time_step, l_speed_coeff);
//        l_speed_coeff *= l_period / bench_period;

//        return l_speed_coeff;
//    }
//}
