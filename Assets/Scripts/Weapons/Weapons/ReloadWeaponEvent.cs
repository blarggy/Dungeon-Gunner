using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[DisallowMultipleComponent]
public class ReloadWeaponEvent : MonoBehaviour
{
    public event Action<ReloadWeaponEvent, ReloadWeaponEventArgs> OnReloadWeapon;

    /// <summary>
    /// Specify the weapon to have its magazine reloaded. If the total ammo is also to be increased then specify the topUpAmmoPercent
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="topUpAmmoPercent"></param>
    public void CallReloadWeaponEvent(Weapon weapon, int topUpAmmoPercent)
    {
        OnReloadWeapon?.Invoke(this, new ReloadWeaponEventArgs() { weapon = weapon, topUpAmmoPercent = topUpAmmoPercent });
    }
}

public class ReloadWeaponEventArgs : EventArgs
{
    public Weapon weapon;
    public int topUpAmmoPercent; // how much ammo that needs to be topped up
}