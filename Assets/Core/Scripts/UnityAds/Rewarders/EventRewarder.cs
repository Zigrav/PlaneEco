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

    private void Start()
    {
        Advertisement.AddListener(this);
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

                finished.Invoke();
            }
            else if (showResult == ShowResult.Skipped)
            {
                // Do not reward the user for skipping the ad.
                // Debug.Log("Skipped");

                skipped.Invoke();
            }
            else if (showResult == ShowResult.Failed)
            {
                // Debug.Log("Failed");

                failed.Invoke();
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
            ad_started.Invoke();
        }
    }
}
