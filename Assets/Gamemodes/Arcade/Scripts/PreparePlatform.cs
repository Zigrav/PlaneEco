using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}

public class PreparePlatform : MonoBehaviour
{
    private Animator animator;

    public Material from_mat_0;
    public Material from_mat_1;
    public Material from_mat_2;

    public Material to_mat_0;
    public Material to_mat_1;
    public Material to_mat_2;

    public GameObject[] infrastructure;
    public GameObject[] infrastructure_PX;

    private List<GameObject> PX;

    private List<GameObject> hexes;
    
    public float infrastructure_offsets_min;
    public float infrastructure_offsets_max;
    private List<float> infrastructure_offsets;

    public float PX_offsets_min;
    public float PX_offsets_max;
    private List<float> PX_offsets;

    public float hex_offset;
    
    public int stuff_anim_number;

    private bool is_vfx_starting = false;
    private float vfx_start_time;

    float starting_vfx_time = 0.0f;

    public GameObject target;

    private void Start()
    {
        animator = GetComponent<Animator>();

        from_mat_0 = new Material(from_mat_0);
        from_mat_1 = new Material(from_mat_1);
        from_mat_2 = new Material(from_mat_2);

        PX = new List<GameObject>();
        hexes = new List<GameObject>();
        
        CollectPlatformObjects();
        PrepareHexes();

        PrepareTarget();

        GenerateOffsets();

        GetComponent<ColorAnimHelper>().Prepare();
    }

    // Update is called once per frame
    void Update()
    {
        float passed_time = Time.time - vfx_start_time;

        if (is_vfx_starting)
        {
            // Debug.Log("starting: " + is_vfx_starting + "passed_time: " + passed_time);
            StartingVFX(passed_time);

            if (passed_time > starting_vfx_time)
            {
                is_vfx_starting = false;
                // Debug.Log("stop vfx: " + is_vfx_starting + "passed_time: " + passed_time);
            }
        }

    }

    private void GenerateOffsets()
    {
        infrastructure_offsets = new List<float>();
        PX_offsets = new List<float>();

        float max_offset = 0.0f;

        for (int i = 0; i < infrastructure.Length; i++)
        {
            infrastructure_offsets.Add(Random.value.Remap(0, 1, infrastructure_offsets_min, infrastructure_offsets_max));
            // Debug.Log(infrastructure_offsets[i]);
            max_offset = Mathf.Max(infrastructure_offsets[i], max_offset);
        }

        for (int i = 0; i < PX.Count; i++)
        {
            PX_offsets.Add(Random.value.Remap(0, 1, PX_offsets_min, PX_offsets_max));
            // Debug.Log(PX_offsets[i]);
            max_offset = Mathf.Max(PX_offsets[i], max_offset);
        }

        max_offset = Mathf.Max(hex_offset, max_offset);

        starting_vfx_time = max_offset;
    }

    [ContextMenu("StartVFX")]
    public void StartVFX()
    {
        is_vfx_starting = true;
        vfx_start_time = Time.time;
    }

    [ContextMenu("RestartVFX")]
    public void RestartVFX()
    {
        for (int i = 0; i < infrastructure.Length; i++)
        {
            Animator infr_animator = infrastructure[i].GetComponent<Animator>();
            infr_animator.SetInteger("expand", -1);
        }

        for (int i = 0; i < PX.Count; i++)
        {
            Activator px_activator = PX[i].GetComponent<Activator>();
            px_activator.DeactivateChildren();
        }

        animator.SetBool("color", false);

        GenerateOffsets();
    }

    public void StartingVFX(float passed_time)
    {
        for (int i = 0; i < infrastructure_offsets.Count; i++)
        {
            if (infrastructure_offsets[i] == -1) continue;

            if (passed_time > infrastructure_offsets[i])
            {
                // Debug.Log("infr animator is set");
                // Start Expanding Infrastructure
                Animator infr_animator = infrastructure[i].GetComponent<Animator>();

                if (infr_animator == null) throw new System.Exception(infrastructure[i].name + " does not have an animator component");

                infr_animator.SetInteger("expand", stuff_anim_number);

                infrastructure_offsets[i] = -1;
            }
        }

        for (int i = 0; i < PX_offsets.Count; i++)
        {
            if (PX_offsets[i] == -1) continue;

            if (passed_time > PX_offsets[i])
            {
                // Debug.Log("px animator is set");
                Activator px_activator = PX[i].GetComponent<Activator>();

                if (px_activator == null) throw new System.Exception(PX[i].name + " does not have an activator component");

                px_activator.ActivateChildren();

                // Mark This Object As Started
                PX_offsets[i] = -1;
            }
        }
        
        if(passed_time > hex_offset)
        {
            // Debug.Log("main animator is set");
            animator.SetBool("color", true);
        }
    }

    private void CollectPlatformObjects()
    {
        Transform[] children_transforms = GetComponentsInChildren<Transform>();

        foreach (Transform child_transform in children_transforms)
        {
            GameObject platform_object = child_transform.gameObject;

            if (!platform_object.activeSelf) continue;
            
            if (platform_object.name.Contains("hexagon") || platform_object.name.Contains("hex_triangle"))
            {
                Material hex_MAT = platform_object.GetComponent<Renderer>().sharedMaterial;

                if (!hex_MAT.name.Contains("foliage"))
                {
                    hexes.Add(platform_object);
                    // Debug.Log(platform_object);

                    platform_object.tag = "OnPlatformPass";
                }
            }

            if (platform_object.name.Contains("PS") && platform_object.tag == "OnPlatformPass")
            {
                PX.Add(platform_object);
                // Debug.Log(platform_object);
            }
        }
    }

    private void PrepareTarget()
    {
        Vector3 new_target_pos = GameObject.Find("target_place").transform.position;
        GameObject new_target = Instantiate(target, new_target_pos, Quaternion.identity);
    }

    private void PrepareHexes()
    {
        Transform[] children_transforms = GetComponentsInChildren<Transform>();

        for (int i = 0; i < hexes.Count; i++)
        {
            GameObject hex = hexes[i];
            Material hex_shared_mat = hex.GetComponent<Renderer>().sharedMaterial;
            Renderer hex_renderer = hex.GetComponent<Renderer>();

            // Assign Approprite Copied Bench Material
            if (hex_shared_mat.name.Contains("0"))
            {
                hex_renderer.sharedMaterial = from_mat_0;
            }
            else if (hex_shared_mat.name.Contains("1"))
            {
                hex_renderer.sharedMaterial = from_mat_1;
            }
            else if (hex_shared_mat.name.Contains("2"))
            {
                hex_renderer.sharedMaterial = from_mat_2;
            }
        }
    }



    //Vector3 new_hex_pos = hex.transform.position;
    //new_hex_pos.y += 0.01f;

    //GameObject new_hex = Instantiate(hex, new_hex_pos, hex.transform.rotation, transform);
    //new_hex.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
}