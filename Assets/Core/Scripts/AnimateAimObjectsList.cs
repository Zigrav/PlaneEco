using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AnimateAimObjectsList : MonoBehaviour
{
    [SerializeField]
    private GOList golist = null;

    [SerializeField, SuffixLabel("frames", overlay: true)]
    private int step = 10;

    private bool is_anim_started = false;
    private int curr_anim_step = 0;
    private int next_anim_obj = 0;

    void FixedUpdate()
    {
        if (!is_anim_started) return;

        int next_anim_frame = (next_anim_obj * step);

        // Debug.Log("curr_anim_step: " + curr_anim_step + " next_anim_obj: " + next_anim_obj + " next_anim_frame: " + next_anim_frame);

        if (curr_anim_step == next_anim_frame)
        {
            // Debug.Log("inside");
            EventAnimator event_animator = golist.list[next_anim_obj].GetComponent<EventAnimator>();
            event_animator.SetParam("expand");
            event_animator.SetInteger(-1);

            // If last elem in list
            if (golist.list.Count - 1 == next_anim_obj)
            {
                // Finish the animation
                is_anim_started = false;
                next_anim_obj = 0;
                curr_anim_step = 0;
                return;
            }

            next_anim_obj++;
        }

        curr_anim_step++;
    }

    public void StartAnim()
    {
        is_anim_started = true;
    }
}
