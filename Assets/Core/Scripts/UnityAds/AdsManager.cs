using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

public class AdsManager : SerializedMonoBehaviour, IUnityAdsListener
{
    public class AdIds
    {
        public string internal_id = "";
        public string ios_id = "";
        public string android_id = "";
    }

    public enum balancer_type_enum { time_balancer, opportunity_balancer, always_yes, always_no }

    public class TimeBalancer
    {
        public int ads = 1;
        public int minutes = 1;
    }

    public class OpportunityBalancer
    {
        [ValidateInput("LessThanOne", "ads / opportunities must be <= 1 && > 0", InfoMessageType.Warning)]
        public int ads = 1;
        [ValidateInput("LessThanOne", "ads / opportunities must be  <= 1 && > 0", InfoMessageType.Warning)]
        public int opportunities = 2;

        private bool LessThanOne(int not_used)
        {
            if (opportunities == 0) return false;
            if (ads == 0) return false;

            float coeff = (float)ads / (float)opportunities;
            if(coeff <= 1 && coeff > 0)
            {
                return true;
            }

            return false;
        }
    }

    private class PlacementGroup
    {
        public string id = "";
        public List<string> internal_ids = new List<string>();

        [Title("Balance Min")]
        [HorizontalGroup("A"), HideLabel]
        public float balance_min = 0.0f;
        [Title("Balance Max")]
        [HorizontalGroup("A"), HideLabel]
        public float balance_max = 0.0f;

        public FloatVariable balance = null; // if (balance >= 1) - ad can be shown, else() - not
        public FloatVariable last_opportunity_timestamp = null; // The time stamp when it was last given an opportunity
        public IntVariable opportunities = null; // How many opportunities this ads were given

        [EnumToggleButtons, HideLabel]
        public balancer_type_enum balancer_type = balancer_type_enum.always_no;

        [ShowIf("balancer_type", balancer_type_enum.time_balancer)]
        public List<TimeBalancer> time_balancers = new List<TimeBalancer>();

        [ShowIf("balancer_type", balancer_type_enum.opportunity_balancer)]
        public List<OpportunityBalancer> opportunity_balancers = new List<OpportunityBalancer>();
    }

    private class BalancerEvent
    {
        public string id = ""; // id of this event
        public int delta = 0; // balance will be changed by delta when this event is called
        public string placement_group_id = "";
    }

#if UNITY_IOS
    private string gameId = "3259999";
#elif UNITY_ANDROID || UNITY_EDITOR
    private string gameId = "3259998";
#endif

    [SerializeField]
    private bool test_mode = true;

    [SerializeField]
    private List<PlacementGroup> placement_groups = new List<PlacementGroup>();

    [SerializeField]
    private List<AdIds> ad_placements = new List<AdIds>();

    [SerializeField]
    private List<BalancerEvent> balancer_events = new List<BalancerEvent>();

    [SerializeField]
    private BoolVariable no_ads = null;

    [SerializeField, Title("Placement Groups Excluded by No Ads")]
    private List<string> no_ads_excluded = null;

    [SerializeField]
    private IntVariable rd_resurrect_shown_count = null;

    [SerializeField]
    private string rd_resurrect_name = "";

    private void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, test_mode);
    }

    private TimeBalancer GetCurrTimeBalancer(PlacementGroup placement_group, float time)
    {
        float test_seconds = 0.0f;
        for (int i = 0; i < placement_group.time_balancers.Count; i++)
        {
            TimeBalancer time_balancer = placement_group.time_balancers[i];
            test_seconds += time_balancer.minutes * 60;

            if (time <= test_seconds)
            {
                return time_balancer;
            }
            
            if(i == placement_group.time_balancers.Count - 1)
            {
                return time_balancer;
            }
        }

        throw new System.Exception("Appropriate Time Balancer is not found! " + name);
    }

    private OpportunityBalancer GetCurrOpportunityBalancer(PlacementGroup placement_group, int opportunities)
    {
        int test_opportunities = 0;
        for (int i = 0; i < placement_group.opportunity_balancers.Count; i++)
        {
            OpportunityBalancer opportunity_balancer = placement_group.opportunity_balancers[i];
            test_opportunities += opportunity_balancer.opportunities;

            if (opportunities <= test_opportunities)
            {
                return opportunity_balancer;
            }

            if (i == placement_group.opportunity_balancers.Count - 1)
            {
                return opportunity_balancer;
            }
        }

        throw new System.Exception("Appropriate Opportunity Balancer is not found! " + name);
    }

    private AdIds GetAdIds(string internal_id)
    {
        for (int i = 0; i < ad_placements.Count; i++)
        {
            if(ad_placements[i].internal_id == internal_id)
            {
                return ad_placements[i];
            }
        }

        throw new System.Exception(internal_id + " id is not included in ad_placements dictionary.");
    }

    // Get Placement Group By one of its internal_ids
    private PlacementGroup GetPlacementGroup(string internal_id)
    {
        for (int i = 0; i < placement_groups.Count; i++)
        {
            List<string> placement_group_internal_ids = placement_groups[i].internal_ids;

            if (placement_group_internal_ids.Contains(internal_id))
            {
                return placement_groups[i];
            }
        }

        throw new System.Exception(internal_id + " id is not included in any placement group.");
    }

    // Get Placement Group By one of its internal_ids
    private PlacementGroup GetPlacementGroupById(string placement_group_id)
    {
        for (int i = 0; i < placement_groups.Count; i++)
        {
            if (placement_groups[i].id == placement_group_id)
            {
                return placement_groups[i];
            }
        }

        throw new System.Exception("There is no placement group with such id: " + placement_group_id);
    }

    public void ShowCurrAd(string internal_id)
    {
        // Get Stuff about the ad by the internal_id
        AdIds ad_ids = GetAdIds(internal_id);

        // Auto-call balancer events on banner and interstitials
        if(internal_id.Contains("rd"))
        {
            CallBalancerEvent("rd shown");
        }
        else if (internal_id.Contains("br"))
        {
            CallBalancerEvent("br shown");
        }
        else if (internal_id.Contains("il"))
        {
            CallBalancerEvent("il shown");
        }

        // If it is a resurrection ad
        // Debug.Log("Showing: " + " internal_id: " + internal_id + " rd_resurrect_name: " + rd_resurrect_name);
        if (internal_id.Contains(rd_resurrect_name))
        {
            rd_resurrect_shown_count.v++;
        }

        // "br" is the abbreviation that is used for all banner ads
        if (internal_id.Contains("br"))
        {
#if UNITY_IOS
            Advertisement.Banner.Show(value.ios_id);
#elif UNITY_ANDROID || UNITY_EDITOR
            Advertisement.Banner.Show(ad_ids.android_id);
#endif
        }
        else
        {
#if UNITY_IOS
            Advertisement.Show(value.ios_id);
#elif UNITY_ANDROID || UNITY_EDITOR
            // Debug.Log("Android ad will be shown");
            Advertisement.Show(ad_ids.android_id);
#endif
        }
    }

    public bool CheckOpportunity(string internal_id)
    {
        // Get Stuff about the ad by the internal_id
        AdIds ad_ids = GetAdIds(internal_id);
        PlacementGroup placement_group = GetPlacementGroup(internal_id);

        // If No Ads is bought and placement_group is excluded by this purchase
        if (no_ads.v && no_ads_excluded.Contains(placement_group.id)) return false;

        // If resurrection ad was already shown this level
        // Debug.Log("Check Opp: " + " internal_id: " + internal_id + " rd_resurrect_shown_count: " + rd_resurrect_shown_count.v + " rd_resurrect_name: " + rd_resurrect_name);
        if (internal_id.Contains(rd_resurrect_name) && rd_resurrect_shown_count.v >= 1)
        {
            return false;
        }
        
        if(placement_group.balancer_type == balancer_type_enum.time_balancer)
        {
            // Get current time
            float curr_time = Time.time;

            TimeBalancer time_balancer = GetCurrTimeBalancer(placement_group, Time.time);

            float minutes_since_last_opportunity = (curr_time - placement_group.last_opportunity_timestamp.v) / 60;
            float ad_per_minutes = ((float) time_balancer.ads / (float) time_balancer.minutes );

            placement_group.balance.v += minutes_since_last_opportunity * ad_per_minutes;

            placement_group.last_opportunity_timestamp.v = curr_time;
        }
        else if (placement_group.balancer_type == balancer_type_enum.opportunity_balancer)
        {
            // This counts as an opportunity
            placement_group.opportunities.v++;

            OpportunityBalancer opportunity_balancer = GetCurrOpportunityBalancer(placement_group, placement_group.opportunities.v);
            float ad_per_opportunity = ( (float) opportunity_balancer.ads / (float) opportunity_balancer.opportunities);

            // 1 symbolizes one opportunity that is provided with this method
            placement_group.balance.v += 1 * ad_per_opportunity;
        }
        else if (placement_group.balancer_type == balancer_type_enum.always_yes)
        {
            // Just set it to 1
            placement_group.balance.v = 1.0f;
        }
        else if (placement_group.balancer_type == balancer_type_enum.always_no)
        {
            // Just set it to 0
            placement_group.balance.v = 0.0f;
        }

        if (placement_group.balance.v < placement_group.balance_min) placement_group.balance.v = placement_group.balance_min;
        if (placement_group.balance.v > placement_group.balance_max) placement_group.balance.v = placement_group.balance_max;

        // Debug.Log("id: " + internal_id + " group: " + placement_group.id + " bal: " + placement_group.balance.v);

        if (placement_group.balance.v < 1) return false;

#if UNITY_IOS
        string platform_ad_id = ad_ids.ios_id;
#elif UNITY_ANDROID || UNITY_EDITOR
        string platform_ad_id = ad_ids.android_id;
#endif

        if (!Advertisement.IsReady(platform_ad_id)) return false;

        return true;
    }

    // With rewarded ads, opportunity should be realized (this method should be called) when the ad is proposed. With interstitials - when it is shown.
    // Developer needs to call the event that this method is called upon, when it is appropriate
    public void CallBalancerEvent(string event_id)
    {
        // Get Stuff about the event
        BalancerEvent balancer_event = GetBalancerEvent(event_id);

        // Get the placement group that will be affected by this event
        PlacementGroup affected_placement_group = GetPlacementGroupById(balancer_event.placement_group_id);

        // Change by delta
        affected_placement_group.balance.v += balancer_event.delta;

        if (affected_placement_group.balance.v < affected_placement_group.balance_min) affected_placement_group.balance.v = affected_placement_group.balance_min;
        if (affected_placement_group.balance.v > affected_placement_group.balance_max) affected_placement_group.balance.v = affected_placement_group.balance_max;

        // Debug.Log("Event. id: " + event_id + " group: " + affected_placement_group.id + " bal: " + affected_placement_group.balance.v);
    }

    private BalancerEvent GetBalancerEvent(string id)
    {
        for (int i = 0; i < balancer_events.Count; i++)
        {
            BalancerEvent balancer_event = balancer_events[i];

            if(balancer_event.id == id)
            {
                return balancer_event;
            }
        }

        throw new System.Exception("No balancer event found by this id: " + id);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {

    }

    public void OnUnityAdsReady(string placementId)
    {

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
