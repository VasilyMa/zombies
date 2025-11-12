using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelBase", menuName = "Levels/LevelBase")]
public class LevelBase : ScriptableObject
{
    [SerializeField][Header("Основные параметры")]
    private float SpawnInterval = 2f;
    [SerializeField]
    private int SpawnCount = 5; 
    [SerializeField]
    [Header("Контейнеры врагов по времени")]
    private List<EnemyContainer> EnemyContainers;
     
    [Header("Длительность матча")]
    [Tooltip("Максимальная длительность матча в секундах")]
    public float MaxMatchDuration = 180f;

    [SerializeField]
    [Header("Кривые сложности")]
    [Tooltip("Кривая для изменения интервала спавна в зависимости от времени (0 - начало, 1 - конец матча)")]
    private AnimationCurve SpawnIntervalCurve = AnimationCurve.Linear(0, 1, 1, 0.5f);

    [SerializeField]
    [Tooltip("Кривая для изменения количества врагов за спавн (0 - начало, 1 - конец матча)")]
    private AnimationCurve EnemiesPerSpawnCurve = AnimationCurve.Linear(0, 1, 1, 3);
    
    [SerializeField]
    [Tooltip("Кривая для влияния веса врага на вероятность появления (0 - начало, 1 - конец матча)")]
    private AnimationCurve EnemyWeightCurve = AnimationCurve.Linear(0, 1, 1, 1);
    
    [SerializeField]
    [Header("Кривая усиления врагов")]
    [Tooltip("Кривая усиления врагов (0 - старт, 1 - конец матча). Значение умножается на параметры врага.")]
    private AnimationCurve EnemyPowerUpCurve = AnimationCurve.Linear(0, 1, 1, 2);

    /// <summary>
    /// Получить интервал спавна на текущий момент матча (timeNormalized: 0..1)
    /// </summary>
    public float GetCurrentSpawnInterval(float timeNormalized)
    {
        float curveValue = SpawnIntervalCurve.Evaluate(timeNormalized);
        return SpawnInterval * curveValue;
    }

    /// <summary>
    /// Получить количество врагов за спавн на текущий момент матча (timeNormalized: 0..1)
    /// </summary>
    public int GetCurrentEnemiesPerSpawn(float timeNormalized)
    {
        // Кривая задаёт коэффициент (0..1), который масштабирует максимальное количество врагов
        float curveValue = Mathf.Clamp01(EnemiesPerSpawnCurve.Evaluate(timeNormalized));
        int result = Mathf.RoundToInt(SpawnCount * curveValue);
        return Mathf.Max(1, result);
    }

    /// <summary>
    /// Получить EnemyContainer для текущего времени матча (currentTimeSeconds)
    /// </summary>
    public EnemyContainer GetCurrentEnemyContainer(float currentTimeSeconds)
    {
        if (EnemyContainers == null || EnemyContainers.Count == 0)
            return null;

        // Сортируем контейнеры по StartTimeSeconds
        EnemyContainers.Sort((a, b) => a.StartTimeSeconds.CompareTo(b.StartTimeSeconds));

        // Находим подходящий контейнер
        EnemyContainer selected = EnemyContainers[0];

        foreach (var container in EnemyContainers)
        {
            if (currentTimeSeconds >= container.StartTimeSeconds)
                selected = container;
            else
                break;
        }
        return selected;
    }

    /// <summary>
    /// Получить множитель усиления врагов на текущий момент матча (timeNormalized: 0..1)
    /// </summary>
    public float GetCurrentEnemyPowerUp(float timeNormalized)
    {
        return EnemyPowerUpCurve.Evaluate(timeNormalized);
    }
}

[System.Serializable]
public class EnemyContainer
{
    [Tooltip("Время начала действия контейнера в секундах с начала матча")]
    public float StartTimeSeconds = 0f;

    [Tooltip("Список врагов для этого контейнера")]
    public List<EnemyBase> Enemies;

    /// <summary>
    /// Получить случайного врага из контейнера с учетом веса.
    /// </summary>
    public EnemyBase GetRandomEnemy()
    {
        if (Enemies == null || Enemies.Count == 0)
            return null;

        float totalWeight = 0f;
        foreach (var enemy in Enemies)
            totalWeight += Mathf.Max(0f, enemy.Weight);

        if (totalWeight <= 0f)
            return Enemies[Random.Range(0, Enemies.Count)];

        float randomValue = Random.value * totalWeight;
        float cumulative = 0f;
        foreach (var enemy in Enemies)
        {
            cumulative += Mathf.Max(0f, enemy.Weight);
            if (randomValue <= cumulative)
                return enemy;
        }
        return Enemies[Enemies.Count - 1];
    }
}