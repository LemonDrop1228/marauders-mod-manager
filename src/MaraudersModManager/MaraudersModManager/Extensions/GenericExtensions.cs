using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MaraudersModManager.Extensions;

public static class GenericExtensions
{
    public static string ToJson(this object obj) => Newtonsoft.Json.JsonConvert.SerializeObject(obj);
    public static T FromJson<T>(this string json) => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
    public static bool In<T>(this object value, params T[] comparisonArray) => comparisonArray.Contains((T) value);
    
    
    
    public static IEnumerable<Type> GetTypesAssignableFrom<T1, T2>(this Assembly assembly)
    {
        return assembly.GetTypesAssignableFrom(typeof(T1), typeof(T2));
    }

    public static IEnumerable<Type> GetTypesAssignableFrom(this Assembly assembly, Type compareType, Type excludeType)
    {
        List<Type> ret = new List<Type>();
        foreach (var type in assembly.DefinedTypes)
        {
            if (compareType.IsAssignableFrom(type) && compareType != type && excludeType != type)
            {
                ret.Add(type);
            }
        }
        return ret;
    }
}