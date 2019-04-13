using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyOwnEvent : UnityEvent<string>
{

}

public class TestEvent : MonoBehaviour
{
    public UnityEvent test_event;
    public MyOwnEvent my_test_event;
    
    int w = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(w);
        w++;
        if(w == 100)
        {
            test_event.Invoke();
            my_test_event.Invoke("petro");
        }
    }
}
