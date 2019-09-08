// #define UNITY_STANDALONE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR

public class PressReleaseEventSpawner : MonoBehaviour
{
    [SerializeField]
    private BoolVariable is_pointer_hold = null;

    public UnityEvent InputPress;
    public UnityEvent InputRelease;

    // Update is called once per frame
    void Update()
    {
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR

        if (Input.GetMouseButtonDown(0))
        {
            if (IsMouseOverUI()) return;

            InputPress.Invoke();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!is_pointer_hold.v) return;

            InputRelease.Invoke();
        }

#else

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if(IsMouseOverUI()) return;

            InputPress.Invoke();
        }

        if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            if (!is_pointer_hold.v) return;
            
            InputRelease.Invoke();
        }

#endif

    }

    private bool IsMouseOverUI()
    {
        bool is_mouse_over_ui = false;

        PointerEventData pointer_event_data = new PointerEventData(EventSystem.current);

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
        pointer_event_data.position = Input.mousePosition;
#else
        pointer_event_data.position = Input.GetTouch(0).position;
#endif

        List<RaycastResult> raycast_results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(pointer_event_data, raycast_results);

        if (raycast_results.Count > 0)
        {
            // Debug.Log("It is actually over UI: " + raycast_results.Count);
            is_mouse_over_ui = true;
        }

        return is_mouse_over_ui;
    }
}
