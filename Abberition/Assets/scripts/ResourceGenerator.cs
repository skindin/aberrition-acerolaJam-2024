using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    public ResourceManager resourceManager;

    public MMA TypeAmount, RenewableRatio, RecipePortions, CompTypeRatio, HighestPolymerDepth, AvgPolymerDepth, PolymerDepthConc;
    MMA polymerDepth;

    public string resourceNameList = "A B C D E F G H I J K L M N O P Q R S T U V W X Y Z";

    public MMA test;
    public int results = 10;

    public string GetResults ()
    {
        string output = "";

        for (var i = 0; i < results; i++)
        {
            var randVal = Random.value;
            var value = test.GetValue(randVal);

            output += value;

            if (i != results+1)
            {
                output += ", ";
            }
        }
        return output;
    }

    public void GenerateResources ()
    {
        var typeAmount = TypeAmount.GetInt();
        var renewableAmount = Mathf.RoundToInt(RenewableRatio.GetValue() * typeAmount);
        var compTypeAmount = Mathf.RoundToInt(CompTypeRatio.GetValue() * typeAmount);

        var highestPolymerDepth = HighestPolymerDepth.GetInt();
        var avgPolymerDepth = Mathf.Min(HighestPolymerDepth.GetInt(),highestPolymerDepth);
        var polymerDepthConc = PolymerDepthConc.GetValue();
        polymerDepth.dynamic = true;
        polymerDepth.min = 0;
        polymerDepth.max = highestPolymerDepth;
        polymerDepth.avg = avgPolymerDepth;
        polymerDepth.conc = polymerDepthConc;
        //recipe portions will be generated for each recipe

        List<ResourceType> types = new();

        var names = resourceNameList.Split(" ");

        for (var i = 0; i < typeAmount; i++)
        {
            types.Add(new(names[i]));
        }

        List<ResourceType> renewableTypes = ListRandom(renewableAmount, types);
        List<ResourceType> compTypes = ListRandom(compTypeAmount, types);

        List<List<ResourceType>> polymers = new();

        for (var i = typeAmount-1; polymers.Count < highestPolymerDepth; i--)
        {
        }

        List<Recipe> recipes = new();
    }

    public List<T> ListRandom <T> (int count, List<T> list)
    {
        List<T> output = new();

        if (count > list.Count)
        {
            Debug.LogError("Count was bigger than list");
            return new(list);
        }

        List<T> notIncluded = new(list);
        for (var i = 0; i < count; i++)
        {
            var randIndex = Random.Range(0, notIncluded.Count);
            var resource = list[randIndex];
            output.Add(resource);
            notIncluded.Remove(resource);
        }

        return output;
    }
}
