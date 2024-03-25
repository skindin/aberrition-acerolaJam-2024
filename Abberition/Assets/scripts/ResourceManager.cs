using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public List<ResourceType> resourceTypes = new();

    public List<Recipe> recipes = new();

    public void AddResource ()
    {
        resourceTypes.Add(new());
    }

    public void RemoveResource (ResourceType type)
    {
        resourceTypes.Remove(type);
    }

    public void AddRecipe()
    {
        recipes.Add(new());
    }

    public void RemoveRecipe (Recipe recipe)
    {
        recipes.Remove(recipe);
    }
}