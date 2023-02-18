namespace MaraudersModManager.Extensions;

public static class GenericExtensions
{
    public static string ToJson(this object obj) => Newtonsoft.Json.JsonConvert.SerializeObject(obj);
    public static T FromJson<T>(this string json) => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
}