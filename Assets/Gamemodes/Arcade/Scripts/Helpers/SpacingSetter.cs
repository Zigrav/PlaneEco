using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class SpacingSetter : MonoBehaviour
{
    public PathCreator path;

    public void SetSpacing(float spacing)
    {
        path.bezierPath.AutoControlLength = spacing;
        Debug.Log("set back: " + path.bezierPath.AutoControlLength);
    }
}
