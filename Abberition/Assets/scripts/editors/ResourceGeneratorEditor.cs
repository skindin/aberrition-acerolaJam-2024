using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(ResourceGenerator))]
public class ResourceGeneratorEditor : Editor
{
    public ResourceGenerator generator;
    public string output = "";
    public float minMin = 0, maxMax = 20;

    public override void OnInspectorGUI()
    {
        if (!generator) generator = (ResourceGenerator)target;

        generator.seed = EditorGUILayout.IntField("Seed", generator.seed);

        if (GUILayout.Button("New Seed"))
        {
            generator.NewSeed();
            generator.GenerateResources();
            generator.WriteData();
        }

        if (GUILayout.Button("Generate Resources"))
        {
            generator.GenerateResources();
            generator.WriteData();
        }

        EditorGUILayout.TextArea(generator.dataString);

        GUILayout.Space(20);

        EditorGUILayout.LabelField("Type Amount");
        DrawMMA(ref generator.TypeAmount);

        EditorGUILayout.LabelField("Renewable Ratio");
        DrawMMA(ref generator.RenewableRatio);

        EditorGUILayout.LabelField("Component Type Ratio");
        DrawMMA(ref generator.CompTypeRatio);

        //EditorGUILayout.LabelField("Highest Polymer Depth");
        //DrawMMA(ref generator.HighestPolymerDepth);

        //EditorGUILayout.LabelField("Average Polymer Depth");
        //DrawMMA(ref generator.AvgPolymerDepth);

        //EditorGUILayout.LabelField("Polymer Depth Concentration");
        //DrawMMA(ref generator.PolymerDepthConc);

        EditorGUILayout.LabelField("Polymer Depth");
        DrawMMA(ref generator.PolymerDepth);

        EditorGUILayout.LabelField("Types per Recipe Group");
        DrawMMA(ref generator.TypesPerRecipeGroup);

        EditorGUILayout.LabelField("Recipe Portions");
        DrawMMA(ref generator.RecipePortions);

        if (GUI.changed && !Application.isPlaying)
        {
            EditorUtility.SetDirty(generator);
            EditorSceneManager.MarkSceneDirty(generator.gameObject.scene);
        }
    }

    public void DrawMMA (ref MMA mma)
    {
        EditorGUI.indentLevel++;
        mma.dynamic = EditorGUILayout.Toggle("Dynamic", mma.dynamic);
        if (mma.dynamic)
        {
            mma.intValues = EditorGUILayout.Toggle("Int Values", mma.intValues);

            if (mma.intValues)
            {
                mma.min = Mathf.RoundToInt(mma.min);
                mma.max = Mathf.RoundToInt(mma.max);
            }

            EditorGUILayout.MinMaxSlider("Min: " + mma.min + " Max: " + mma.max, ref mma.min, ref mma.max, minMin, maxMax);
            mma.avg = EditorGUILayout.Slider("Average", mma.avg, mma.min, mma.max);
            mma.conc = EditorGUILayout.Slider("Concentration", mma.conc, 0, 1);
        }
        else
        {
            mma.fixedVal = EditorGUILayout.Slider("Fixed Value", mma.fixedVal, minMin, maxMax);
        }
        EditorGUI.indentLevel--;
    }
}
