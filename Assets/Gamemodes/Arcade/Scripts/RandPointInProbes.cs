using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandPointInProbes
{
    public static Vector3 Get(ProbeManager.Probe[] probes)
    {
        // Set orb position
        int rand_probe_index = UnityEngine.Random.Range(0, probes.Length);

        ProbeManager.Probe rand_probe = probes[rand_probe_index];

        float rand_x_turn = UnityEngine.Random.Range(0.0f, 360.0f);
        float rand_y_turn = UnityEngine.Random.Range(0.0f, 360.0f);
        float rand_z_turn = UnityEngine.Random.Range(0.0f, 360.0f);

        float rand_probe_radius = UnityEngine.Random.Range(0.0f, rand_probe.radius);

        Vector3 rand_probe_point = Quaternion.Euler(rand_x_turn, rand_y_turn, rand_z_turn) * Vector3.up;
        
        rand_probe_point *= rand_probe_radius;

        rand_probe_point += rand_probe.pos;

        return rand_probe_point;
    }

}

























//using System.Linq;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using PathCreation;

//static class ListExtension
//{
//    public static T PopAt<T>(this List<T> list, int index)
//    {
//        T r = list[index];
//        list.RemoveAt(index);
//        return r;
//    }

//    public static bool IsWithin(this float value, float one_float, float other_float)
//    {
//        float minimum = Mathf.Min(one_float, other_float);
//        float maximum = Mathf.Max(one_float, other_float);

//        return value >= minimum && value <= maximum;
//    }
//}

//public class SpawnOrbs : MonoBehaviour
//{
//    public float cast_orb_dist;

//    public PathCreator orb_spawn_area;
//    private float right_border_x = 0.0f;
//    private float left_border_x = 0.0f;

//    public float min_gamearea_x = 0.0f;
//    public float max_gamearea_x = 0.0f;

//    public Vector3 start_spawn_point;
//    private Vector3 spawn_point;

//    public int simple_orbs_space = 0;
//    public int simple_orbs_range = 2;

//    public int switcher_orbs_space = 1;
//    public int switcher_orbs_range = 3;

//    public int falsy_orbs_space = 1;
//    public int falsy_orbs_range = 3;

//    public GameObject alpha_orb;
//    public GameObject beta_orb;

//    public FloatReference pl_orb_mode;

//    public float gizmo_orb_x = 4.0f;

//    private List<int> orb_orb_modes;
//    private List<int> orb_switchers;
//    private List<int> orb_falsies;

//    private Component player_trans;
//    private Collider pl_collider;
//    private Collision pl_collision;

//    private List<GameObject> spawned_orbs;

//    private void Awake()
//    {
//        right_border_x = orb_spawn_area.path.bounds.center.x + orb_spawn_area.path.bounds.extents.x;
//        left_border_x = orb_spawn_area.path.bounds.center.x - orb_spawn_area.path.bounds.extents.x;

//        orb_orb_modes = new List<int>();
//        orb_switchers = new List<int>();
//        orb_falsies = new List<int>();

//        spawn_point = start_spawn_point;

//        player_trans = GameObject.FindWithTag("Player").transform;
//        pl_collider = GameObject.FindWithTag("Player").GetComponent<Collider>();
//        pl_collision = GameObject.FindWithTag("Player").GetComponent<Collision>();

//        spawned_orbs = new List<GameObject>();

//        RenewLists();
//    }

//    private void FixedUpdate()
//    {
//        while (IsNewOrbNeeded())
//        {
//            RenewLists();
//            SpawnOrb();
//        }

//        if (CheckIfOrbIsPast())
//        {
//            float pl_hp = pl_collision.hp;

//            bool is_switcher = spawned_orbs[0].GetComponent<OrbSettings>().is_switcher;

//            int orb_orb_mode = (int)spawned_orbs[0].GetComponent<OrbSettings>().orb_mode;
//            float orb_regen_force = spawned_orbs[0].gameObject.GetComponent<OrbSettings>().regen_force;
//            float orb_degen_force = spawned_orbs[0].GetComponent<OrbSettings>().degen_force;

//            if (orb_orb_mode == pl_orb_mode.v)
//            {
//                pl_hp -= orb_degen_force;
//            }
//            else
//            {
//                pl_hp += orb_regen_force;
//            }

//            //if (is_switcher)
//            //{
//            //    pl_collision.SwitchOrbMode();
//            //}

//            pl_hp = Mathf.Clamp(pl_hp, 0.0f, pl_collision.get_max_hp);

//            pl_collision.hp = pl_hp;

//            DeleteOrb();
//        }
//    }

//    private void OnDrawGizmos()
//    {
//        Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);

//        Gizmos.DrawCube(new Vector3(min_gamearea_x, 1.5f, 10.0f), new Vector3(0.2f, 1.0f, 5.0f));
//        Gizmos.DrawCube(new Vector3(max_gamearea_x, 1.5f, 10.0f), new Vector3(0.2f, 1.0f, 5.0f));

//        //right_border_x = orb_spawn_area.path.bounds.center.x + orb_spawn_area.path.bounds.extents.x;
//        //left_border_x = orb_spawn_area.path.bounds.center.x - orb_spawn_area.path.bounds.extents.x;

//        //if (gizmo_orb_x.IsWithin(left_border_x, right_border_x))
//        //{
//        //    List<Vector3> list = GetLineAndPolygonIntersectionPoints(gizmo_orb_x);
//        //    Gizmos.DrawCube(list[0], new Vector3(0.3f, 0.3f, 0.3f));
//        //    Gizmos.DrawCube(list[1], new Vector3(0.3f, 0.3f, 0.3f));
//        //}

//        //Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
//        //Vector3 right = orb_spawn_area.path.bounds.center;
//        //right.x = right_border_x;
//        //Gizmos.DrawCube(right, new Vector3(0.3f, 0.3f, 2.0f));

//        //Vector3 left = orb_spawn_area.path.bounds.center;
//        //left.x = left_border_x;
//        //Gizmos.DrawCube(left, new Vector3(0.3f, 0.3f, 2.0f));
//    }

//    [ContextMenu("test new orbs")]
//    public void Testlist()
//    {
//        player_trans = GameObject.FindWithTag("Player").transform;

//        orb_orb_modes = new List<int>();
//        orb_switchers = new List<int>();
//        orb_falsies = new List<int>();

//        spawn_point = start_spawn_point;

//        for (int i = 0; i < 30; i++)
//        {
//            RenewLists();
//            SpawnOrb();
//        }
//    }

//    private bool IsNewOrbNeeded()
//    {
//        // Test if new orb is needed
//        if (Mathf.Abs(spawn_point.z - player_trans.transform.position.z) < cast_orb_dist)
//        {
//            return true;
//        }

//        return false;
//    }

//    private void RenewLists()
//    {
//        if (orb_orb_modes.Count == 0)
//        {
//            orb_orb_modes = CreateNewList(simple_orbs_range, simple_orbs_space);
//        }

//        if (orb_switchers.Count == 0)
//        {
//            orb_switchers = CreateNewList(switcher_orbs_range, switcher_orbs_space);
//        }

//        if (orb_falsies.Count == 0)
//        {
//            orb_falsies = CreateNewList(falsy_orbs_range, falsy_orbs_space);
//        }
//    }

//    private List<int> CreateNewList(int range, int space)
//    {
//        List<int> list = Enumerable.Repeat(0, range + space).ToList();

//        int one_index = Random.Range(0, range);

//        list[one_index] = 1;

//        return list;
//    }

//    private void SpawnOrb()
//    {
//        // Set OrbSettings values
//        int neworb_orb_mode = orb_orb_modes.PopAt(0);
//        bool is_neworb_switcher = orb_switchers.PopAt(0) == 1 ? true : false;
//        bool is_neworb_falsy = orb_falsies.PopAt(0) == 1 ? true : false;

//        // Set up orb prefab
//        GameObject orb_prefab = neworb_orb_mode == 0 ? alpha_orb : beta_orb;

//        // Set local orb position and move it to world position
//        Vector3 local_orb_pos = GetNewOrbPos();
//        Vector3 orb_pos = local_orb_pos + spawn_point;

//        if (!orb_pos.x.IsWithin(min_gamearea_x, max_gamearea_x))
//        {
//            local_orb_pos.x *= -1;
//            orb_pos = local_orb_pos + spawn_point;
//        }

//        // Set orb's z position at new orb_spawn_z
//        spawn_point = orb_pos;

//        // Instantiate an orb
//        InstantiateOrb(orb_prefab, orb_pos, neworb_orb_mode, is_neworb_falsy, is_neworb_switcher);
//    }

//    private Vector3 GetNewOrbPos()
//    {
//        // Set orb position
//        return new Vector3(spawn_point.x, spawn_point.y, spawn_point.z + );
//    }

//    private void SpawnOrbOld()
//    {
//        // Set OrbSettings values
//        int neworb_orb_mode = orb_orb_modes.PopAt(0);
//        bool is_neworb_switcher = orb_switchers.PopAt(0) == 1 ? true : false;
//        bool is_neworb_falsy = orb_falsies.PopAt(0) == 1 ? true : false;

//        ///
//        //is_neworb_switcher = false;
//        //is_neworb_falsy = false;
//        /// 

//        // Set up orb prefab
//        GameObject orb_prefab = neworb_orb_mode == 0 ? alpha_orb : beta_orb;

//        // Set local orb position and move it to world position
//        Vector3 local_orb_pos = GetNewOrbPosOld();
//        Vector3 orb_pos = local_orb_pos + spawn_point;

//        if (!orb_pos.x.IsWithin(min_gamearea_x, max_gamearea_x))
//        {
//            local_orb_pos.x *= -1;
//            orb_pos = local_orb_pos + spawn_point;
//        }

//        // Set orb's z position at new orb_spawn_z
//        spawn_point = orb_pos;

//        // Instantiate an orb
//        InstantiateOrb(orb_prefab, orb_pos, neworb_orb_mode, is_neworb_falsy, is_neworb_switcher);
//    }

//    private Vector3 GetNewOrbPosOld()
//    {
//        // Set orb position
//        float orb_x = Random.Range(left_border_x, right_border_x);

//        List<Vector3> list = GetLineAndPolygonIntersectionPoints(orb_x);
//        float orb_z = Random.Range(list[0].z, list[1].z);

//        orb_x = Random.Range(0, 2) == 1 ? -orb_x : orb_x;

//        return new Vector3(orb_x, 0.0f, orb_z);
//    }

//    private List<Vector3> GetLineAndPolygonIntersectionPoints(float orb_x)
//    {
//        List<Vector3> points_collected = new List<Vector3>();

//        Vector3[] vertices = orb_spawn_area.path.vertices;
//        int l = orb_spawn_area.path.NumVertices;

//        for (int first_index = 0; first_index < l; first_index++)
//        {
//            int second_index = first_index == l - 1 ? 0 : first_index + 1;

//            // Special case when line is parallel to x axis
//            if (orb_x == vertices[first_index].x && orb_x == vertices[second_index].x) continue;

//            if (orb_x.IsWithin(vertices[first_index].x, vertices[second_index].x))
//            {
//                points_collected.Add(GetPointFromX(orb_x, vertices[first_index], vertices[second_index]));
//            }

//            if (points_collected.Count == 2)
//            {
//                break;
//            }
//        }

//        return points_collected;
//    }

//    private Vector3 GetPointFromX(float x, Vector3 from, Vector3 to)
//    {
//        Vector3 vect = to - from;
//        float ratio_x = (x - from.x) / vect.x;
//        vect *= ratio_x;

//        Vector3 point = from + vect;

//        return point;
//    }

//    private void InstantiateOrb(GameObject orb_prefab, Vector3 orb_pos, int orb_mode, bool is_falsy, bool is_switcher)
//    {
//        GameObject new_orb = Instantiate(orb_prefab, orb_pos, Quaternion.identity) as GameObject;

//        OrbSettings orb_settings = new_orb.GetComponent<OrbSettings>();
//        orb_settings.orb_mode = orb_mode;
//        orb_settings.is_falsy = is_falsy;
//        orb_settings.is_switcher = is_switcher;

//        orb_settings.SetUpOrbSettings();

//        spawned_orbs.Add(new_orb);
//    }

//    private bool CheckIfOrbIsPast()
//    {
//        Collider orb_collider = spawned_orbs[0].GetComponent<Collider>();
//        float orb_collider_upper_point_z = orb_collider.bounds.center.z + orb_collider.bounds.extents.z;
//        float pl_collider_lower_point_z = pl_collider.bounds.center.z - pl_collider.bounds.extents.z;

//        if (pl_collider_lower_point_z > orb_collider_upper_point_z)
//        {
//            return true;
//        }

//        return false;
//    }

//    public void DeleteOrb()
//    {
//        // Debug.Log("Clear Orb");
//        Destroy(spawned_orbs[0]);
//        spawned_orbs.RemoveAt(0);
//    }

//}
