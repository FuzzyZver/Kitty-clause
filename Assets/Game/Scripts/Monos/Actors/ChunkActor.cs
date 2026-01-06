using Leopotam.Ecs;
using UnityEngine;

public class ChunkActor: Actor
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Transform _startSP;
    [SerializeField] private Transform _endSP;
    public override void ExpandEntity(EcsEntity entity)
    {
        entity.Get<TransformRef>().Transform = _transform;
        entity.Get<ChunkComponent>() = new ChunkComponent()
        {
            StartSP = _startSP,
            EndSP = _endSP
        };
    }
}
