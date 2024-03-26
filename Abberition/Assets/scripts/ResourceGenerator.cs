using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    public ResourceManager resourceManager;

    public MMA
        TypeAmount,
        RenewableRatio,
        CompTypeRatio,
        HighestPolymerDepth,
        AvgPolymerDepth,
        PolymerDepthConc,
        RecipePortions,
        TypesPerRecipeGroup
        ;

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
        //generate typeAmount, renewable amount, and composition type amount

        //recipe portions and types per recipe will be generated for each recipe

        List<ResourceType> types = new();

        var names = resourceNameList.Split(" ");

        for (var i = 0; i < typeAmount; i++)
        {
            types.Add(new(names[i]));
        }
        //create resources and assign names

        List<ResourceType> renewableTypes = ListRandom(renewableAmount, types);
        //choose random types to be renewable

        List<ResourceType> compTypes = ListRandom(compTypeAmount, types);
        //choose random types to be available for organism composition

        //resources created and sorted into renewable and composition types

        var highestPolymerDepth = HighestPolymerDepth.GetInt();
        var avgPolymerDepth = Mathf.Min(HighestPolymerDepth.GetInt(), highestPolymerDepth);
        var polymerDepthConc = PolymerDepthConc.GetValue();
        polymerDepth.dynamic = true;
        polymerDepth.min = 0;
        polymerDepth.max = highestPolymerDepth;
        polymerDepth.avg = avgPolymerDepth;
        polymerDepth.conc = polymerDepthConc;
        //setup polymer depth min, max, avg, and concentration

        List<ResourceType> notAssignedDepth = new(types);
        //resources that haven't been assigned a depth

        List<List<ResourceType>> polymers = new(); 
        //list of lists of types.
        //element zero contains all elements of depth 0

        List<int> depthCount = new();

        for (var i = 0; i < typeAmount-highestPolymerDepth; i++)
        {
            depthCount.Add(polymerDepth.GetInt()); //designate an amount of types to each depth
        }

        for (var i = 0; i < highestPolymerDepth; i++)
        {
            List<ResourceType> polymerList = new();
            //for each depth, create a list of polymers


            for (var l = 0; l < depthCount[i]; l++)
            {
                var polymer = types[l];
                polymers.Add(polymerList);
                notAssignedDepth.Remove(polymer);
            }
            //and populate the list with designated amount
        }

        //all resources have been assigned a depth, now we must decide what it takes to make them

        List<Recipe> recipes = new();

        List<List<ResourceType>> notAssignedRecipes = new(polymers);

        for (var i = 1; i < polymers.Count; i++)
        {
            MMA RecipeAmount = new();

            RecipeAmount.dynamic = true;
            RecipeAmount.min = Mathf.CeilToInt(polymers[i].Count / TypesPerRecipeGroup.max);
            RecipeAmount.max = Mathf.CeilToInt(polymers[i].Count / TypesPerRecipeGroup.min);
            RecipeAmount.avg = Mathf.CeilToInt(polymers[i].Count / TypesPerRecipeGroup.avg);
            RecipeAmount.conc = TypesPerRecipeGroup.conc;

            var recipeCount = RecipeAmount.GetInt();

            //List<int> typeCounts = new();

            for (var l = 0; l < recipeCount; l++)
            {
                //typeCounts.Add(TypesPerRecipeGroup.GetInt());

                List<Resource> ingredients = new();
                List<Resource> products = new();
                Recipe recipe = new();
                recipe.ingredients = ingredients;
                recipe.products = products;

                var productCount = TypesPerRecipeGroup.GetInt();
                for (var p = 0; p < productCount; p++)
                {
                    //add atleast one type of this depth
                    //can only include types of the same depth
                    //must remove these products from the "not assigned" list
                }

                var ingredientCount = TypesPerRecipeGroup.GetInt();
                for (var p = 0; p < ingredientCount; p++)
                {
                    //must include at least one type of depth decreased by 1
                    //may include any other types of decreased depth
                    //must be different from every other ingredientGroup
                }
            }
        }
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
