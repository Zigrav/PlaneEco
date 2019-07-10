using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandPosInProbes : MonoBehaviour
{
    public BoolVariable is_right;

    public GOList probes;
    public GOList reflected_probes;

    public void SetNewCameraPos()
    {
        if (is_right.v)
        {
            // Debug.Log("Reflected");
            transform.position = RandPointInProbes.Get(reflected_probes.list);
        }
        else
        {
            // Debug.Log("Not Reflected");
            transform.position = RandPointInProbes.Get(probes.list);
        }
    }

    //public GameObject test_thing;

    //[ContextMenu("Test Random")]
    //public void TestRandom()
    //{
    //    for (int i = 0; i < 100; i++)
    //    {
    //        Vector3 new_camera_pos = RandPointInProbes.Get(probes.list);
    //        GameObject new_orb = Instantiate(test_thing, new_camera_pos, Quaternion.identity) as GameObject;
    //    }
    //}
}
