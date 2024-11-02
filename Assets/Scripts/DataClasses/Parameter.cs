using System;
using UnityEngine;

[Serializable]
public class Parameter
{
    [SerializeField] public ParameterType ParameterType;
    [SerializeField] public ParameterType MaxParameterType;
    [SerializeField] public float StartValue;


    public Parameter() { }
}
