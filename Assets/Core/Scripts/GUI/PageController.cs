using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvas_group = null;

    public void HidePage()
    {
        canvas_group.alpha = 0.0f; //this makes everything transparent
        canvas_group.blocksRaycasts = false; //this prevents the UI element to receive input events
        canvas_group.interactable = false; //this prevents the UI element to receive input events
    }

    public void ShowPage()
    {
        canvas_group.alpha = 1.0f; //this makes everything transparent
        canvas_group.blocksRaycasts = true; //this prevents the UI element to receive input events
        canvas_group.interactable = true; //this prevents the UI element to receive input events
    }

    public void ActivatePage()
    {
        canvas_group.blocksRaycasts = true; //this prevents the UI element to receive input events
        canvas_group.interactable = true; //this prevents the UI element to receive input events
    }

    public void DeactivatePage()
    {
        canvas_group.blocksRaycasts = false; //this prevents the UI element to receive input events
        canvas_group.interactable = false; //this prevents the UI element to receive input events
    }
}
