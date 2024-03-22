using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceData : MonoBehaviour
{
    public List<ResourceType> resourceTypes = new();

    public List<Recipe> recipes = new();

    public void AddPrimary ()
    {
        resourceTypes.Add(new());
    }

    public void RemovePrimary (ResourceType type)
    {
        resourceTypes.Remove(type);
    }

    public void AddPolymer()
    {
        resourceTypes.Add(new PolymerType());
    }

    public void RemovePolymer(PolymerType type)
    {
        resourceTypes.Remove(type);
    }
}