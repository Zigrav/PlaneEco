using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public static class ExtensionMethods
//{

//    public static float Remap(this float value, float from1, float to1, float from2, float to2)
//    {
//        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
//    }

//}

public class OnPlatformPass : MonoBehaviour
{   
    //public float infrastructure_offsets_min;
    //public float infrastructure_offsets_max;
    //private List<float> infrastructure_offsets;
    
    //public float PX_offsets_min;
    //public float PX_offsets_max;
    //private List<float> PX_offsets;

    //public int hexes_anim_number;
    //public int stuff_anim_number;

    //private bool is_vfx_starting = false;
    //private float vfx_start_time;

    //private GameObject[] infrastructure;
    //private GameObject[] infrastructure_PX;

    //private List<GameObject> PX;
    
    //float starting_vfx_time = 0.0f;
    //private Animator animator;

    //// Update is called once per frame
    //void Update()
    //{
    //    float passed_time = Time.time - vfx_start_time;

    //    if (is_vfx_starting)
    //    {
    //        StartingVFX(passed_time);
    //    }

    //    if (passed_time > starting_vfx_time)
    //    {
    //        is_vfx_starting = false;
    //    }
    //}

    //public void Prepare()
    //{
    //    animator = GetComponent<Animator>();

    //    PreparePlatform prep_platform = GetComponent<PreparePlatform>();

    //    PX = prep_platform.GetPX();
    //    infrastructure = prep_platform.GetInfrastructure();
    //    infrastructure_PX = prep_platform.InfrastructurePX();

    //    GenerateOffsets();
    //}

    //private void GenerateOffsets()
    //{
    //    infrastructure_offsets = new List<float>();
    //    PX_offsets = new List<float>();

    //    float max_offset = 0.0f;

    //    for (int i = 0; i < infrastructure.Length; i++)
    //    {
    //        infrastructure_offsets.Add(Random.value.Remap(0, 1, infrastructure_offsets_min, infrastructure_offsets_max));
    //        max_offset = Mathf.Max(infrastructure_offsets[i], max_offset);
    //    }

    //    for (int i = 0; i < PX.Count; i++)
    //    {
    //        PX_offsets.Add(Random.value.Remap(0, 1, PX_offsets_min, PX_offsets_max));
    //        max_offset = Mathf.Max(PX_offsets[i], max_offset);
    //    }

    //    starting_vfx_time = max_offset;
    //}

    //[ContextMenu("StartVFX")]
    //public void StartVFX()
    //{
    //    is_vfx_starting = true;
    //    vfx_start_time = Time.time;
    //}

    //public void StartingVFX(float passed_time)
    //{
    //    for (int i = 0; i < infrastructure_offsets.Count; i++)
    //    {
    //        if (infrastructure_offsets[i] == -1) continue;
            
    //        if(passed_time > infrastructure_offsets[i])
    //        {
    //            // Start Expanding Infrastructure
    //            Animator infr_animator = infrastructure[i].GetComponent<Animator>();

    //            if (infr_animator == null) throw new System.Exception(infrastructure[i].name + " does not have an animator component");

    //            RuntimeAnimatorController infr_controller = infr_animator.runtimeAnimatorController;

    //            infr_animator.SetInteger("expand", stuff_anim_number);

    //            // Activate Infrastructure PX
    //            //Animator px_animator = infrastructure_PX[i].GetComponent<Animator>();

    //            //if (px_animator == null) throw new System.Exception(PX[i].name + " does not have an animator component");

    //            //RuntimeAnimatorController px_controller = px_animator.runtimeAnimatorController;

    //            //px_animator.SetBool("activated", true);

    //            // Mark This Object As Started
    //            infrastructure_offsets[i] = -1;
    //        }
    //    }

    //    for (int i = 0; i < PX_offsets.Count; i++)
    //    {
    //        if (PX_offsets[i] == -1) continue;

    //        if (passed_time > PX_offsets[i])
    //        {
    //            Animator px_animator = PX[i].GetComponent<Animator>();

    //            if (px_animator == null) throw new System.Exception(PX[i].name + " does not have an animator component");

    //            RuntimeAnimatorController infr_controller = px_animator.runtimeAnimatorController;

    //            px_animator.SetBool("activated", true);

    //            // Mark This Object As Started
    //            PX_offsets[i] = -1;
    //        }
    //    }
        
    //    RuntimeAnimatorController controller = animator.runtimeAnimatorController;
    //    animator.SetBool("color", true);
    //}

    //public void PlayVFX()
    //{
    //    // Expand Hexes and Activate VFX
    //    foreach (Transform child in transform)
    //    {
    //        if(child.gameObject.tag == "OnPlatformPass" && child.gameObject.activeSelf == true)
    //        {
    //            Animator animator = child.gameObject.GetComponent<Animator>();

    //            if (animator == null) throw new System.Exception(child.gameObject.name + " does not have an animator component");

    //            // Debug.Log(animator.runtimeAnimatorController.name);

    //            RuntimeAnimatorController controller = animator.runtimeAnimatorController;

    //            if (controller.name == "activator")
    //            {
    //                // Debug.Log("It's activator");
    //                animator.SetBool("activated", true);
    //            }
    //            else if(controller.name == "expander")
    //            {
    //                // Debug.Log("It's expander");
    //                animator.SetInteger("expand", hex_anim_number);
    //            }
    //        }
    //    }

    //    // Expand Platform Infrastructure
    //    foreach (GameObject pl_object in platform_objects)
    //    {
    //        Animator animator = pl_object.GetComponent<Animator>();
    //        RuntimeAnimatorController controller = animator.runtimeAnimatorController;

    //        animator.SetInteger("expand", stuff_anim_number);
    //    }
    //}

    //[ContextMenu("ResetVFX")]
    //public void ResetVFX()
    //{
    //    // Expand Hexes and Activate VFX
    //    foreach (Transform child in transform)
    //    {
    //        if (child.gameObject.tag == "OnPlatformPass" && child.gameObject.activeSelf == true)
    //        {
    //            Animator animator = child.gameObject.GetComponent<Animator>();

    //            if (animator == null) throw new System.Exception(child.gameObject.name + " does not have an animator component");

    //            // Debug.Log(animator.runtimeAnimatorController.name);

    //            RuntimeAnimatorController controller = animator.runtimeAnimatorController;

    //            if (controller.name == "activator")
    //            {
    //                // Debug.Log("It's activator");
    //                animator.SetBool("activated", false);
    //            }
    //            else if (controller.name == "expander")
    //            {
    //                // Debug.Log("It's expander");
    //                animator.SetInteger("expand", -1);
    //            }
    //        }
    //    }

    //    // Expand Platform Infrastructure
    //    foreach(GameObject pl_object in platform_objects)
    //    {
    //        Animator animator = pl_object.GetComponent<Animator>();
    //        RuntimeAnimatorController controller = animator.runtimeAnimatorController;

    //        animator.SetInteger("expand", -1);
    //    }
    //}

}
