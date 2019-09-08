using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectGODWithAnimators : MonoBehaviour
{
    private List<Animator> circles_animators = new List<Animator>();

    [SerializeField]
    private EventAnimator event_animator = null;

    private void Awake()
    {
        foreach(Transform sibling in transform.parent)
        {
            // If it is not the drone itself
            if(sibling.gameObject.GetComponent<ConnectGODWithAnimators>() == null)
            {
                circles_animators.Add(sibling.GetComponent<Animator>());
            }
        }

        // Set animator of the outer circle to play first (cause it's called on Release)z
        SetAnimator(1);
    }

    public void SetAnimator(int index)
    {
        event_animator.animator = circles_animators[index];
    }
}
