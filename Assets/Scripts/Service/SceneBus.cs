using System.Collections.Generic;
using UnityEngine;

public static class SceneBus
{
    private static readonly Dictionary<string, object> _data = new();

    public static void Set<T>(T data)
    {
        var key = typeof(T).FullName;
        _data[key] = data;
    }

    public static bool TryGet<T>(out T data)
    {
        var key = typeof(T).FullName;
        if (_data.TryGetValue(key, out var value) && value is T casted)
        {
            data = casted;
            _data.Remove(key); // одноразово Ч как Event
            return true;
        }

        data = default;
        return false;
    }
}
