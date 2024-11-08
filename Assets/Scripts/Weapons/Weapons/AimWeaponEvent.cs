using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[DisallowMultipleComponent]
// This event script component is added to Player game object
public class AimWeaponEvent : MonoBehaviour
{
    public event Action<AimWeaponEvent, AimWeaponEventArgs> OnWeaponAim; // OnWeaponAim is an "Action" delegate
                                                                         // Any subscribers interested in this event will subscribe to OnWeaponAim
                                                                         // Any pubishers who want to trigger this event would call CallAimWeaponEvent();

    /// <summary>
    /// Wrapper method to invoke OnWeaponAim event <br/>
    /// Pass in parameters that need to be set for the event
    /// </summary>
    public void CallAimWeaponEvent(AimDirection aimDirection, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        // Publisher calls this method, this method invokes the OnWeaponAim event. Subscribers will subscribe to the OnWeaponAim event.
        OnWeaponAim?.Invoke(this, new AimWeaponEventArgs() { aimDirection = aimDirection, aimAngle = aimAngle, weaponAimAngle = weaponAimAngle, weaponAimDirectionVector = weaponAimDirectionVector });
            // '?' is a check for null operator, if there are no subscribers to OnWeaponAim event, it doesn't process the method call following the '?'

    }
}

public class AimWeaponEventArgs : EventArgs
    // EventArgs uses the System namespace. Standard class used w/n unity to define arguments for events. We inherit EventArgs and some parameters to pass as part of the event in the member variable section
    // this gives us the parameters needed for the OnWeaponAim event
{
    public AimDirection aimDirection;
    public float aimAngle;
    public float weaponAimAngle;
    public Vector3 weaponAimDirectionVector;

    
}