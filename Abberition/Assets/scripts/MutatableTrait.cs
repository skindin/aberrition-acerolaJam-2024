using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MutatableTrait
{
    float input;
    public float chance, value, min, max; 
    //waste of memory to have these values stored on every single organism

    float GetValue()
    {
        return Mathf.Clamp(input, min, max);
    }

    float UpdateAndGet (float increase)
    {
        input += increase;
        value = GetValue();
        return value;
    }

    public void Mutate (float coeff)
    {
        if (Random.value > chance * coeff) return;

        var intensity = Random.Range(0, 1);
        if (intensity == 0) intensity = -1;

        UpdateAndGet(intensity * coeff);
    }
}
