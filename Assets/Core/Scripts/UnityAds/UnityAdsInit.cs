using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsInit : MonoBehaviour, IUnityAdsListener
{

#if UNITY_IOS
    [SerializeField]
    private string gameId = "3245134";
#elif UNITY_ANDROID || UNITY_EDITOR
    [SerializeField]
    private string gameId = "3245135";
#endif

    [SerializeField]
    private bool test_mode = true;

    private struct AdIds
    {
        public AdIds(string ios_id, string android_id)
        {
            this.ios_id = ios_id;
            this.android_id = android_id;
        }

        public string ios_id;
        public string android_id;
    }

    private Dictionary<string, AdIds> ad_placements = new Dictionary<string, AdIds>();

    private void Awake()
    {
        Debug.Log("awake");
        InitPlacements();

        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, test_mode);
    }

    private void InitPlacements()
    {
        // banner_home > "" | "android_banner_home"
        AdIds ad_ids = new AdIds("banner_main", "banner_main");
        ad_placements.Add("banner_main", ad_ids);

        // rewarded_res > "" | "android_rewarded_res"
        ad_ids = new AdIds("rewarded_res", "rewarded_res");
        ad_placements.Add("rewarded_res", ad_ids);

        // insterstitial_runend > "" | "android_insterstitial_runend"
        ad_ids = new AdIds("interstitial_run_end", "interstitial_run_end");
        ad_placements.Add("interstitial_run_end", ad_ids);
    }

    //public void ShowAd1()
    //{
    //    Advertisement.Show("rewarded_res");
    //}

    public void ShowAd(string ad_placement)
    {
        AdIds value;
        if (!ad_placements.TryGetValue(ad_placement, out value))
        {
            Debug.LogError("The ad placement does not exist in the dictionary");
            return;
        }

        Debug.Log("before");
        if (!IsAdReady(ad_placement)) return;
        Debug.Log("after");

        if (ad_placement.Contains("banner"))
        {
#if UNITY_IOS
            Advertisement.Banner.Show(value.ios_id);
#elif UNITY_ANDROID || UNITY_EDITOR
            Advertisement.Banner.Show(value.android_id);
#endif
        }
        else
        {
#if UNITY_IOS
            Advertisement.Show(value.ios_id);
#elif UNITY_ANDROID || UNITY_EDITOR
            Debug.Log("right where it should be");
            Advertisement.Show(value.android_id);
#endif
        }
    }

    public bool IsAdReady(string ad_placement)
    {
        AdIds value;
        if (!ad_placements.TryGetValue(ad_placement, out value))
        {
            Debug.LogError("The ad placement does not exist in the dictionary");
            return false;
        }

#if UNITY_IOS
            return Advertisement.IsReady(value.ios_id);
#elif UNITY_ANDROID || UNITY_EDITOR
        return Advertisement.IsReady(value.android_id);
#endif
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {

    }

    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log("ready: " + placementId);
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
