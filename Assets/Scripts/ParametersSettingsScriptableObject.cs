using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Parameters Settings")]
public class ParametersSettingsScriptableObject : ScriptableObject
{
    public List<ParametersSettingsScriptableObject> parameters;
}

[Serializable]
public class Parameter
{

}
