using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Recipe
{
    public List<Resource> groupA = new();
    public List<Resource> groupB = new();

    public float aToBChance, bToAChance, bothChance;
    //these are the chance of a species unlocking this recipe

    public enum RecipeType
    {
        aToB,
        bToA,
        both
    }

    public bool Check (bool log = false)
    {
        var logText = "";

        List<Resource> rawA = new();
        foreach (var resource in groupA)
        {
            GetRaw(resource, rawA, 1);
        }
        logText += ReadResourceList(groupA) + " -> " + ReadResourceList(rawA) + "\n";

        List<Resource> rawB = new();
        foreach (var resource in groupA)
        {
            GetRaw(resource, rawB, 1);
        }
        logText += ReadResourceList(groupB) + " -> " + ReadResourceList(rawB) + "\n";

        bool identical = TestIdentical(rawA, rawB);

        if (identical) logText += "Recipes Match!";
        else logText += "Invalid Recipe!";

        if (log) Debug.Log(logText);

        return identical;
    }

    public static List<Resource> GetRaw (Resource resource, List<Resource> rawList, int factor)
    {
        if (resource.type is PolymerType polymer)
        {
            foreach (var a in polymer.resources)
            {
                var yieldFactor = 1;

                foreach (var b in polymer.resources)
                {
                    if (b != a && b.type is PolymerType polymerB)
                    {
                        yieldFactor *= polymerB.yieldAmount;
                    }
                }
                GetRaw(a, rawList, factor * yieldFactor * a.amount);
            }
        }
        else
        {
            AddResource(resource, rawList, factor);
        }

        return rawList;
    }

    public static void AddResource (Resource resource, List<Resource> list, int factor)
    {
        foreach (var a in list)
        {
            if (resource.type == a.type)
            {
                a.amount += resource.amount * factor;
                return;
            }
        }

        list.Add(new Resource(resource.type, resource.amount * factor));
    }

    public static bool TestIdentical (List<Resource> a, List<Resource> b)
    {
        if (a.Count != b.Count) return false;

        bool missingResource = false;

        foreach (var resource in a)
        {
            var found = b.FirstOrDefault(x => x.type == resource.type);
            if (found == null || resource.amount != found.amount) missingResource = true;
        }

        return !missingResource;
    }

    public string ReadResourceList (List<Resource> resources)
    {
        string output = "";

        foreach (var resource in resources)
        {
            output += resource.type.name + "x" + resource.amount + " ";
        }

        return output;
    }
}
