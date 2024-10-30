
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    [CustomEditor(typeof(ItemScriptableObject))]
    public class ItemScriptableObjectCustomInspector : Editor
    {
        public override void OnInspectorGUI ()
        {
            serializedObject.Update();
            var so = target as ItemScriptableObject;
            var weaponProperty = serializedObject.FindProperty("WeaponSettings");

            so.Icon = EditorGUILayout.ObjectField("Icon",so.Icon,typeof(Sprite),allowSceneObjects : false) as Sprite;
            so.Name = EditorGUILayout.TextField("Name",so.Name);
            so.ItemType = (ItemType)EditorGUILayout.EnumPopup("Item Type",so.ItemType);
            if(so.ItemType == ItemType.Weapon)
            {
                
                EditorGUILayout.PropertyField(weaponProperty);
                
            }
            else if(so.ItemType == ItemType.InventoryExtension)
            {
                so.ExtensionSize = EditorGUILayout.Vector2IntField("Extension Size", so.ExtensionSize);
            }



            serializedObject.ApplyModifiedProperties();
        }
    }
}