using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Organism
{
    public float agression;

    public List<Resource> diet;
    public List<Resource> products;
    public List<Resource> composition;
}
