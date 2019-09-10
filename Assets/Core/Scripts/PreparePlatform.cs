using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public List<GameObject> infrastructure;
    public List<GameObject> infrastructure_PX;

    private List<GameObject> PX;

    private GameObject platform_model = null;
    
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
        
        CollectPlatformObjects();
        SetSharedMaterials();

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

    public void GenerateOffsets()
    {
        infrastructure_offsets = new List<float>();
        PX_offsets = new List<float>();

        float max_offset = 0.0f;

        for (int i = 0; i < infrastructure.Count; i++)
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
        for (int i = 0; i < infrastructure.Count; i++)
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

                // Debug.Log(infrastructure[i].name + ": " + stuff_anim_number);
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
            
            if (platform_object.name.Contains("platform_model"))
            {
                platform_model = platform_object;
                // Debug.Log("platform_model name: " + platform_model.name);
            }

            if (platform_object.name.Contains("PS") && platform_object.tag == "OnPlatformPass")
            {
                PX.Add(platform_object);
                // Debug.Log(platform_object);
            }
        }
    }

    private void SetSharedMaterials()
    {
        Renderer platform_model_renderer = platform_model.GetComponent<Renderer>();

        // Assign Approprite Copied Bench Material
        Material[] new_shared_MATs = { platform_model_renderer.sharedMaterials[0], from_mat_0, from_mat_1, from_mat_2 };
        platform_model_renderer.sharedMaterials = new_shared_MATs;
    }

}