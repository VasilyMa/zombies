using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    sealed class RunPlayerLevelUpSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world;
        readonly EcsSharedInject<BattleState> _state;

        readonly EcsFilterInject<Inc<PlayerLevelUpEvent, WeaponHolderComponent, UpgradeHolderComponent>> _filter = default;

        readonly EcsPoolInject<PlayerLevelUpEvent> _levelUpPool = default;
        readonly EcsPoolInject<WeaponHolderComponent> _weaponHolderPool = default;
        readonly EcsPoolInject<UpgradeHolderComponent> _upgradeHolderPool = default;
        readonly EcsPoolInject<WeaponComponent> _weaponPool = default;
        readonly EcsPoolInject<UpgradeComponent> _upgradePool = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                ref var weaponHolder = ref _weaponHolderPool.Value.Get(entity);
                ref var upgradeHolder = ref _upgradeHolderPool.Value.Get(entity);

                var weaponConfig = ConfigModule.GetConfig<WeaponConfig>();
                var upgradeConfig = ConfigModule.GetConfig<UpgradeConfig>();

                List<IUpgrade> rewardPool = new();
                List<IUpgrade> finalRewardList = new();


                // ============================================================
                // 1) Добавляем НОВЫЕ ОРУЖИЯ (если есть слоты)
                // ============================================================
                if (weaponHolder.WeaponEntities.Count < 3)
                {
                    List<string> exclude = new();

                    foreach (var w in weaponHolder.WeaponEntities)
                    {
                        ref var wc = ref _weaponPool.Value.Get(w);
                        if (wc.Level >= wc.MaxLevel) continue;

                        exclude.Add(wc.Name);
                    }

                    var newWeapons = weaponConfig.GetRandomWeapons(3, exclude.ToArray());
                    rewardPool.AddRange(newWeapons);
                }


                // ============================================================
                // 2) Добавляем НОВЫЕ АПГРЕЙДЫ (если есть слоты)
                // ============================================================
                if (upgradeHolder.UpgradesEntities.Count < 5)
                {
                    List<string> exclude = new();

                    foreach (var u in upgradeHolder.UpgradesEntities)
                    {
                        ref var uc = ref _upgradePool.Value.Get(u); 

                        exclude.Add(uc.KeyName);
                    }

                    var newUpgrades = upgradeConfig.GetRandomUpgrades(3, exclude.ToArray());
                    rewardPool.AddRange(newUpgrades);
                }


                // ============================================================
                // 3) Если меньше 3 — добавляем существующие оружия
                // ============================================================
                if (rewardPool.Count < 3)
                {
                    foreach (var w in weaponHolder.WeaponEntities)
                    {
                        ref var wc = ref _weaponPool.Value.Get(w);
                        if (wc.Level >= wc.MaxLevel) continue;

                        var baseWeapon = weaponConfig.GetWeapon(wc.Name);
                        if (baseWeapon != null)
                        {
                            rewardPool.Add(baseWeapon);
                            if (rewardPool.Count >= 3) break;
                        }
                    }
                }


                // ============================================================
                // 4) Если меньше 3 — добавляем существующие апгрейды
                // ============================================================
                if (rewardPool.Count < 3)
                {
                    foreach (var u in upgradeHolder.UpgradesEntities)
                    {
                        ref var uc = ref _upgradePool.Value.Get(u); 

                        var baseUpgrade = upgradeConfig.GetUpgrade(uc.KeyName);
                        if (baseUpgrade != null)
                        {
                            rewardPool.Add(baseUpgrade);
                            if (rewardPool.Count >= 3) break;
                        }
                    }
                }


                // ============================================================
                // 5) Выбираем до 3 случайных
                // ============================================================
                rewardPool.Shuffle();

                int count = Mathf.Min(3, rewardPool.Count);
                for (int i = 0; i < count; i++)
                    finalRewardList.Add(rewardPool[i]);


                // ============================================================
                // 6) Выводим окно
                // ============================================================
                if (UIModule.TryGetCanvas<BattleCanvas>(out var canvas))
                {
                    if (canvas.TryOpenPanel<BattlePanel>(out var panel))
                    {
                        panel.OpenWindow<UpgradeWindow>().InvokeRewardList(finalRewardList);
                    }
                }

                _levelUpPool.Value.Del(entity);
            }
        }
    }
}


// Utility shuffle
public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int k = Random.Range(0, i + 1);
            (list[i], list[k]) = (list[k], list[i]);
        }
    }
}
