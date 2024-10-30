using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "GameItems/Item",  menuName = "GameItems/Item")]
    public class ItemScriptableObject : ScriptableObject
    {
        public Sprite Icon;
        public string Name;
        public int Price;
        public ItemType ItemType;
        public WeaponSettings WeaponSettings;
        public Vector2Int ExtensionSize;

        public List<CustomEffect> Effects;
    }

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

    [Serializable]
    public class CustomEffect
    {
        public string Name;
        public EffectType EffectType;

        public float DelayTime;

        public EventType eventType;


        public CustomEffect()
        {
            Name = "New Custom Effect";
        }
    }
}