using Client;
using Leopotam.EcsLite;
using Statement;
using UnityEngine;
using UnityEngine.UI;

public class DevSlot : SourceSlot
{
    [SerializeField] DevType _type;

    [SerializeField] TMPro.TextMeshProUGUI _title;
    [SerializeField] Slider slider;

    [UIInject] EcsWorld _world;
    [UIInject] BattleState _state;

    public override SourceSlot Init(SourceLayout layout)
    {
        if(slider) slider.onValueChanged.AddListener(ValueUpdate);
        return base.Init(layout);
    }

    void ValueUpdate(float value)
    {
        switch (_type)
        {
            case DevType.spawn:
                SpawnChange();
                break;
            case DevType.interval:
                IntervalChange();
                break;
        }
    }

    void SpawnChange()
    {
        if (_state.TryGetEntity("level", out int entity))
        {
            ref var levelComp = ref _world.GetPool<LevelComponent>().Get(entity);
            levelComp.SpawnCount = Mathf.RoundToInt(slider.value);
            _title.text = $"Spawn in one time: {levelComp.SpawnCount}";
        }
    }

    void IntervalChange()
    {
        if (_state.TryGetEntity("level", out int entity))
        {
            ref var levelComp = ref _world.GetPool<LevelComponent>().Get(entity);
            levelComp.SpawnInterval = slider.value;
            _title.text = $"Spawn delay: {levelComp.SpawnInterval}";
        }
    }
     
    void InvokeClose()
    {
        if (UIModule.TryGetCanvas<BattleCanvas>(out var battleCanvas))
        {
            if (battleCanvas.TryOpenPanel<BattlePanel>(out var panel))
            {

            }
        }
    }

    void AddMoney()
    {
        if (_state.TryGetEntity("player", out int entity))
        {
            ref var playerComp = ref _world.GetPool<PlayerComponent>().Get(entity);
            playerComp.AddMoney(10); 
        }
    }

    void AddLevel()
    {
        if (_state.TryGetEntity("player", out int entity))
        {
            ref var playerComp = ref _world.GetPool<PlayerComponent>().Get(entity);
            playerComp.Level++;
            _world.GetPool<PlayerLevelUpEvent>().Add(entity);
        }
    }

    public override void Dispose()
    {
        slider.onValueChanged.RemoveAllListeners();
        base.Dispose();
    }

    public override void OnActive()
    { 
    }

    public override void OnClick()
    { 
        switch (_type)
        {
            case DevType.money:
                AddMoney();
                break;
            case DevType.level:
                AddLevel();
                break;
            case DevType.close:
                InvokeClose();
                break;
        }
    }

    public override void OnHold(float holdTime)
    { 
    }

    public override void OnPress()
    { 
    }

    public override void OnRelease()
    { 
    }

    public override void UpdateView(object data)
    { 
    }
     

    public enum DevType { spawn, interval, money, level, close }
}
