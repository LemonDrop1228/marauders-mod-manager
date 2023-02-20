using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MaraudersModManager.Extensions;

public static class IEnumerableExtensions
{
    public static void AddRange<T>(this ObservableCollection<T> collection, params T[] items) => items.ForEach(i => collection.Add(i));
    
    public static void AddIfMissing<T>(this ObservableCollection<T> collection, T item)
    {
        if(!collection.Contains(item))
            collection.Add(item);
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (action == null) throw new ArgumentNullException(nameof(action));

        foreach (T item in source)
        {
            action(item);
        }
    }
}