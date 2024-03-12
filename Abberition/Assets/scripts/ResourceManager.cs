using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public List<Resource> resourceTypes;

    public List<ResourceGroup> diet;
    public List<ResourceGroup> orgProducts;

    public List<ResourceGroup> rawProducts;
}

public class ResourceGroup
{
    public Resource resource;
    public float chance;
}