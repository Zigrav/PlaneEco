using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandPosOnEvent : MonoBehaviour
{
    public ProbeManager probe_manager;

    public GameObject test_thing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNewCameraPos()
    {
        // Debug.Log("SetNewCameraPos");

        // Should Use Reflect probes
        bool should_use_reflected = (Random.value > 0.5f);

        if(should_use_reflected)
        {
            // Debug.Log("Reflected");
            transform.position = RandPointInProbes.Get(probe_manager.reflected_probes);
        }
        else
        {
            // Debug.Log("Not Reflected");
            transform.position = RandPointInProbes.Get(probe_manager.probes);
        }

        
    }

    [ContextMenu("Test Random")]
    public void TestRandom()
    {
        for (int i = 0; i < 100; i++)
        {
            Vector3 new_camera_pos = RandPointInProbes.Get(probe_manager.probes);
            GameObject new_orb = Instantiate(test_thing, new_camera_pos, Quaternion.identity) as GameObject;
        }
    }

    public void MoveToNextPos()
    {

    }
}
