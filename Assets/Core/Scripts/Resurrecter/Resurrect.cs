using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resurrect : MonoBehaviour
{
    [SerializeField]
    private GameObject error_target = null;

    [SerializeField]
    private GameObject camera_fly_path = null;

    [SerializeField]
    private new GameObject camera = null;

    [SerializeField]
    private GameObject focus_point_path = null;

    [SerializeField]
    private GameObject focus_point = null;

    [SerializeField]
    private GameEvent new_target = null;

    [SerializeField]
    private GOVariable target = null;

    [SerializeField]
    private BoolVariable is_shootable = null;

    public void Res()
    {
        new_target.Raise();

        PrepareRes();
    }

    public void PrepareRes()
    {
        GameObject drone = target.go.transform.Find("Target_GOD(Clone)").gameObject;

        // Bring outer target back
        drone.GetComponent<ConnectGODWithAnimators>().SetAnimator(1);
        drone.GetComponent<EventAnimator>().SetInteger(-1);

        // Start Anim from the beginning
        camera_fly_path.GetComponent<PathAnimator>().StartAnimation();
        // Set Camera Itself To Path's start
        camera.GetComponent<MoveByPath>().OnFixedUpdate.Invoke(); // just call whatever stuff is usually called through OnFixedUpdate
        // Stop Anim
        camera_fly_path.GetComponent<PathAnimator>().StopAnimation();

        // Start Anim from the beginning
        focus_point_path.GetComponent<PathAnimator>().StartAnimation();
        // Set focus point back to start
        focus_point.GetComponent<MoveByPath>().SetToStartOfPathAnim();
        //  Stop Anim
        focus_point_path.GetComponent<PathAnimator>().StopAnimation();

        // Move error_target back to center and set its anim to default
        error_target.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        error_target.GetComponent<EventAnimator>().SetInteger(-1);

        // Make char be able to shoot again
        is_shootable.v = true;
    }
}
