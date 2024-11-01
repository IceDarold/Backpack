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
    private List<bool> states;
    private bool hasChanges;

    public void OnEnable()
    {
        parameters = Enum.GetNames(typeof(ParameterType)).ToList();
        
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ParametersSettingsScriptableObject so = (ParametersSettingsScriptableObject)target;
        if (states == null || states.Count != so.parameters.Count) states = new List<bool>(new bool[so.parameters.Count]);

        

        string header = hasChanges ? "Parameters*" : "Parameters";
        EditorGUILayout.TextArea(header, EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        for (int i = 0; i < parameters.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            if(i < so.parameters.Count )states[i] = EditorGUILayout.Foldout(states[i],"");
            //EditorGUILayout.Space(-10);

            string cash = parameters[i];
            parameters[i] = EditorGUILayout.TextField(parameters[i]);
            if (cash != parameters[i])
            {
                hasChanges = true;
            }


            if (GUILayout.Button("-"))
            {
                hasChanges = true;
                parameters.RemoveAt(i);
                so.parameters.RemoveAt(i);
                states.RemoveAt(i);
                continue;
            }
            EditorGUILayout.EndHorizontal();

            if(i < so.parameters.Count)
            {
                //Debug.Log(i);
                if (states[i])
                {
                    EditorGUI.indentLevel++;
                    DrawParameter(i, so);
                    EditorGUI.indentLevel--;
                }
            }
            



            EditorGUILayout.Space(5);

            
        }

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add new parameter"))
        {
            parameters.Add("NewParameter");
            states.Add(false);
            hasChanges = true;
        }

        if (GUILayout.Button("Apply"))
        {
            CodeGeneration();
            DataChange(so);

            EditorUtility.SetDirty(so);
            AssetDatabase.SaveAssetIfDirty(so);

            hasChanges = false;

            AssetDatabase.Refresh();
        }

        /*if (GUILayout.Button("Return"))
        {
            parameters = Enum.GetNames(typeof(ParameterType)).ToList();
            hasChanges = false;
        }*/

        EditorGUILayout.EndHorizontal();


        serializedObject.ApplyModifiedProperties();

    }

    private void DrawParameter(int index, ParametersSettingsScriptableObject so)
    {
       /* EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextField("Parameter Type",so.parameters[index].ParameterType.ToString());
        EditorGUI.EndDisabledGroup();*/

        so.parameters[index].MaxParameterType = (ParameterType)EditorGUILayout.EnumPopup("Max Parameter Type", so.parameters[index].MaxParameterType);
        so.parameters[index].StartValue = EditorGUILayout.FloatField("Start Value",so.parameters[index].StartValue);

        float maxValue = so.GetParameter(so.parameters[index].MaxParameterType).StartValue;

        if (so.parameters[index].StartValue > maxValue)
        {
            so.parameters[index].StartValue = maxValue;
        }
    }




    private void DataChange(ParametersSettingsScriptableObject so)
    {
        if(so.parameters.Count <= parameters.Count)
        {
            for (int i = 0; i < parameters.Count; i++)
            {
                if (i >= so.parameters.Count)
                {
                    var param = new Parameter();
                    param.ParameterType = (ParameterType)i;
                    so.parameters.Add(param);

                }
            }
        }

        

        
    }

    private void CodeGeneration()
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
        

    }
}

