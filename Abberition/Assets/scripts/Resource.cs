using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Resource //it kills me to make this a class *crying emoji* so much wasted memory
{
    [SerializeReference]
    public ResourceType type;
    public int typeIndex; //this needs a lot of attention
    public int amount;

    public Resource(ResourceType type = null, int amount = 1, int typeIndex = 0)
    {
        this.type = type;
        this.amount = amount;
        this.typeIndex = typeIndex;
    }
}
