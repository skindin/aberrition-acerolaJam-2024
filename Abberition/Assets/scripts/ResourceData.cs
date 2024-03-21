using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceData : MonoBehaviour
{
    public List<ResourceType> primaryTypes = new();
    public List<PolymerType> polymerTypes = new();

    public List<Recipe> recipes = new();

    public void AddPrimary ()
    {
        primaryTypes.Add(new());
    }

    public void RemovePrimary (ResourceType type)
    {
        primaryTypes.Remove(type);
    }

    public void AddPolymer()
    {
        primaryTypes.Add(new());
    }

    public void RemovePolymer(PolymerType type)
    {
        polymerTypes.Remove(type);
    }
}