using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAdShower : MonoBehaviour
{
    [SerializeField]
    private GOVariable ads_manager_govar = null;

    [SerializeField]
    public string ad_id = "";

    public void StartBannerCoroutine()
    {
        StartCoroutine(ShowBannerWhenReady());
    }

    IEnumerator ShowBannerWhenReady()
    {
        AdsManager ads_manager = ads_manager_govar.go.GetComponent<AdsManager>();

        // Debug.Log("Enter coroutine");
        while (!ads_manager.CheckOpportunity(ad_id))
        {
            // Debug.Log("Wait");
            yield return new WaitForSeconds(0.5f);
        }

        // Debug.Log("Show");
        ads_manager.ShowCurrAd(ad_id);
    }
}