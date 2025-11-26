using System.Collections.Generic;
using UnityEngine;

public static class GameObjectPoolService
{
    private static readonly Dictionary<string, Queue<GameObject>> _pools = new();
    private static readonly object _lock = new();

    /// <summary>
    /// Пытается получить объект из пула по ключу.
    /// </summary>
    public static bool TryGet(string key, out GameObject obj)
    {
        obj = null;

        if (key == null)
            return false;

        lock (_lock)
        {
            if (!_pools.TryGetValue(key, out var queue) || queue.Count == 0)
                return false;

            obj = queue.Dequeue();
            obj.SetActive(true);
            return true;
        }
    }

    /// <summary>
    /// Возвращает объект обратно в пул.
    /// </summary>
    public static void Release(string key, GameObject obj)
    {
        if (key == null || obj == null)
            return;

        lock (_lock)
        {
            if (!_pools.TryGetValue(key, out var queue))
            {
                queue = new Queue<GameObject>();
                _pools[key] = queue;
            }

            obj.SetActive(false);
            queue.Enqueue(obj);
        }
    }

    /// <summary>
    /// Есть ли в пуле объекты по ключу?
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
    /// Количество объектов в пуле.
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
            {
                while (q.Count > 0)
                {
                    var obj = q.Dequeue();
                    if (obj != null)
                        Object.Destroy(obj);
                }
            }
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
            {
                while (queue.Count > 0)
                {
                    var obj = queue.Dequeue();
                    if (obj != null)
                        Object.Destroy(obj);
                }
            }
        }
    }
}
