using UnityEngine;
using Leopotam.Ecs;

public class GroundCheckSystem: Injects, IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter<GroundRaycastEvent> _groundRaycastEventFilter;
    private PlayerActor _playerRef;
    private bool _isWasGround;
    
    public void Init()
    {
        _playerRef = SceneData.Player;
    }

    public void Run()
    {
        var playerEntity = _playerRef.GetEntity();
        bool isGround = _groundRaycastEventFilter.GetEntitiesCount() > 0;
        if (isGround && !_isWasGround) EcsWorld.NewEntity().Get<GroundEvent>();
        _isWasGround = isGround;

        if (isGround) playerEntity.Get<IsGroundFlag>();
        else playerEntity.Del<IsGroundFlag>();
    }
}
