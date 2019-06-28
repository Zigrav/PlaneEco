using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorAnimHelper : MonoBehaviour
{
    private Material from_mat_0;
    private Material from_mat_1;
    private Material from_mat_2;

    private Color from_color_0;
    private Color from_color_1;
    private Color from_color_2;

    private Color to_color_0;
    private Color to_color_1;
    private Color to_color_2;

    public float colorfulness = 0.0f;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAnimPlaying())
        {
            AnimateColor();
        }
    }

    public void Prepare()
    {
        PreparePlatform prep_platf = GetComponent<PreparePlatform>();

        from_mat_0 = prep_platf.from_mat_0;
        from_mat_1 = prep_platf.from_mat_1;
        from_mat_2 = prep_platf.from_mat_2;

        from_color_0 = prep_platf.from_mat_0.GetColor("_BaseColor");
        from_color_1 = prep_platf.from_mat_1.GetColor("_BaseColor");
        from_color_2 = prep_platf.from_mat_2.GetColor("_BaseColor");

        to_color_0 = prep_platf.to_mat_0.GetColor("_BaseColor");
        to_color_1 = prep_platf.to_mat_1.GetColor("_BaseColor");
        to_color_2 = prep_platf.to_mat_2.GetColor("_BaseColor");
    }

    bool IsAnimPlaying()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("empty")) return false;

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }

    private void AnimateColor()
    {
        // Debug.Log("anim: " + colorfulness);

        Color curr_color_0 = Color.Lerp(from_color_0, to_color_0, colorfulness);
        from_mat_0.SetColor("_BaseColor", curr_color_0);

        Color curr_color_1 = Color.Lerp(from_color_1, to_color_1, colorfulness);
        from_mat_1.SetColor("_BaseColor", curr_color_1);

        Color curr_color_2 = Color.Lerp(from_color_2, to_color_2, colorfulness);
        from_mat_2.SetColor("_BaseColor", curr_color_2);
    }
}
