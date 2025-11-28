using UnityEngine;
using Client; 
using Leopotam.EcsLite;
using System;
using UnityEngine.UI;
using TMPro;

public class BattlePanel : SourcePanel
{
    [SerializeField] Button _devTool;
    [SerializeField] MovementJoystick movementJoystick;
    [SerializeField] Image _expFill;
    [SerializeField] TextMeshProUGUI _levelTitle;
    [SerializeField] TextMeshProUGUI _resourcesTitle;
    [SerializeField] TextMeshProUGUI _elapsedTimeTitle;

    EcsPool<InputMovementState> _movePool = default;
    EcsPool<DisposeEvent> _disposePool = default;
    EcsFilter _filter = null;

    [UIInject] EcsWorld _world;

    public override void Init(SourceCanvas canvasParent)
    {
        _devTool.gameObject.SetActive(false);

        if (Debug.isDebugBuild)
        {
            _devTool.gameObject.SetActive(true);
            _devTool.onClick.AddListener(ToDevPanel);
        }

        ObserverEntity.instance.OnLevelChange += OnLevelChange;
        ObserverEntity.instance.OnExperienceChange += OnExpChange;
        ObserverEntity.instance.OnElapsedTimeChange += OnElapsedTime;
        ObserverEntity.instance.OnResourcesChange += OnResources;


        base.Init(canvasParent);
    }

    void OnResources(int value)
    {
        _resourcesTitle.text = $"{value}";
    }

    void OnElapsedTime(float value)
    { 
        float seconds = value;
        TimeSpan time = TimeSpan.FromSeconds(seconds);

        _elapsedTimeTitle.text = $"Time elapsed: {time.Minutes:D2}:{time.Seconds:D2}";
    }

    void OnLevelChange(int level)
    {
        _levelTitle.text = level.ToString();
    }

    void OnExpChange(float value)
    {
        _expFill.fillAmount = value;
    }

    public override void OnDipose()
    {
        if (Debug.isDebugBuild) _devTool.onClick.RemoveAllListeners();

        ObserverEntity.instance.OnLevelChange -= OnLevelChange;
        ObserverEntity.instance.OnExperienceChange -= OnExpChange;
        ObserverEntity.instance.OnElapsedTimeChange -= OnElapsedTime;
        ObserverEntity.instance.OnResourcesChange -= OnResources;

        base.OnDipose();
    }

    void ToDevPanel()
    {
        if (UIModule.TryGetCanvas<BattleCanvas>(out var battleCanvas))
        {
            battleCanvas.OpenPanel<DevelopPanel>();
        }
    }

    public override void OnOpen(params Action[] onComplete)
    {
        base.OnOpen(onComplete);

        movementJoystick.OnJoystickDown += OnInputDown;
        movementJoystick.OnJoystickUp += OnInputUp;  
    }

    public override void OnCLose(params Action[] onComplete)
    {
        base.OnCLose(onComplete);

        movementJoystick.OnJoystickDown -= OnInputDown;
        movementJoystick.OnJoystickUp -= OnInputUp; 
    }

    public override void OnInject()
    {
        if (_world == null) return;

        _movePool = _world.GetPool<InputMovementState>(); 
        _disposePool = _world.GetPool<DisposeEvent>();
        _filter = _world.Filter<PlayerComponent>().End(); 
    }

    void OnInputDown(InputType type)
    { 
        foreach (var entity in _filter)
        {
            if (!_movePool.Has(entity)) _movePool.Add(entity).MovementJoystick = movementJoystick; 
        }
    }

    void OnInputUp(InputType type)
    {
        foreach (var entity in _filter)
        {
            if(!_disposePool.Has(entity)) _disposePool.Add(entity);
        }
    }
}

public enum InputType { move, aim, ability }
