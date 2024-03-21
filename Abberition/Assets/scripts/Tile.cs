using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public List<Resource> finiteResources = new();
    public List<Resource> indefinite = new();
    public List<Organism> inhabitants = new();
}
