using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliders2DState : MonoBehaviour
{
    public Collider2D inner_test_collider;
    public Collider2D outer_test_collider;
    public Collider2D border_test_collider;

    public int GetState(Collider2D inner_collider, Collider2D outer_collider, Collider2D border_collider)
    {
        bool is_touching = inner_collider.IsTouching(outer_collider);
        bool is_touching_border = inner_collider.IsTouching(border_collider);

        if (is_touching && !is_touching_border)
        {
            // Completely Inside
            return 0;
        }
        else if (is_touching && is_touching_border)
        {
            // Inside While Colliding With Border
            return 1;
        }
        else if (!is_touching && is_touching_border)
        {
            // Touching Only Border
            return 2;
        }
        else if (!is_touching && !is_touching_border)
        {
            // Completely Outside
            return 3;
        }

        return -1;
    }

    [ContextMenu("GetState")]
    public void GetState()
    {
        bool is_touching = inner_test_collider.IsTouching(outer_test_collider);
        bool is_touching_border = inner_test_collider.IsTouching(border_test_collider);

        if (is_touching && !is_touching_border)
        {
            // Completely Inside
            Debug.Log(0);
        }
        else if (is_touching && is_touching_border)
        {
            // Inside While Colliding With Border
            Debug.Log(1);
        }
        else if (!is_touching && is_touching_border)
        {
            // Touching Only Border
            Debug.Log(2);
        }
        else if (!is_touching && !is_touching_border)
        {
            // Completely Outside
            Debug.Log(3);
        }

        Debug.Log(-1);
    }
}
