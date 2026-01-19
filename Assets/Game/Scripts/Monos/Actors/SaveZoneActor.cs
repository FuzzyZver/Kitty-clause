using UnityEngine;
using Leopotam.Ecs;

public class SaveZoneActor: Actor
{
    public override void ExpandEntity(EcsEntity entity)
    {
        entity.Get<TransformRef>();
    }
}
