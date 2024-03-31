using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceType
{
    public string name;
    public bool renewable = false;

    public ResourceType (string name = "")
    {
        this.name = name;
    }
}
