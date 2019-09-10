using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MatchManager : MonoBehaviour
{
    // Set of Objects to be chosen from
    public List<GameObject> platforms_set;
    public List<GameObject> platform_colliders_set;
    public List<GameObject> targets_set;
    public List<GameObject> heliports_set;
    public List<GameObject> maps_set;

    // GOVariables
    public GOVariable curr_platform;
    public GOVariable curr_platform_collider;
    public GOVariable curr_target;
    public GOVariable curr_heliport;
    public GOVariable curr_map;

    // Next GOVariables
    public GOVariable next_platform;
    public GOVariable next_target;
    public GOVariable next_heliport;

    // GOLists
    public GOList platforms;
    public GOList platform_colliders;
    public GOList targets;
    public GOList heliports;
    public GOList maps;

    public IntVariable platforms_passed;
    public BoolVariable is_right;

    [SerializeField]
    private SOList difficulty_list = null;
    [SerializeField]
    private IntVariable levels_passed = null;
    [SerializeField]
    private IntVariable currlevel_platforms_count = null;
    [SerializeField]
    private IntVariable currlevel_platforms_passed = null;

    [SerializeField]
    private GameEvent level_passed = null;
    [SerializeField]
    private GameEvent level_failed = null;

    [SerializeField]
    private BoolVariable curr_level_passed = null;

    [SerializeField]
    private int extra_platforms = 2;

    public float height_step;
    public float width_step;
    public float platform_angle;
    public float platform_scale;

    public float target_height;

    [Range(0.0f, 360.0f)]
    public float min_angle;
    [Range(0.0f, 360.0f)]
    public float max_angle;

    public float heliport_dist;

    private int next_platform_num;
    private float dist_z = 0.0f;

    public void PrepareLevelStart()
    {
        // Decide From Which Platform To Start
        next_platform_num = platforms_passed.v;
        if (next_platform_num % 2 == 0)
        {
            is_right.v = true;
        }
        else
        {
            is_right.v = false;
        }

        // UploadPlatformCollider
        UploadPlatformCollider();

        // UploadMap with an eco stuff
        UploadMap();

        // Create The Level
        CreateLevel();

        // Do the starting stuff
        next_heliport.go = heliports.Get(0);
        next_platform.go = platforms.Get(0);
        next_target.go = targets.Get(0);

        is_right.v = !is_right.v;

        next_heliport.go = heliports.Get(1);
        next_platform.go = platforms.Get(1);
        next_target.go = targets.Get(1);

        curr_heliport.go = heliports.Get(0);
        curr_platform.go = platforms.Get(0);
        curr_target.go = targets.Get(0);
    }
    
    public void CreateLevel()
    {
        // Local level variable to predict how much platforms_count will be in the future
        int predicted_level = levels_passed.v;

        // Update the real level progress data
        SODict level_difficulty_elem = GetElemByLevel.Get(levels_passed.v, difficulty_list);

        int level_platforms_count = (level_difficulty_elem.v["platforms_count"] as IntVariable).v;

        // Create tiles
        for (int i = 0; i < level_platforms_count + extra_platforms; i++)
        {
            dist_z += height_step;
            CreateNextTile();
        }

        currlevel_platforms_count.v = (level_difficulty_elem.v["platforms_count"] as IntVariable).v;

        currlevel_platforms_passed.v = 0;
    }

    public void CreateNextTile()
    {
        Vector3 pos;
        Quaternion rot;

        // Define pos and rot of the platform
        if (next_platform_num % 2 == 0)
        {
            pos = new Vector3(width_step, 0.0f, dist_z);
            rot = Quaternion.Euler(0.0f, platform_angle, 0.0f);
        }
        else
        {
            pos = new Vector3(-width_step, 0.0f, dist_z);
            rot = Quaternion.Euler(0.0f, -platform_angle, 0.0f);
        }

        // Create platform //
        GameObject platform_prefab = platforms_set[UnityEngine.Random.Range(0, platforms_set.Count)];
        GameObject platform = Instantiate(platform_prefab, pos, rot);
        platform.transform.localScale = new Vector3(platform_scale, platform_scale, platform_scale);

        platforms.Add(platform);

        // Move pos Up By target_height
        pos.y += target_height;

        // Create target //
        GameObject target_prefab = targets_set[UnityEngine.Random.Range(0, targets_set.Count)];
        GameObject target = Instantiate(target_prefab, pos, rot);

        targets.Add(target);

        // Move pos to heliport place
        Vector3 heliport_move_vect;
        if (next_platform_num % 2 == 0)
        {
            heliport_move_vect = platform.transform.right * heliport_dist;
        }
        else
        {
            heliport_move_vect = -platform.transform.right * heliport_dist;
        }
        pos += heliport_move_vect;

        // Create heliport //
        GameObject heliport_prefab = heliports_set[UnityEngine.Random.Range(0, heliports_set.Count)];
        GameObject heliport = Instantiate(heliport_prefab, pos, rot);

        heliports.Add(heliport);

        next_platform_num++;
    }

    public void UploadPlatformCollider()
    {
        // Upload Platform Collider //
        GameObject platform_collider_prefab = platform_colliders_set[0];
        GameObject platform_collider = Instantiate(platform_collider_prefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        platform_collider.transform.localScale = new Vector3(platform_scale, platform_scale, platform_scale);

        curr_platform_collider.go = platform_collider;
        platform_colliders.Add(platform_collider);
    }

    public void UploadMap()
    {
        // Upload Map //
        GameObject map_prefab = maps_set[0];
        GameObject map = Instantiate(map_prefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);

        curr_map.go = map;
        maps.Add(map);
    }

    // Called on pl_hit event. Used to reward pl for passing platform / level
    public void ProcessPlMissed()
    {
        level_failed.Raise();
    }

    // Called on pl_hit event. Used to reward pl for passing platform / level
    public void ProcessPlHit()
    {
        // Platforms of the current level passed
        currlevel_platforms_passed.v++;

        // If level was finished
        if (currlevel_platforms_passed.v == currlevel_platforms_count.v)
        {
            // Platforms passed in general by this player
            platforms_passed.v += currlevel_platforms_passed.v;

            // Levels passed in general by this player
            levels_passed.v++;

            level_passed.Raise();

            curr_level_passed.v = true;
        }
    }

    // Called on platform_passed event. Used to move curr things to the next platform
    public void Proceed()
    {
        // If it is the last platform that was cleared, do not proceed further
        if (curr_level_passed.v) return;

        is_right.v = !is_right.v;

        heliports.Remove(heliports.Get(0));
        curr_heliport.go = heliports.Get(0);

        platforms.Remove(platforms.Get(0));
        curr_platform.go = platforms.Get(0);

        targets.Remove(targets.Get(0));
        curr_target.go = targets.Get(0);
    }

    public void ProceedNextObjects()
    {
        next_heliport.go = heliports.Get(1);
        next_platform.go = platforms.Get(1);
        next_target.go = targets.Get(1);
    }

    public void log(string s)
    {
        Debug.Log(s);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}













/// New Variant
/// 

//public void CreateLevel()
//{
//    // Local level variable to predict how much platforms_count will be in the future
//    int predicted_level = levels_passed.v;

//    ///////////////////////////

//    //// Fill the level with platforms until fog will do its job
//    //while(dist_z < content_border_dist.v)
//    //{
//    //    // Get the predicted level_platforms_count
//    //    SODict predicted_level_difficulty_elem = GetElemByLevel.Get(predicted_level, difficulty_list);
//    //    int level_platforms_count = (predicted_level_difficulty_elem.v["platforms_count"] as IntVariable).v;

//    //    // Create tiles
//    //    for (int i = 0; i < level_platforms_count; i++)
//    //    {
//    //        dist_z += height_step;
//    //        CreateNextTile();
//    //    }

//    //    // Create spacing between levels
//    //    dist_z += level_height_step;

//    //    // Increase predicted level
//    //    predicted_level++;
//    //}

//    //////////////////////////

//    // Update the real level progress data
//    SODict level_difficulty_elem = GetElemByLevel.Get(levels_passed.v, difficulty_list);

//    int level_platforms_count = (level_difficulty_elem.v["platforms_count"] as IntVariable).v;

//    // Create tiles
//    for (int i = 0; i < level_platforms_count; i++)
//    {
//        dist_z += height_step;
//        CreateNextTile();
//    }
//    ////////////////////////

//    // Debug.Log("levels_passed.v: " + levels_passed.v + " level_difficulty_elem pl count: " + (level_difficulty_elem.v["platforms_count"] as IntVariable).v );

//    currlevel_platforms_count.v = (level_difficulty_elem.v["platforms_count"] as IntVariable).v;

//    currlevel_platforms_passed.v = 0;
//}




//// Old Variant

//public void CreateLevel()
//{
//    // Local level variable to predict how much platforms_count will be in the future
//    int predicted_level = levels_passed.v;

//    // Fill the level with platforms until fog will do its job
//    while (dist_z < content_border_dist.v)
//    {
//        // Get the predicted level_platforms_count
//        SODict predicted_level_difficulty_elem = GetElemByLevel.Get(predicted_level, difficulty_list);
//        int level_platforms_count = (predicted_level_difficulty_elem.v["platforms_count"] as IntVariable).v;

//        // Create tiles
//        for (int i = 0; i < level_platforms_count; i++)
//        {
//            dist_z += height_step;
//            CreateNextTile();
//        }

//        // Crate spacing between levels
//        dist_z += level_height_step;

//        // Increase predicted level
//        predicted_level++;
//    }

//    // Update the real level progress data
//    SODict level_difficulty_elem = GetElemByLevel.Get(levels_passed.v, difficulty_list);

//    // Debug.Log("levels_passed.v: " + levels_passed.v + " level_difficulty_elem pl count: " + (level_difficulty_elem.v["platforms_count"] as IntVariable).v );

//    currlevel_platforms_count.v = (level_difficulty_elem.v["platforms_count"] as IntVariable).v;

//    currlevel_platforms_passed.v = 0;
//}