using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public List<Resource> finiteResources = new(); //per step
    //gotta make sure this is distributed correctly
    public List<Resource> renewableResources = new();
    public List<Organism> inhabitants = new();
}
