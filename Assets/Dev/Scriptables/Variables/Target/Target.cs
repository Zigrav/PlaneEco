using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour
{
    public TargetVariable target;
    public UnityEvent NewCurrTarget;

    private void OnEnable()
    {
        target.v = this;
        NewCurrTarget.Invoke();
    }

    private void OnDisable()
    {
        if(target.v = this)
        {
            target.v = null;
        }
    }

}