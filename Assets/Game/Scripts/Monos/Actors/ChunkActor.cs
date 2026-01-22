using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

public class ChunkActor: Actor
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Transform _startSP;
    [SerializeField] private Transform _endSP;
    [SerializeField] private List<ObstacleActor> _obstaclesActors;
    public override void ExpandEntity(EcsEntity entity)
    {
        entity.Get<TransformRef>().Transform = _transform;
        entity.Get<ChunkComponent>() = new ChunkComponent()
        {
            StartSP = _startSP,
            EndSP = _endSP
        };
        foreach (ObstacleActor obstacle in _obstaclesActors)
        {
            obstacle.Init(GetWorld());
        }
    }
}
