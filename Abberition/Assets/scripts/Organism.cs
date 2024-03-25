using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Organism
{
    public string name;
    public List<string> lineage;
    public float aggression;
    public float instability;

    public List<Recipe> recipes = new();
    public List<Resource> comp = new(); //resources this organism is made of
}
