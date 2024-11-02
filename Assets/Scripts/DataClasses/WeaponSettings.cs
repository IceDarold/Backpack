using System;
using UnityEngine;

[Serializable]
public struct WeaponSettings
{
    public int MinDamage;
    public int MaxDamage;
    public float ReloadingTime;
    [Range(0f, 1f)]
    public float Accuracy;
    public float Stamina;
}
