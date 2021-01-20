using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class AmmoDisplayHandler
{
    public static AmmoDisplayHandler Instance { get; private set; }

    public event EventHandler<OnAmmoDiplayChangeArgs> OnAmmoChanged;
    public class OnAmmoDiplayChangeArgs : EventArgs
    {
        public int CurrentAmmo;
        public int MaxAmmo;
    }

    public AmmoDisplayHandler()
    {
        Instance = this;

        World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<WeaponFiringSystem>().OnAmmoChange += AmmoDisplayHandler_OnAmmoChanged;
        World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<WeaponReloadSystem>().OnAmmoChange += AmmoDisplayHandler_OnAmmoChanged;
    }

    public void DestroySelf()
    {
        if (World.DefaultGameObjectInjectionWorld != null)
        {
            World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<WeaponFiringSystem>().OnAmmoChange -= AmmoDisplayHandler_OnAmmoChanged;
            World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<WeaponReloadSystem>().OnAmmoChange -= AmmoDisplayHandler_OnAmmoChanged;
        }
    }

    private void AmmoDisplayHandler_OnAmmoChanged(object sender, OnAmmoDiplayChangeArgs e)
    {
        Debug.Log("AmmoDisplayHandler_OnAmmoChanged: Event Fired.");
        OnAmmoChanged?.Invoke(this, e);
    }

}
