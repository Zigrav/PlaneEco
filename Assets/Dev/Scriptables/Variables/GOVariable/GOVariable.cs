using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static GameObject FindGameObjectWithTag(this GameObject parent, string tag)
    {
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (tr.tag == tag)
            {
                return tr.gameObject;
            }
        }
        return null;
    }
}

// I can't do transform.parent = gameobject.transform because of the editor bug
// (does not show that the parent changed in editor)

[CreateAssetMenu]
public class GOVariable : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

    // This game_object reference is saved even after you exit PlayMode
    private GameObject gameobject = null;
    
    // Events to be called when there is new or null object assigned to this GOVarible
    public GameEvent e_new;
    public GameEvent e_no;

    // GODrone Prefab
    public GameObject godrone_prefab;
    private GameObject godrone;

    public void OnEnable()
    {
        gameobject = null;
        godrone = null;
    }

    public GameObject go
    {
        get
        {
            // If it was set to null or if it's MissingReference
            // it will either way return null
            return gameobject;
        }
        set
        {
            if(value == null)
            {
                // Just assign and return
                gameobject = null;

                if (godrone != null) Destroy(godrone);

                // Raise The Event
                e_no.Raise();
            }
            else
            {
                // Assign new game_object to this GOVariable
                gameobject = value;

                // If GODrone Prefab exists - create new drone
                if(godrone_prefab != null)
                {
                    if (godrone != null) Destroy(godrone);
                    godrone = Instantiate(godrone_prefab, gameobject.transform);
                }

                // Raise The Event
                e_new.Raise();
            }
            
        }
    }

    
}