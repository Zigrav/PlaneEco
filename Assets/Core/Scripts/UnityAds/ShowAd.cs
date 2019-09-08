using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAd : MonoBehaviour
{
    [SerializeField]
    private GOVariable ads_manager_govar = null;

    public void Show(string ad_id)
    {
        AdsManager ads_manager = ads_manager_govar.go.GetComponent<AdsManager>();

        ads_manager.ShowCurrAd(ad_id);
    }

}
