using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Evolution : MonoBehaviour
{
    public ResourceData resourceMngr;

    //these represent the chance of a given trait mutating within a million years
    public float recipeChance, compChance, aggChance, instabChance;

    public void Evolve (Organism organism)
    {
        
    }

    public Resource FindResource (List<Resource> list, ResourceType type)
    {
        return list.Find(x => x.type == type);
    }
}
