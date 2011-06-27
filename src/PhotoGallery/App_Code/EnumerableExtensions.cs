using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumerableExtensions {

    public static IEnumerable<TSource> Shuffle<TSource>(this IEnumerable<TSource> source) {
        List<TSource> list = source.ToList();
        Random random = new Random();

        for (int i = list.Count - 1; i >= 0; i--) {
            int r = random.Next(i + 1);
            yield return list[r];
            list[r] = list[i];
        }
    }

}
