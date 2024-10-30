using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "GameItems/Item",  menuName = "GameItems/Item")]
    public class ItemScriptableObject : ScriptableObject
    {
        public Sprite Icon;
        public string Name;
        public ItemType ItemType;
        public WeaponSettings WeaponSettings;
        public Vector2Int ExtensionSize;
        
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
}