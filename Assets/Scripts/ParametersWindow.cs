


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;


public class ParametersWindow: EditorWindow
{
    private List<string> parameters;
    private static ParametersWindow instance;

    [MenuItem("Window/Parameters")]
    public static void ShowWindow()
    {
        instance = GetWindow<ParametersWindow>("Parameters");
        instance.parameters = Enum.GetNames(typeof(ParameterType)).ToList();
    }

    private void OnGUI()
    {
        EditorGUILayout.TextArea("Parameters",EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        for (int i = 0; i < parameters.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            parameters[i] = EditorGUILayout.TextField(parameters[i]);

            if (GUILayout.Button("-"))
            {
                parameters.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);
        }

        EditorGUILayout.BeginHorizontal();

        if(GUILayout.Button("Add new parameter"))
        {
            parameters.Add("NewParameter");
        }

        if (GUILayout.Button("Apply"))
        {
            Apply();
        }

        if (GUILayout.Button("Return"))
        {
            parameters = Enum.GetNames(typeof(ParameterType)).ToList();
        }

        EditorGUILayout.EndHorizontal() ;
    }


    private void Apply()
    {
        string path = Application.dataPath + "/Scripts/Enums/ParametrType.cs";
        string text = "public enum ParameterType \n { \n";

        for (int i = 0; i <parameters.Count; i++)
        {
            text += parameters[i];
            text += ',';
            text += '\n';
        }

        text += '}';

        File.WriteAllText(path, text);
        //File.AppendAllText(path, text);
        AssetDatabase.Refresh();
        
    }
}