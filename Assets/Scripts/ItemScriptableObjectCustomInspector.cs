
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    [CustomEditor(typeof(ItemScriptableObject))]
    public class ItemScriptableObjectCustomInspector : Editor
    {
        private List<bool> states;
        private ItemScriptableObject so;
        public void OnEnable()
        {
            so = target as ItemScriptableObject;
            states = new List<bool>();

        }
        public override void OnInspectorGUI ()
        {



            
            serializedObject.Update();

            if(so.Effects == null)
            {
                so.Effects = new List<CustomEffect> ();
            }

            if(states.Count != so.Effects.Count) 
            {
                states = new List<bool>(new bool[so.Effects.Count]);
            }

            var weaponProperty = serializedObject.FindProperty("WeaponSettings");

            so.Icon = EditorGUILayout.ObjectField("Icon",so.Icon,typeof(Sprite),allowSceneObjects : false) as Sprite;
            so.Name = EditorGUILayout.TextField("Name",so.Name);
            so.Price = EditorGUILayout.IntField("Price", so.Price);
            so.ItemType = (ItemType)EditorGUILayout.EnumPopup("Item Type",so.ItemType);
            if(so.ItemType == ItemType.Weapon)
            {
                
                EditorGUILayout.PropertyField(weaponProperty);
                
            }
            else if(so.ItemType == ItemType.InventoryExtension)
            {
                so.ExtensionSize = EditorGUILayout.Vector2IntField("Extension Size", so.ExtensionSize);
            }

            EditorGUILayout.Space(20);

            EditorGUILayout.LabelField("Custom Effects",EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            for(int i = 0; i < so.Effects.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                states[i] = EditorGUILayout.Foldout(states[i], so.Effects[i].Name);

                bool res = GUILayout.Button("Remove");
                EditorGUILayout.EndHorizontal();

                if(res)
                {
                    states.RemoveAt(i);
                    so.Effects.RemoveAt(i);
                    continue;
                }

                if (states[i])
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.Space(5);
                    DrawElement(i);
                    EditorGUI.indentLevel--;
                }
                
                
            }

            EditorGUILayout.Space(10);

            if (GUILayout.Button("Add new custom effect"))
            {
                states.Add(false);
                so.Effects.Add(new CustomEffect());
            }

             


            serializedObject.ApplyModifiedProperties();
        }

        private void DrawElement(int index)
        {
            CustomEffect effect = so.Effects[index];
            effect.Name = EditorGUILayout.TextField("Name", so.Effects[index].Name);
            effect.EffectType = (EffectType) EditorGUILayout.EnumPopup("Effect Type",effect.EffectType);

            if(effect.EffectType == EffectType.Delay)
            {
                effect.DelayTime = EditorGUILayout.FloatField("Delay Time", effect.DelayTime);
            }
            else if(effect.EffectType == EffectType.Event)
            {
                effect.eventType = (EventType)EditorGUILayout.EnumPopup("Event Type", effect.eventType);
            }

            
        }
    }
}