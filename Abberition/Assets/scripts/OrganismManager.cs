using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganismManager : MonoBehaviour
{
    public List<Organism> organisms = new();
    public Organism original = new();
    public ResourceManager resourceMngr;
    public MinMaxLine agression, instability;

    public void MutateOrganism (Organism organism)
    {
        organism.MutateTraits();
        resourceMngr.MutateOrganismRecipes(organism);
    }

    public Organism GetOffspring (Organism organism)
    {
        var offspring = organism.GetOffspring();
        MutateOrganism(offspring);
        return offspring;
    }
}
