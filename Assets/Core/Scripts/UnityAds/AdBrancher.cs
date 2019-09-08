using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AdBrancher : MonoBehaviour
{
    [SerializeField]
    private GOVariable ads_manager_govar = null;

    [SerializeField]
    private string ad_id = ""; // internal

    [SerializeField]
    private UnityEvent has_event = null;
    [SerializeField]
    private UnityEvent no_event = null;

    public void Branch()
    {
        AdsManager ads_manager = ads_manager_govar.go.GetComponent<AdsManager>();

        if (ads_manager.CheckOpportunity(ad_id))
        {
            has_event.Invoke();
        }
        else
        {
            no_event.Invoke();
        }
    }

}
