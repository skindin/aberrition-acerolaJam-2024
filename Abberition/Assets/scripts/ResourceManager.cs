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

    public void MutateOrganismRecipes (Organism organism)
    {
        foreach (var recipe in recipes)
        {
            var mutationChance = recipe.mutationChance * organism.instability.value;

            if (Random.value <= mutationChance)
            {
                if (!organism.recipes.Contains(recipe))
                {
                    organism.recipes.Add(recipe);
                }
                else
                {
                    organism.recipes.Remove(recipe);
                }
            }
        }
    }
}