using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GOList : ScriptableObject
{
    public List<GameObject> list = new List<GameObject>();
    private List<GameObject> godrones = new List<GameObject>();

    public GameObject godrone_prefab = null;

    public void OnEnable()
    {
        list = new List<GameObject>();
        godrones = new List<GameObject>();
        godrone_prefab = null;
    }

    public void Add(GameObject gameobject)
    {
        if (!list.Contains(gameobject))
        {
            list.Add(gameobject);

            // If GODrone Prefab exists - create a drone for it
            if (godrone_prefab != null)
            {
                GameObject godrone = Instantiate(godrone_prefab, gameobject.transform);
                godrones.Add(godrone);
            }
        }
    }

    public void Remove(GameObject gameobject)
    {
        if (list.Contains(gameobject))
        {
            int index = list.IndexOf(gameobject);

            // Remove gameobject from the list
            list.RemoveAt(index);
            
            if (godrone_prefab != null)
            {
                // Destroy the drone and remove it from the list
                Destroy(godrones[index]);
                godrones.RemoveAt(index);
            }
        }
    }

    public GameObject Get(int index)
    {
        if (list.Count > index)
            return list[index];
        else
            return null;
    }
}
