
using System;
using UnityEngine;

[Serializable,CreateAssetMenu(fileName = "GameItems/BuffBase",menuName = "GameItems/BuffBase")]
public class BuffBase : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public bool IsBuff;
    public ImpactParameter ImpactParameter;

    
}


