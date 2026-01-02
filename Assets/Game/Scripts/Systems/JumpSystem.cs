using UnityEngine;
using Leopotam.Ecs;

public class JumpSystem: Injects, IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter<JumpInputEvent> _jumpInputEventFilter;
    private PlayerActor _playerRef;

    public void Init()
    {
        _playerRef = SceneData.Player;
    }

    public void Run()
    {
        var playerEntity = _playerRef.GetEntity();
        
        foreach (int i in _jumpInputEventFilter)
        {
            if (playerEntity.Has<DeadFlag>()) return;
            if (playerEntity.Has<FreezeFlag>()) return;
            if (playerEntity.Has<IsGroundFlag>())
            {
                playerEntity.Get<JumpFlag>();
            }
        }
    }
}
