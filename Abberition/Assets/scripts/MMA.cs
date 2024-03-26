using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MMA
{
    public bool dynamic, intValues;
    public float fixedVal, min, max, avg, conc;

    public float GetValue(float input = default)
    {
        if (!dynamic) return fixedVal;

        if (input == default) input = Random.value;

        max = Mathf.Max(max, min);
        avg = Mathf.Clamp(avg, min, max);

        var linear = input * (max - min) + min;
        var warpedValue = linear + ((avg - linear) * Mathf.Pow(conc, 2));

        //if (intValues) warpedValue = Mathf.Round(warpedValue);

        return warpedValue;
    }

    public int GetInt (float input = default)
    {
        return Mathf.RoundToInt(GetValue(input));
    }
}
