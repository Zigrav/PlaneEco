using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class SpacingAnimator : MonoBehaviour
{
    public float to_spacing = 0.3f;
    public float speed = 0.01f;

    public PathCreator path;

    public void SpaceOut()
    {
        float delta = to_spacing * speed * Time.fixedDeltaTime;

        if (Mathf.Abs(path.bezierPath.AutoControlLength - to_spacing) > 0.03f)
        {
            float sign = to_spacing - path.bezierPath.AutoControlLength;
            if(sign > 0.0f)
            {
                path.bezierPath.AutoControlLength = path.bezierPath.AutoControlLength + delta;
            }
            else
            {
                path.bezierPath.AutoControlLength = path.bezierPath.AutoControlLength - delta;
            }
        }
    }
}
