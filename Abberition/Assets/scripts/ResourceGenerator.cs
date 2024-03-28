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
        PolymerDepth,
        //HighestPolymerDepth,
        //AvgPolymerDepth,
        //PolymerDepthConc,
        TypesPerRecipeGroup,
        RecipePortions
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

        List<ResourceType> resourceTypes = new();

        var names = resourceNameList.Split(" ");

        for (var i = 0; i < typeAmount; i++)
        {
            resourceTypes.Add(new(names[i]));
        }
        //create resources and assign names

        List<ResourceType> renewableTypes = ListRandom(renewableAmount, resourceTypes);
        //choose random types to be renewable

        List<ResourceType> compTypes = ListRandom(compTypeAmount, resourceTypes);

        int biggestDepth = 0;

        for (var i = 0; i < typeAmount; i++)
        {
            var depth = PolymerDepth.GetInt();
            if (depth > biggestDepth) biggestDepth = depth;
        }

        List<int> depthList = new();

        for (var i = 0; i < typeAmount-biggestDepth; i++)
        {
            depthList.Add(PolymerDepth.GetInt());
        }

        List<Recipe> recipes = new();
        List<ResourceType> notAssignedRecipe = new(resourceTypes);
        List<ResourceType> assignedRecipe = new();

        for (var i = 0; i < biggestDepth; i++)
        {
            var typeAmountI = 0;
            for (var l = 0; l < depthList.Count;l++)
            {
                if (l == i) typeAmountI++;
            }

            var recipeAmount = typeAmountI / TypesPerRecipeGroup.GetInt()+1;
            //adds 1 to ciel

            for (var l = 0; l < recipeAmount; l++)
            {
                var recipe = new Recipe();
                List<Resource> products = recipe.products = new();

                var productAmount = TypesPerRecipeGroup.GetInt();

                for (var p = 0; p < typeAmount; p++)
                {
                    var resourceType = notAssignedRecipe[0];
                    notAssignedRecipe.Remove(resourceType);
                    assignedRecipe.Add(resourceType);

                    products.Add(new())
                }

                List<Resource> ingredients = recipe.ingredients = new();

                recipes.Add(recipe);
            }
        }

        //choose random types to be available for organism composition

        //resources created and sorted into renewable and composition types

        //var highestPolymerDepth = HighestPolymerDepth.GetInt();
        //var avgPolymerDepth = Mathf.Min(HighestPolymerDepth.GetInt(), highestPolymerDepth);
        //var polymerDepthConc = PolymerDepthConc.GetValue();
        //polymerDepth.dynamic = true;
        //polymerDepth.min = 0;
        //polymerDepth.max = highestPolymerDepth;
        //polymerDepth.avg = avgPolymerDepth;
        //polymerDepth.conc = polymerDepthConc;
        ////setup polymer depth min, max, avg, and concentration

        //List<ResourceType> notAssignedDepth = new(types);
        ////resources that haven't been assigned a depth

        //List<List<ResourceType>> polymerDepths = new(); 
        ////list of lists of types.
        ////element zero contains all elements of depth 0

        //List<int> depthCount = new();

        //for (var i = 0; i < typeAmount-highestPolymerDepth; i++)
        //{
        //    depthCount.Add(polymerDepth.GetInt()); //designate an amount of types to each depth
        //}

        //for (var i = 0; i < highestPolymerDepth; i++)
        //{
        //    List<ResourceType> polymerList = new();
        //    //for each depth, create a list of polymers


        //    for (var l = 0; l < depthCount[i]+1; l++)
        //    {
        //        var polymer = types[l];
        //        polymerDepths.Add(polymerList);
        //        notAssignedDepth.Remove(polymer);
        //    }
        //    //and populate the list with designated amount
        //}

        ////all resources have been assigned a depth, now we must decide what it takes to make them

        //List<Recipe> recipes = new();

        //List<List<ResourceType>> notAssignedRecipes = new();
        //foreach (var depth in polymerDepths)
        //{
        //    notAssignedRecipes.Add(new(depth));
        //}

        //for (var i = 1; i < polymerDepths.Count; i++)
        //{
        //    var depthGroup = polymerDepths[i];

        //    MMA RecipeAmount = new();

        //    RecipeAmount.dynamic = true;
        //    RecipeAmount.min = Mathf.CeilToInt(depthGroup.Count / TypesPerRecipeGroup.max);
        //    RecipeAmount.max = Mathf.CeilToInt(depthGroup.Count / TypesPerRecipeGroup.min);
        //    RecipeAmount.avg = Mathf.CeilToInt(depthGroup.Count / TypesPerRecipeGroup.avg);
        //    RecipeAmount.conc = TypesPerRecipeGroup.conc;

        //    var recipeCount = RecipeAmount.GetInt();

        //    for (var l = 0; l < recipeCount; l++)
        //    {
        //        List<Resource> ingredients = new();
        //        List<Resource> products = new();
        //        Recipe recipe = new();
        //        recipe.ingredients = ingredients;
        //        recipe.products = products;

        //        var productCount = TypesPerRecipeGroup.GetInt();
        //        productCount = Mathf.Min(productCount, depthGroup.Count);
        //        //limit product count to amount of resources left within depth
        //        for (var p = 0; p < productCount; p++)
        //        {
        //            var type = notAssignedRecipes[0][0];
        //            notAssignedRecipes[0].Remove(type);
        //            var amount = RecipePortions.GetInt();
        //            products.Add(new(type, amount));
        //        }

        //        if (depthGroup.Count < 1) polymerDepths.Remove(depthGroup);
        //        //if there is no more resources designated this depth, remove this depth from 

        //        var ingredientCount = TypesPerRecipeGroup.GetInt();
        //        for (var p = 0; p < ingredientCount; p++)
        //        {
        //            //must include at least one type of depth decreased by 1
        //            //may include any other types of decreased depth
        //            //must be different from every other ingredientGroup
        //        }
        //    }
        //}
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
