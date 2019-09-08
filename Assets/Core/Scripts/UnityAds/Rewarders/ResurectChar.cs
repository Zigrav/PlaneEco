using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class ResurectChar : MonoBehaviour, IUnityAdsListener
{
    public string ad_placement;

    private void Start()
    {
        Advertisement.AddListener(this);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        Debug.Log("Called from ResurectChar");
        if (placementId == ad_placement)
        {
            // Define conditional logic for each ad completion status:
            if (showResult == ShowResult.Finished)
            {
                // Reward the user for watching the ad to completion.
                Debug.Log("Finished");
            }
            else if (showResult == ShowResult.Skipped)
            {
                // Do not reward the user for skipping the ad.
                Debug.Log("Skipped");
            }
            else if (showResult == ShowResult.Failed)
            {
                Debug.Log("Failed");
            }
        }

        // is_shootable = true;
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
        // Optional actions to take when the end-users triggers an ad.
    }
}
