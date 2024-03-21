using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ResourceData))]
public class ResourceEditor : Editor
{
    ResourceData resMngr;

    private void OnEnable()
    {
        resMngr = (ResourceData)target;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        EditorGUILayout.LabelField("Primary Resources: " + resMngr.primaryTypes.Count);

        List<ResourceType> trash = new();

        if (GUILayout.Button("Add Primary Resource"))
        {
            resMngr.AddPrimary();
        }

        foreach (var type in resMngr.primaryTypes)
        {
            EditorGUILayout.BeginHorizontal();
            type.name = EditorGUILayout.TextField("", type.name);
            if (GUILayout.Button("Delete"))
            {
                trash.Add(type);
            }
            EditorGUILayout.EndHorizontal();
        }

        foreach (var type in trash)
        {
            resMngr.primaryTypes.RemoveAll(x => x == type);
        }
    }
}
