using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Statement;
using Unity.Cinemachine;
using UnityEngine;

namespace Client 
{
    sealed class InitCameraSystem : IEcsInitSystem 
    {
        readonly EcsWorldInject _world = default;
        readonly EcsSharedInject<BattleState> _state = default;
        readonly EcsPoolInject<CameraComponent> _cameraPool = default;

        public void Init (IEcsSystems systems) 
        { 
            var entity = _world.Value.NewEntity();
            ref var cameraComp = ref _cameraPool.Value.Add(entity);

            cameraComp.CameraBrain = GameObject.FindFirstObjectByType<CinemachineBrain>();
            cameraComp.Camera = GameObject.FindFirstObjectByType<CinemachineCamera>();

            _state.Value.AddEntity("camera", entity);
        }
    }
}
