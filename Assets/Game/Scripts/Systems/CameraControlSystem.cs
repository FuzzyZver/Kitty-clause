using UnityEngine;
using Leopotam.Ecs;
using Unity.Cinemachine;

public class CameraControlSystem: Injects, IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter<CameraToPlayerEvent> _cameraToPlayerEventFilter;
    private CinemachineCamera _startCamera;
    private CinemachineCamera _playerCamera;

    public void Init()
    {
        _startCamera = SceneData.StartCamera;
        _playerCamera = SceneData.PlayerCamera;
    }

    public void Run()
    {
        foreach(int i in _cameraToPlayerEventFilter)
        {
            _startCamera.Priority = 0;
            _playerCamera.Priority = 10;
            SceneData.Player.GetEntity().Del<FreezeFlag>();
        }
    }
}
