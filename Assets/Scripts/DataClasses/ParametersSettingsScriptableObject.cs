using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Parameters Settings")]
public class ParametersSettingsScriptableObject : ScriptableObject
{
    [SerializeField ]public List<Parameter> parameters;

    private void OnEnable()
    {
        
        Array types = Enum.GetValues(typeof(ParameterType));

        
        List<Parameter> list = new List<Parameter>();

        for (int i = 0; i < types.Length; i++)
        {
            var a =parameters.Find(p => p.ParameterType == (ParameterType)types.GetValue(i));
            if(a != null)
            {
                list.Add(a);
            }
            else
            {
                var p = new Parameter();
                p.ParameterType = (ParameterType)types.GetValue(i);
                list.Add(p);
            }
                
            
        }

        parameters = list;

    }

    public Parameter GetParameter(ParameterType type)
    {
        return parameters.Find(p => p.ParameterType == type);
    }
}


