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

    static List<int> primeNumbers { get; } = new List<int> { 2, 3, 5, 7, 11, 13, 17, 19 };

    string resourceNameList = "A B C D E F G H I J K L M N O P Q R S T U V W X Y Z";

    List<ResourceType> resourceTypes = new();
    List<Recipe> recipes = new();

    public int seed;

    public string dataString = "";

    public void NewSeed ()
    {
        seed = Random.Range(0, 1000000000);
    }

    public void GenerateResources ()
    {
        if (!resourceManager) resourceManager = FindObjectOfType<ResourceManager>();

        Random.InitState(seed);

        resourceTypes = new();
        recipes = new();

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

        List<ResourceType> renewableTypes = ListRandom(renewableAmount, resourceTypes);

        foreach (var type in renewableTypes)
        {
            type.renewable = true;
        }

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

        for (var l = 0; l < depthList.Count; l++)
        {
            if (depthList[l] == 0)
            {
                var resourceType = notAssignedRecipe[0];
                notAssignedRecipe.Remove(resourceType);
                assignedRecipe.Add(resourceType);

                depthList.RemoveAt(l);
                l--;
                //remove to minimize proceeding recursion
            }
        }
        //

        for (var i = 1; i < biggestDepth+1; i++) //for resource types of depth 1 and higher
        {
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

                        portions.RemoveAt(0);
                    }
                    else break;
                }
                //add products

                List<Resource> ingredients = recipe.ingredients = new();

                var uniqueListCount = Mathf.Min(ingredientAmount, lastResourceIndex);
                var uniqueIntList = GetUniqueInts(0, lastResourceIndex,uniqueListCount);
                //get unique int list for unique resource types

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
                //THAT A RESOURCE OF A GIVEN DEPTH USES ATLEAST ONE RESOURCE OF THE NEIGHBORING LESSER DEPTH

                recipes.Add(recipe);

                if (i+1 >= biggestDepth && l+1 >= recipeAmount && notAssignedRecipe.Count > 0)
                {
                    recipeAmount++;
                }
            }
        }

        Mathf.FloatToHalf(1);
        
        //resourceManager.resourceTypes = resourceTypes;
        //resourceManager.recipes = recipes;
    }

    public string WriteData()
    {
        string output = $"Resources ({resourceTypes.Count}): ";

        foreach (var type in resourceTypes)
        {
            output += type.name;
            if (type != resourceTypes[^1]) output += ", ";
        }

        output += $"\nRecipes ({recipes.Count}):\n";

        foreach (var recipe in recipes)
        {
            output += recipe.ReadRecipe();
            if (recipe != recipes[^1]) output += "\n";
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

    public static List<int> GetUniqueInts (int min, int max, int count, int loopLimit = 100)
    {
        List<int> output = new();

        var maxUnique = max - min;

        for (var i = 0; i < loopLimit; i++) //put a limit because frik while loops
        {
            var randVal = Random.Range(min, max + 1);
            if (!output.Contains(randVal)) output.Add(randVal);

            if (output.Count >= count) break;
            if (count > maxUnique && output.Count >= maxUnique)
            {
                Debug.Log($"There was only {maxUnique} unique values possible!");
                break;
            }
            // in case the user wants more unique int values than possible...
        }

        return output;
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
            var resource = list[randIndex];
            output.Add(resource);
            notIncluded.Remove(resource);
        }

        return output;
    }
}
