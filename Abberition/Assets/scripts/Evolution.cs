using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Evolution : MonoBehaviour
{
    public ResourceManager resourceMngr;

    //these represent the chance of a given trait mutating within a million years
    public float dietChance, productChance, aggChance, instabChance;

    public void Evolve (Organism organism)
    {
        
    }

    public void EvolveDiet (Organism organism)
    {
        var randVal = Random.value;
        if (randVal <= dietChance * organism.instability) //tests chance of mutation
        {
            var addOrRemove = Random.Range(0, 2) == 0; //determines whether to add or remove resource from diet

            var dietRsrceIndex = Random.Range(0, resourceMngr.diet.Count - 1);

            var resource = resourceMngr.diet[dietRsrceIndex].resource; //randomly chooses resource

            var included = FindResource(organism.diet, resource);

            if (addOrRemove && !included)
            {
                organism.diet.Add(resource); //adds resource
            }
            else if (included)
            {
                organism.diet.RemoveAll(x => x == resource); //removes resource
            }
        }
    }

    public void EvolveProducts (Organism organism)
    {

    }

    public void EvolveAgg (Organism organism)
    {

    }

    public void EvolveStab (Organism organism)
    {

    }

    public bool FindResource (List<Resource> list, Resource type)
    {
        return list.Contains(type);
    }
}
