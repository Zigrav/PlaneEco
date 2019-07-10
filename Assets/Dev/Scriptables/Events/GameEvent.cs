// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    /// <summary>
    /// The list of listeners that this event will notify if it is raised.
    /// </summary>
    private List<GameEventListener> eventListeners =
        new List<GameEventListener>();

    public void OnEnable()
    {
        eventListeners = new List<GameEventListener>();
    }

    public void Raise()
    {
        for (int i = 0; i < eventListeners.Count; i++)
        {
            eventListeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            int new_listener_order = listener.Order;

            // If It's The First Listener Then Simply Add It
            if (eventListeners.Count == 0)
            {
                eventListeners.Add(listener);
                return;
            }

            for (int i = 0; i < eventListeners.Count; i++)
            {
                if(new_listener_order <= eventListeners[i].Order)
                {
                    // Insert Before Current Member / Index
                    eventListeners.Insert(i, listener);
                    return;
                }
            }

            // If Its Order Is Bigger Than Each Member
            // Then simply add it to the end
            eventListeners.Add(listener);
        }
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
