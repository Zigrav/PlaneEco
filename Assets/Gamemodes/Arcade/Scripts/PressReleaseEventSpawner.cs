#define UNITY_STANDALONE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressReleaseEventSpawner : MonoBehaviour
{
    public UnityEvent InputPress;
    public UnityEvent InputRelease;

    // Update is called once per frame
    void Update()
    {
#if UNITY_STANDALONE || UNITY_WEBPLAYER

        if(Input.GetMouseButtonDown(0))
        {
            InputPress.Invoke();
        }

        if (Input.GetMouseButtonUp(0))
        {
            InputRelease.Invoke();
        }

#else

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            InputPress.Invoke();
        }

        if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            InputRelease.Invoke();
        }

#endif

    }
}
