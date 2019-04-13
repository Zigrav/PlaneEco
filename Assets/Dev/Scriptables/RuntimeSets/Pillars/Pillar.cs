using UnityEngine;

public class Pillar : MonoBehaviour
{
    public PillarsRuntimeSet RuntimeSet;

    private void OnEnable()
    {
        RuntimeSet.Add(this);
    }

    private void OnDisable()
    {
        RuntimeSet.Remove(this);
    }
}