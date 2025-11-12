using UnityEngine;
using Client; 
using Leopotam.EcsLite;
using System;

public class BattlePanel : SourcePanel
{
    [SerializeField] MovementJoystick movementJoystick;

    EcsPool<InputMovementState> _movePool = default;
    EcsPool<DisposeEvent> _disposePool = default;
    EcsFilter _filter = null;

    [UIInject] EcsWorld _world;

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
            _disposePool.Add(entity);
        }
    }
}

public enum InputType { move, aim, ability }
