using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancerEventCaller : MonoBehaviour
{
    [SerializeField]
    private GOVariable ads_manager_govar = null;

    public void CallEvent(string event_id)
    {
        AdsManager ads_manager = ads_manager_govar.go.GetComponent<AdsManager>();

        ads_manager.CallBalancerEvent(event_id);
    }

}
