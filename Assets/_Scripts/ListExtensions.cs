using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static List<T> Shuffle<T>(this List<T> list)
    {
        var lastIndex = list.Count;
        while (lastIndex > 1)
        {
            var next = Random.Range(0, lastIndex);
            (list[next], list[lastIndex-1]) = (list[lastIndex-1], list[next]);
            lastIndex--;
        }
    
        return list;
    }

    public static T RandomElement<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
}
