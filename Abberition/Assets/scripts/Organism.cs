using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Organism
{
    public string name, code; //display code if name is empty
    public List<Organism> lineage = new();
    public List<Organism> offspring = new();
    public MutatableTrait aggression, instability; //never gets to 0

    public List<Recipe> recipes = new();
    public List<Resource> comp = new(); //resources this organism is made of

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
}
