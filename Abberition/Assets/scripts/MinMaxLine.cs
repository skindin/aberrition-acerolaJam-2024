using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MinMaxLine
{
    public float min, max;

    public float GetValue (float input)
    {
        return ((max - min) * input) + min;
    }
}
