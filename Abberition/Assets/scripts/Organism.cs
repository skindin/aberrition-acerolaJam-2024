using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Organism
{
    public string name, code; //display code if name is empty
    public List<Organism> lineage = new();
    public List<Organism> offspring = new();
    public MutatableTrait aggression, instability;
    //agression determines how much resources they get

    public List<Recipe> recipes = new();
    public List<Resource> comp = new(); //resources this organism is made of
    public List<Resource> diet = new(); //resources this organism needs

    public Organism(Organism parent = null)
    {
        if (parent != null)
        {
            lineage = new(parent.lineage);
            lineage.Add(parent);
            SetCode();
            aggression = parent.aggression;
            instability = parent.instability;
        }
    }

    public Organism GetOffspring ()
    {
        return new Organism(this);
    }

    public void SetCode ()
    {
        var parent = lineage[^1];
        var place = parent.offspring.IndexOf(this)+1;
        code = $"{parent.name}.{place}";
    }

    public void SetName (string newName)
    {
        name = newName;

        foreach (var a in offspring)
        {
            a.SetCode();
        }
    }

    public void MutateTraits ()
    {
        aggression.Mutate(instability.value);
        instability.Mutate(instability.value);
    }

    public void DetermineDiet ()
    {
        foreach (var resource in comp)
        {
            recipes.Find(x => x.products.Any(x => x.type == resource.type));
        }        
    }

    public Recipe FindRecipe (ResourceType type)
    {
        return null;
    }
}
