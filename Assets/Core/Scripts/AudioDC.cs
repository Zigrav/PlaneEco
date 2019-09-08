using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main;

namespace Main
{
    [Serializable]
    public class AudioDU
    {
        public string id = "";
        public AudioSource audio_source = null;
    }
}

public class AudioDC : MonoBehaviour
{
    public List<AudioDU> audio_dus = null;

}
