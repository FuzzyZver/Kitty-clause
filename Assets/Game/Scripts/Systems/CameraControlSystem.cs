using UnityEngine;
using Leopotam.Ecs;
using Unity.Cinemachine;

public class CameraControlSystem: Injects, IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter<CameraToPlayerEvent> _cameraToPlayerEventFilter;
    private CinemachineCamera _startCamera;
    private CinemachineCamera _playerCamera;
    private float _defaultFov;
    private float _runFov;
    private float _cameraSmooth;

    public void Init()
    {
        _startCamera = SceneData.StartCamera;
        _playerCamera = SceneData.PlayerCamera;
        _defaultFov = GameConfig.InputConfig.DefaultFov;
        _runFov = GameConfig.InputConfig.RunFov;
        _cameraSmooth = GameConfig.InputConfig.CameraSmooth;
    }

    public void Run()
    {
        var playerEntity = SceneData.Player.GetEntity();
        foreach(int i in _cameraToPlayerEventFilter)
        {
            _startCamera.Priority = 0;
            _playerCamera.Priority = 10;
            SceneData.Player.GetEntity().Del<FreezeFlag>();
        }

        if (playerEntity.Has<MoveFlag>())
        {
            _playerCamera.Lens.FieldOfView = Mathf.Lerp(_playerCamera.Lens.FieldOfView, _runFov, Time.deltaTime * _cameraSmooth);
        }
        else
        {
            _playerCamera.Lens.FieldOfView = Mathf.Lerp(_playerCamera.Lens.FieldOfView, _defaultFov, Time.deltaTime * _cameraSmooth);
        }
    }
}
