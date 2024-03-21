using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PolymerType : ResourceType
{
    public List<Resource> resources = new();
    public int yieldAmount;

    //gotta think of some way to prevent product resources from refferencing themselves

    public bool CheckForParadox ()
    {
        bool containsPolymer = false;
        bool containsParadox = false;

        foreach (var resource in resources)
        {
            if (resource.type is PolymerType)
            {
                if (resource.type == this)
                {
                    containsParadox = true;
                    break;
                }
                containsPolymer = true;
            }
        }

        if (containsParadox) return true;
        if (!containsPolymer) return false;

        if (!CheckForSelf(this, 0)) containsParadox = false;
        return containsParadox;
    }

    public static bool CheckForSelf (PolymerType polymer, int depth)
    {
        if (depth > 10) //this chunk is just to prevent crashes in case i did something wrong in this script
        {
            Debug.LogError("Max depth reached!");
            return true;
        }
        depth++;

        bool containsSelf = polymer.resources.Any(x => (PolymerType)x.type == polymer);
        if (containsSelf)
        {
            return true;
        }
        else
        {
            foreach (var resource in polymer.resources)
            {
                if (resource.type is PolymerType && !CheckForSelf((PolymerType)resource.type, depth))
                {
                    return false;
                }
            }
        }

        return false;
    }
}