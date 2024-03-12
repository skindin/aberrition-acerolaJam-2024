using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Organism
{
    public float aggression;
    public float instability;

    public List<Resource> diet;
    public List<Resource> products;
}
