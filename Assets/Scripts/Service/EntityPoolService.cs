using System.Collections.Generic;

public static class EntityPoolService
{
    private static readonly Dictionary<string, Queue<int>> _pools = new();
    private static readonly object _lock = new();

    /// <summary>
    /// Пытается получить сущность из пула по ключу.
    /// </summary>
    public static bool TryGet(string key, out int entity)
    {
        entity = -1;

        if (key == null)
            return false;

        lock (_lock)
        {
            if (!_pools.TryGetValue(key, out var queue) || queue.Count == 0)
                return false;

            entity = queue.Dequeue();
            return true;
        }
    }

    /// <summary>
    /// Возвращает сущность обратно в пул.
    /// </summary>
    public static void Release(string key, int entity)
    {
        if (key == null)
            return;

        lock (_lock)
        {
            if (!_pools.TryGetValue(key, out var queue))
            {
                queue = new Queue<int>();
                _pools[key] = queue;
            }

            queue.Enqueue(entity);
        }
    }

    /// <summary>
    /// Есть ли в пуле сущности по ключу?
    /// </summary>
    public static bool Has(string key)
    {
        if (key == null)
            return false;

        lock (_lock)
        {
            return _pools.TryGetValue(key, out var q) && q.Count > 0;
        }
    }

    /// <summary>
    /// Количество сущностей в пуле.
    /// </summary>
    public static int Count(string key)
    {
        if (key == null)
            return 0;

        lock (_lock)
        {
            return _pools.TryGetValue(key, out var q) ? q.Count : 0;
        }
    }

    /// <summary>
    /// Полная очистка одного пула.
    /// </summary>
    public static void Clear(string key)
    {
        if (key == null)
            return;

        lock (_lock)
        {
            if (_pools.TryGetValue(key, out var q))
                q.Clear();
        }
    }

    /// <summary>
    /// Полная очистка всех пулов.
    /// </summary>
    public static void ClearAll()
    {
        lock (_lock)
        {
            foreach (var queue in _pools.Values)
                queue.Clear();
        }
    }
}
