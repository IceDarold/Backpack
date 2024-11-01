using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ParametersSettingsScriptableObject))]
public class ParametersSettingsCustomInspector : Editor
{
    private List<string> parameters;
    private bool hasChanges;

    public void OnEnable()
    {
        parameters = Enum.GetNames(typeof(ParameterType)).ToList();
    }

    public override void OnInspectorGUI()
    {
        string header = hasChanges ? "Parameters*" : "Parameters";
        EditorGUILayout.TextArea(header, EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        for (int i = 0; i < parameters.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            string cash = parameters[i];
            parameters[i] = EditorGUILayout.TextField(parameters[i]);
            if (cash != parameters[i])
            {
                hasChanges = true;
            }


            if (GUILayout.Button("-"))
            {
                parameters.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

            
        }

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add new parameter"))
        {
            parameters.Add("NewParameter");
            hasChanges = true;
        }

        if (GUILayout.Button("Apply"))
        {
            Apply();
            hasChanges = false;
        }

        if (GUILayout.Button("Return"))
        {
            parameters = Enum.GetNames(typeof(ParameterType)).ToList();
            hasChanges = false;
        }

        EditorGUILayout.EndHorizontal();
    }


    private void Apply()
    {
        string path = Application.dataPath + "/Scripts/Enums/ParametrType.cs";
        string text = "public enum ParameterType \n { \n";

        for (int i = 0; i < parameters.Count; i++)
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

