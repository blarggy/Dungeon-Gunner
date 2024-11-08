using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[DisallowMultipleComponent]
public class IdleEvent : MonoBehaviour
{
    // Action delegate variable
    // This is a benefit of using the Action delegate rather than event handler, can specify parameters to pass in
    public event Action<IdleEvent> OnIdle;

    // Wrapper method that calls idle event

    public void CallIdleEvent()
    {
        OnIdle?.Invoke(this); // this refers to the IdleEvent parameter
    }
}
