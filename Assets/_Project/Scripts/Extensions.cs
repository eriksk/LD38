

using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static IEnumerable<Transform> Children(this Transform t)
    {
        for(var i = 0; i < t.childCount; i++)
            yield return t.GetChild(i);
    }
}