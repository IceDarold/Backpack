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




}