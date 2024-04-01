using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public List<Tile> tiles = new();
    OrganismManager organismMngr;
    ResourceManager resourceMngr;

    public TimeValue stepLength;

    public void Step (float time)
    {
        var step = stepLength.GetSeconds();
        var steps = time / step;
    }
}
