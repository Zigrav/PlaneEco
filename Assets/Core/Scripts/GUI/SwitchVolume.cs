using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchVolume : MonoBehaviour
{
    [SerializeField]
    private FloatVariable volume_var = null;

    [SerializeField]
    private Image OnImage = null;

    [SerializeField]
    private Image OffImage = null;

    public void Switch()
    {
        volume_var.v = volume_var.v == 1.0f ? 0.0f : 1.0f;

        SetVolume();
    }

    public void SetVolume()
    {
        if (volume_var.v == 1.0f)
        {
            OnImage.enabled = true;
            OffImage.enabled = false;
        }
        else
        {
            OnImage.enabled = false;
            OffImage.enabled = true;
        }

        AudioListener.volume = volume_var.v;
    }
}
