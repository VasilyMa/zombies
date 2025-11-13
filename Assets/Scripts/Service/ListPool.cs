using System.Collections.Generic;

public static class ListPool<T>
{
    private static readonly Stack<List<T>> _pool = new Stack<List<T>>(128);

    /// <summary>
    /// Получить пустой список из пула.
    /// </summary>
    public static List<T> Get()
    {
        if (_pool.Count > 0)
            return _pool.Pop();

        return new List<T>();
    }

    /// <summary>
    /// Вернуть список в пул.
    /// </summary>
    public static void Release(List<T> list)
    {
        list.Clear();
        _pool.Push(list);
    }

    /// <summary>
    /// Очистить пул (например, при смене сцены).
    /// </summary>
    public static void Clear()
    {
        _pool.Clear();
    }
}
