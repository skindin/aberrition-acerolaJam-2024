using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.Linq;

[CustomEditor(typeof(ResourceManager))]
public class ResourceEditor : Editor
{
    ResourceManager manager;
    bool showResourceTypes = true, showRecipes = true;
    [HideInInspector]
    List<bool> recipeVisibility = new();

    private void OnEnable()
    {

    }

    public override void OnInspectorGUI()
    {
        if (!manager) manager = (ResourceManager)target;

        var invisCount = manager.recipes.Count - recipeVisibility.Count;
        for (var i = 0; i < invisCount; i++)
        {
            recipeVisibility.Add(true);
        }

        //base.OnInspectorGUI()

        //DrawPrimaryResources();
        DrawResourceTypes();
        EditorGUILayout.Space(10);
        DrawRecipes();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(manager);
            EditorSceneManager.MarkSceneDirty(manager.gameObject.scene);
        }
    }

    public void DrawResourceTypes ()
    {
        var label = "Resource Types (" + manager.resourceTypes.Count + ")";
        showResourceTypes = EditorGUILayout.Foldout(showResourceTypes,label);

        if (!showResourceTypes) return;

        EditorGUI.indentLevel++;

        //EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Resource Type"))
        {
            manager.AddResource();
        }
        //if (GUILayout.Button("Add Polymer Resource"))
        //{
        //    resourceData.AddPolymer();
        //}
        //EditorGUILayout.EndHorizontal();

        for (var i = 0; i < manager.resourceTypes.Count; i++)
        {
            var resource = manager.resourceTypes[i];

            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            resource.name = EditorGUILayout.TextField("", resource.name);
            if (GUILayout.Button("Delete"))
            {
                manager.resourceTypes.Remove(resource);
                i--;
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUI.indentLevel--;
    }

    public void DrawRecipes ()
    {
        var label = "Recipes (" + manager.recipes.Count + ")";
        showRecipes = EditorGUILayout.Foldout(showRecipes, label);

        if (!showRecipes) return;

        EditorGUI.indentLevel++;

        if (GUILayout.Button("Add Recipe"))
        {
            manager.AddRecipe();
            recipeVisibility.Add(true);
        }

        for (var i = 0; i < manager.recipes.Count; i++)
        {
            var recipe = manager.recipes[i];

            var recipeLabel = "Recipe " + (i + 1) + ": " + recipe.ReadRecipe();

            recipeVisibility[i] = EditorGUILayout.Foldout(recipeVisibility[i], recipeLabel);

            if (recipeVisibility[i])
            {
                EditorGUI.indentLevel++;
                DrawResourceList(recipe.ingredients,"Ingredients");
                EditorGUILayout.Space(10);
                DrawResourceList(recipe.products,"Products");//
                if (GUILayout.Button("Delete"))
                {
                    manager.recipes.Remove(recipe);
                    recipeVisibility.RemoveAt(i);
                    i--;
                }
                EditorGUI.indentLevel--;
            }
        }

        EditorGUI.indentLevel--;
    }

    public void DrawResource (Resource resource)
    {
        resource.typeIndex = EditorGUILayout.Popup("", resource.typeIndex, ResourceList(), GUILayout.Width(200));//
        resource.typeIndex = Mathf.Clamp(resource.typeIndex, 0, manager.resourceTypes.Count - 1);
        resource.type = manager.resourceTypes[resource.typeIndex];
        resource.amount = EditorGUILayout.IntField("", resource.amount, GUILayout.Width(200));
    }

    public void DrawResourceList(List<Resource> resources, string label)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label);
        if (GUILayout.Button("Add Resource"))
        {
            var newResource = new Resource();
            resources.Add(newResource);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUI.indentLevel++;
        for (var i = 0; i < resources.Count; i++)//
        {
            var resource = resources[i];

            EditorGUILayout.BeginHorizontal();
            DrawResource(resource);
            if (GUILayout.Button("Delete"))
            {
                resources.Remove(resource);
                i--;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUI.indentLevel--;
    }

    public string[] ResourceList ()
    {
        List<string> output = new();

        foreach (var resource in manager.resourceTypes)
        {
            output.Add(resource.name);
        }

        return output.ToArray();
    }


    //public ResourceType TypeFromString (string name)
    //{
    //    foreach (var resource in resourceData.resourceTypes)
    //    {
    //        if (resource.name == name) return resource;
    //    }

    //    Debug.LogError("No Resource of name" + name);
    //    return null;
    //}
}
