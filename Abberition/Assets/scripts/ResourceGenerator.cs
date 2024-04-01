using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    public ResourceManager resourceManager;

    public MMA
        TypeAmount,
        CompTypeRatio,
        PolymerDepth,
        //HighestPolymerDepth,
        //AvgPolymerDepth,
        //PolymerDepthConc,
        TypesPerRecipeGroup,
        RecipePortions,
        RenewableRatio,
        DestroyEfficiency
        ;

    static List<int> primeNumbers { get; } = new List<int> { 2, 3, 5, 7, 11, 13, 17, 19 };

    string resourceNameList = "A B C D E F G H I J K L M N O P Q R S T U V W X Y Z";

    List<ResourceType> resourceTypes = new();
    List<ResourceType> renewableTypes = new();
    List<Recipe> constructiveRecipes = new();
    List<Recipe> destructiveRecipes = new();

    public int seed;

    public string dataString = "";

    public void NewSeed ()
    {
        seed = Random.Range(int.MinValue, int.MaxValue);
    }

    public void GenerateResources ()
    {
        if (!resourceManager) resourceManager = FindObjectOfType<ResourceManager>();

        Random.InitState(seed);

        resourceTypes = new();
        renewableTypes = new();
        constructiveRecipes = new();
        destructiveRecipes = new();

        var typeAmount = TypeAmount.GetInt();
        var renewableAmount = Mathf.RoundToInt(RenewableRatio.GetValue() * typeAmount);
        var compTypeAmount = Mathf.RoundToInt(CompTypeRatio.GetValue() * typeAmount);
        //generate typeAmount, renewable amount, and composition type amount

        //recipe portions and types per recipe will be generated for each recipe

        //List<ResourceType> resourceTypes = new();

        var names = resourceNameList.Split(" ");

        for (var i = 0; i < typeAmount; i++)
        {
            resourceTypes.Add(new(names[i]));
        }
        //create resources and assign names

        //choose random types to be renewable

        List<ResourceType> compTypes = ListRandom(compTypeAmount, resourceTypes);

        int biggestDepth = 0;
        for (var i = 0; i < typeAmount; i++)
        {
            var depth = Mathf.Min(PolymerDepth.GetInt(),typeAmount); //gotta clamp this
            if (depth > biggestDepth) biggestDepth = depth;
        }

        //find highest of random depths

        List<int> depthList = new();

        for (var i = 0; i < biggestDepth+1; i++)
        {
            depthList.Add(i);
        }

        var extras = typeAmount - biggestDepth;

        for (var i = 0; i < extras; i++)
        {
            var depth = PolymerDepth.GetInt();
            depthList.Add(depth);
            biggestDepth = Mathf.Max(biggestDepth, depth);
        }

        //get list of depths of for extra types

        //List<Recipe> recipes = new();
        List<ResourceType> notAssignedRecipe = new(resourceTypes);
        List<ResourceType> assignedRecipe = new();
        List<List<ResourceType>> depthGroups = new();

        List<ResourceType> initialDepthGroup = new();
        depthGroups.Add(initialDepthGroup);

        for (var l = 0; l < depthList.Count; l++)
        {
            if (depthList[l] == 0)
            {
                var resourceType = notAssignedRecipe[0];
                notAssignedRecipe.Remove(resourceType);
                assignedRecipe.Add(resourceType);

                initialDepthGroup.Add(resourceType);

                depthList.RemoveAt(l);
                l--;
                //remove to minimize proceeding recursion
            }
        }
        //

        for (var i = 1; i < biggestDepth+1; i++) //for resource types of depth 1 and higher
        {
            List<ResourceType> depthGroup = new();
            depthGroups.Add(depthGroup);

            if (notAssignedRecipe.Count == 0)
            {
                break;
            }

            var lastResourceIndex = assignedRecipe.Count-1;

            var typeAmountI = 0; //how many types will be at this depth
            //starts at one to ensure at least one type per depth

            for (var l = 0; l < depthList.Count;l++)
            {
                if (depthList[l] == i)
                {
                    typeAmountI++;
                    depthList.RemoveAt(l);
                    l--;
                    //remove to minimize proceeding recursion
                }
            }
            //foreach occurance of this depth value, add one to how many are in this depth

            var recipeAmount = Mathf.CeilToInt(typeAmountI / (TypesPerRecipeGroup.max+1)); //amount of recipes for this depth
            //adds 1 to ciel

            for (var l = 0; l < recipeAmount; l++)
            {
                var recipe = new Recipe();
                List<Resource> products = recipe.products = new();

                var productAmount = TypesPerRecipeGroup.GetInt();
                var ingredientAmount = TypesPerRecipeGroup.GetInt();

                List<int> portions = new();

                for (var p = 0; p < productAmount+ingredientAmount; p++)
                {
                    portions.Add(RecipePortions.GetInt());
                }
                //generate list of ints for portions of resources

                SimplifyValues(portions);
                //simplify portions, like a chemistry equation

                for (var p = 0; p < productAmount; p++)
                {
                    if (notAssignedRecipe.Count != 0)
                    {

                        var resourceType = notAssignedRecipe[0];
                        var amount = portions[0];

                        products.Add(new(resourceType, amount));

                        notAssignedRecipe.Remove(resourceType);
                        assignedRecipe.Add(resourceType);
                        depthGroup.Add(resourceType);

                        portions.RemoveAt(0);
                    }
                    else break;
                }
                //add products

                List<Resource> ingredients = recipe.ingredients = new();

                var lesserDepthGroup = depthGroups[i - 1];
                var lesserDepthTypeIndex = Random.Range(0, lesserDepthGroup.Count);
                var lesserDepthType = lesserDepthGroup[lesserDepthTypeIndex];
                var depthTypeResourceIndex = resourceTypes.IndexOf(lesserDepthType);

                //ensures at least one type of neighboring depth is used to ensure recipe depth

                var uniqueListCount = Mathf.Min(ingredientAmount, lastResourceIndex+1);
                var uniqueIntList = GetUniqueInts(0, lastResourceIndex,uniqueListCount);

                //get unique int list for unique resource types

                if (!uniqueIntList.Contains(depthTypeResourceIndex))
                    uniqueIntList[0] = depthTypeResourceIndex;

                //if the list does not include the designated depth type index, replace first of list

                for (var p = 0; p < uniqueIntList.Count; p++)
                {
                    var typeIndex = uniqueIntList[p];
                    var resourceType = resourceTypes[typeIndex];

                    var amount = portions[0];
                    portions.RemoveAt(0);

                    ingredients.Add(new(resourceType, amount));
                }

                //add unique resources

                //!!THIS SYSTEM DOES NOT GUARANTEE THE FOLLOWING:
                //THAT EVERY RESOURCE IS USED
                //THAT THERE ARE NO DUPLICATE INGREDIENT GROUPS

                constructiveRecipes.Add(recipe);

                if (i+1 >= biggestDepth && l+1 >= recipeAmount && notAssignedRecipe.Count > 0)
                {
                    recipeAmount++;
                }
            }
        }
        //resourceManager.resourceTypes = resourceTypes;
        //resourceManager.recipes = recipes;

        //generate destructive recipes...

        renewableTypes = ListRandom(renewableAmount, resourceTypes);

        foreach (var type in renewableTypes)
        {
            type.renewable = true;
        }

        var recipeCount = constructiveRecipes.Count;

        for (var i = 0; i < recipeCount; i++)
        {
            var ogRecipe = constructiveRecipes[i];
            var newRecipe = new Recipe();
            newRecipe.ingredients = new(ogRecipe.products);
            newRecipe.products = new(ogRecipe.ingredients);
            destructiveRecipes.Add(newRecipe);

            foreach (var resource in newRecipe.products)
            {
                if (renewableTypes.Contains(resource.type))
                {
                    var efficiency = DestroyEfficiency.GetValue();
                    var amount = Mathf.RoundToInt(resource.amount * (1-efficiency));
                    if (amount > 0)
                        newRecipe.ingredients.Add(new(resource.type, amount));
                }
            }
        }
    }

    public string WriteData()
    {
        string output = $"Resources ({resourceTypes.Count}): ";

        foreach (var type in resourceTypes)
        {
            output += type.name;
            if (type != resourceTypes[^1]) output += ", ";
        }

        output += $"\nRenewable Resources ({renewableTypes.Count}): ";

        foreach (var type in renewableTypes)
        {
            output += type.name;
            if (type != renewableTypes[^1]) output += ", ";
        }

        output += $"\nConstructive Recipes ({constructiveRecipes.Count}):\n";

        foreach (var recipe in constructiveRecipes)
        {
            output += recipe.ReadRecipe();
            if (recipe != constructiveRecipes[^1]) output += "\n";
        }

        output += $"\nDestructive Recipes ({constructiveRecipes.Count}):\n";

        foreach (var recipe in destructiveRecipes)
        {
            output += recipe.ReadRecipe();
            if (recipe != destructiveRecipes[^1]) output += "\n";
        }

        dataString = output;

        return output;
    }

    public static int SimplifyValues (List<int> values) //edits the values of the list
    {
        var commonPrime = 0;

        for (var i = 0; i < primeNumbers.Count; i++)
        {
            bool isCommon = true;

            var primeNumber = primeNumbers[i];

            for (var l = 0; l < values.Count; l++)
            {
                var value = values[l];

                if (value % primeNumber != 0)
                {
                    isCommon = false;
                    break;
                }
            }

            if (isCommon) commonPrime = primeNumber;
        }

        if (commonPrime != 0)
        {
            for (var i = 0; i < values.Count; i++)
            {
                values[i] = values[i] / commonPrime;
            }

            SimplifyValues(values);
        }

        return commonPrime;
    }

    public static List<int> GetUniqueInts (int min, int max, int count)
    {
        List<int> intList = new();
        
        for (var i = min; i < max+1; i++)
        {
            intList.Add(i);
        }

        return ListRandom(count, intList);
    }

    public static List<T> ListRandom <T> (int count, List<T> list)
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
            var resource = notIncluded[randIndex];
            output.Add(resource);
            notIncluded.Remove(resource);
        }

        return output;
    }
}
