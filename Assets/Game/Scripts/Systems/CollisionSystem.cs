using UnityEngine;
using Leopotam.Ecs;

public class CollisionSystem: Injects, IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter<OnCollisionEvent> _onCollisionEventFilter;
    public float _obstacleDeletedTime;

    public void Init()
    {
        _obstacleDeletedTime = GameConfig.CommonConfig.ObstacleDeletedTime;
    }

    public void Run()
    {
        foreach (int i in _onCollisionEventFilter)
        {
            RealtimeData.StartLevelTime -= _obstacleDeletedTime;
        }
    }
}
