using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShowAd : MonoBehaviour
{
    [SerializeField]
    private GOVariable ads_manager_govar = null;

    [SerializeField]
    private UnityEvent before_ad_show = null;

    public void Show(string ad_id)
    {
        before_ad_show.Invoke();

        AdsManager ads_manager = ads_manager_govar.go.GetComponent<AdsManager>();
        ads_manager.ShowCurrAd(ad_id);
    }

}
