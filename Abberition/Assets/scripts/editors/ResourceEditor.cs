using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomEditor(typeof(ResourceData))]
public class ResourceEditor : Editor
{
    ResourceData resourceData;

    private void OnEnable()
    {
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        if (!resourceData) resourceData = (ResourceData)target;

        //DrawPrimaryResources();
        DrawResourceTypes();
    }

    public void DrawResourceTypes ()
    {
        EditorGUILayout.LabelField("Polymer Resources: " + resourceData.resourceTypes.Count);

        EditorGUI.indentLevel++;

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Resource"))
        {
            resourceData.AddPrimary();
        }
        //if (GUILayout.Button("Add Polymer Resource"))
        //{
        //    resourceData.AddPolymer();
        //}
        EditorGUILayout.EndHorizontal();

        var resourceList = ResourceList();

        for (var i = 0; i < resourceData.resourceTypes.Count; i++)
        {
            var resource = resourceData.resourceTypes[i];

            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            resource.name = EditorGUILayout.TextField("Resource Name", resource.name);
            if (GUILayout.Button("Delete"))
            {
                resourceData.resourceTypes.Remove(resource);
                i--;
            }
            EditorGUILayout.EndHorizontal();

            var isPolymer = EditorGUILayout.Toggle("Is Polymer", resource is PolymerType);
            if (isPolymer && !(resource is PolymerType))
            {
                resourceData.resourceTypes[i] = resource = new PolymerType(resource.name);
            }
            if (!isPolymer && resource is PolymerType)
            {
                resourceData.resourceTypes[i] = resource = new ResourceType(resource.name);
            }

            if (resource is PolymerType polymer)
            {
                EditorGUILayout.LabelField("Ingredients");
                EditorGUI.indentLevel++;

                if (polymer.ingredients.Count == 0 || GUILayout.Button("Add Ingredient"))
                {
                    polymer.ingredients.Add(new(resourceData.resourceTypes[0], 1));
                }

                foreach (var ingredient in polymer.ingredients)
                {
                    int typeIndex = resourceData.resourceTypes.IndexOf(ingredient.type);
                    typeIndex = EditorGUILayout.Popup("Resource Type", typeIndex, resourceList);
                    ingredient.type = TypeFromString(resourceList[typeIndex]);
                    ingredient.amount = EditorGUILayout.IntField("Amount", ingredient.amount);
                }

                EditorGUI.indentLevel--;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Yield Amount");
                polymer.yieldAmount = EditorGUILayout.IntField(polymer.yieldAmount);
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUI.indentLevel--;
    }

    public string[] ResourceList ()
    {
        List<string> output = new();

        foreach (var resource in resourceData.resourceTypes)
        {
            output.Add(resource.name);
        }

        return output.ToArray();
    }

    public ResourceType TypeFromString (string name)
    {
        foreach (var resource in resourceData.resourceTypes)
        {
            if (resource.name == name) return resource;
        }

        Debug.LogError("No Resource of name" + name);
        return null;
    }
}
