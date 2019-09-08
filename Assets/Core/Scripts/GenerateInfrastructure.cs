using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateInfrastructure : MonoBehaviour
{
    public GOVariable platform_collider_govar;
    public GOVariable map_govar;

    int frame_num = 0;

    private void FixedUpdate()
    {
        // We need to give one frame to unity's physics to calculate how colliders2d were moved
        if(frame_num == 0)
        {
            PreparePlatform();
            frame_num++;
        }
        else if(frame_num <= 3)
        {
            frame_num++;
        }
        else if(frame_num == 4)
        {
            Generate();
            frame_num++;
        }
    }

    public void Generate()
    {
        if (platform_collider_govar.go == null)
        {
            Debug.LogError("Curr Platform Collider Is Not Defined Yet!");
            return;
        }

        if (map_govar.go == null)
        {
            Debug.LogError("Curr Platform Collider Is Not Defined Yet!");
            return;
        }

        GameObject platform_collider = platform_collider_govar.go;
        Vector3 platform_collider_pos = platform_collider.transform.position;
        Quaternion platform_collider_rot = platform_collider.transform.rotation;
        GameObject map = map_govar.go;

        List<GameObject> infrastructure = map.GetComponent<UploadInfrastructure>().Upload();

        // Make Infrastructure Children Of The Platform Collider
        for (int i = 0; i < infrastructure.Count; i++)
        {
            // Debug.Log("infrastructure: " + infrastructure[i].name);
            GameObject infr_object = infrastructure[i];
            infr_object.transform.parent = platform_collider.transform;

            // Debug.Log("0 infrastructure scale : " + infrastructure[i].transform.localScale + " lossy: " + infrastructure[i].transform.lossyScale);
        }

        // Snapshot Platform Collider Transform
        Quaternion platform_collider_rotation = platform_collider.transform.rotation;
        Vector3 platform_collider_position = platform_collider.transform.position;

        // Make Platform Collider Have Same Pos / Rot / Scale As Current Platform
        // With this we are also moving all infrastructure
        platform_collider.transform.rotation = transform.parent.rotation;
        platform_collider.transform.position = transform.parent.position;

        // Deparent Infrastructure And Rotate It To Make It Really Align With The Platform
        for (int i = 0; i < infrastructure.Count; i++)
        {
            // Debug.Log("1 infrastructure scale : " + infrastructure[i].transform.localScale + " lossy: " + infrastructure[i].transform.lossyScale);

            GameObject infr_object = infrastructure[i];
            infr_object.transform.parent = null;
            infr_object.transform.RotateAround(platform_collider.transform.position, platform_collider.transform.right, 90.0f);

            // Debug.Log("2 infrastructure scale : " + infrastructure[i].transform.localScale + " lossy: " + infrastructure[i].transform.lossyScale);

        }

        // Return Platform Collider To Its Saved Transform
        platform_collider.transform.rotation = platform_collider_rotation;
        platform_collider.transform.position = platform_collider_position;

        // Assign Infrastructure To Platform Field
        transform.parent.GetComponent<PreparePlatform>().infrastructure = infrastructure;
        transform.parent.GetComponent<PreparePlatform>().GenerateOffsets();

    }

    private void PreparePlatform()
    {
        if (platform_collider_govar.go == null)
        {
            Debug.LogError("Curr Platform Collider Is Not Defined Yet!");
            return;
        }

        if (map_govar.go == null)
        {
            Debug.LogError("Curr Platform Collider Is Not Defined Yet!");
            return;
        }

        GameObject platform_collider = platform_collider_govar.go;

        // Reset Its Position Because It Could Have Been Moved By Previous Platform That Called This Script
        platform_collider.transform.position = new Vector3(0.0f, 0.0f, 0.0f);

        GOBounds platform_collider_gobounds = platform_collider.GetComponent<GOBounds>();
        GameObject map = map_govar.go;
        Bounds map_bounds = map.GetComponent<GOBounds>().bounds;

        float x_pos_extent = map_bounds.extents.x;
        float y_pos_extent = map_bounds.extents.y;

        // Rotate Randomly
        platform_collider.transform.RotateAround(platform_collider.transform.position, new Vector3(0.0f, 0.0f, 1.0f), Random.Range(0.0f, 360.0f));

        // Reset Bounds After Rotation
        platform_collider_gobounds.SetBounds();

        // Make Random Extents Smaller So That Platofrm Collider Will Not Go Outside The Map
        x_pos_extent -= platform_collider_gobounds.bounds.extents.x;
        y_pos_extent -= platform_collider_gobounds.bounds.extents.y;

        // Get X and Y Positions Relative To Map Center
        float rand_x = Random.Range(-x_pos_extent, x_pos_extent);
        float rand_y = Random.Range(-y_pos_extent, y_pos_extent);

        // Get Map Center
        Vector3 map_bb_center = map_bounds.center;

        platform_collider.transform.position = new Vector3(map_bb_center.x + rand_x, map_bb_center.y + rand_y, platform_collider.transform.position.z);

        // Reset Bounds After Movement
        platform_collider_gobounds.SetBounds();
    }
}
