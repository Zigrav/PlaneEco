using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Advertisements;

public class EventRewarder : MonoBehaviour, IUnityAdsListener
{
    [SerializeField]
    private string ad_placement = "";

    [SerializeField]
    private UnityEvent finished = null;
    [SerializeField]
    private UnityEvent skipped = null;
    [SerializeField]
    private UnityEvent failed = null;

    [SerializeField]
    private UnityEvent ad_started = null;

    private bool on_ad_started_callback = false;
    private bool on_finished_callback = false;
    private bool on_skipped_callback = false;
    private bool on_failed_callback = false;

    private void Start()
    {
        Advertisement.AddListener(this);
    }

    private void Update()
    {
        if (on_ad_started_callback)
        {
            ad_started.Invoke();
            on_ad_started_callback = false;
        }

        if (on_finished_callback)
        {
            finished.Invoke();
            on_finished_callback = false;
        }

        if (on_skipped_callback)
        {
            skipped.Invoke();
            on_skipped_callback = false;
        }

        if (on_failed_callback)
        {
            failed.Invoke();
            on_failed_callback = false;
        }
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId == ad_placement)
        {
            // Define conditional logic for each ad completion status:
            if (showResult == ShowResult.Finished)
            {
                // Reward the user for watching the ad to completion.
                // Debug.Log("Finished");

                on_finished_callback = true;

                //finished.Invoke();
            }
            else if (showResult == ShowResult.Skipped)
            {
                // Do not reward the user for skipping the ad.
                // Debug.Log("Skipped");

                on_skipped_callback = true;

                //skipped.Invoke();
            }
            else if (showResult == ShowResult.Failed)
            {
                // Debug.Log("Failed");

                on_failed_callback = true;

                //failed.Invoke();
            }
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        // Called on each ad when it ready to be shown
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        if (placementId == ad_placement)
        {
            on_ad_started_callback = true;
        }
    }
}
