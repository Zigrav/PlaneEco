using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main;
using Sirenix.OdinInspector;

public class PlayTrack : MonoBehaviour
{
    [SerializeField]
    private GOVariable audio_dc = null;

    private List<AudioDU> audio_dus_v;
    private List<AudioDU> audio_dus
    {
        get
        {
            audio_dus_v = audio_dc.go.GetComponent<AudioDC>().audio_dus;
            return audio_dus_v;
        }
    }

    // [SerializeField]

    [Button]
    public void Play(string play_id)
    {
        AudioDU audio_du = audio_dus.Find(x => x.id == play_id);

        if (audio_du == null)
        {
            throw new System.Exception("AudioDU with " + play_id + " id was not found!");
        }

        audio_du.audio_source.Play();
    }
}
