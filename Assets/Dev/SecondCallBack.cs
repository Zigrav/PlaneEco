using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SecondCallBack : MonoBehaviour
{
    public UnityEvent PlayerDied;

    public FloatReference hp;

    // Start is called before the first frame update
    void Start()
    {
        float hp_one = hp.v;
        Debug.Log("hp_one: " + hp_one);
        hp.v = 32;
        Debug.Log("hp.v: " + hp.v);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SecJustDoIt(string sec_name)
    {
        Debug.Log("Sec, Ok I am just doing it! " + sec_name);
    }
}
