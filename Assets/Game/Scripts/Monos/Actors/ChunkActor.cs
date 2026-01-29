using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using Unity.Cinemachine;
using UnityEngine;

public class ChunkActor: Actor
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Transform _startSP;
    [SerializeField] private Transform _endSP;
    [SerializeField] private List<ObstacleActor> _obstaclesActors;
    [SerializeField] private CatActor _catActor;
    [SerializeField] private CinemachineCamera _cinemachineCamera;
    public override void ExpandEntity(EcsEntity entity)
    {
        entity.Get<TransformRef>().Transform = _transform;
        entity.Get<ChunkComponent>() = new ChunkComponent()
        {
            StartSP = _startSP,
            EndSP = _endSP
        };

        if (_catActor)
        {
            _catActor.Init(GetWorld());
        }
        foreach (ObstacleActor obstacle in _obstaclesActors)
        {
            obstacle.Init(GetWorld());
        }

        if (_cinemachineCamera)
        {
            entity.Get<CinemachineCameraRef>().CinemachineCamera = _cinemachineCamera;
        }
    }

    public CatActor GetCat() => _catActor;
}
