using System;
using Leopotam.Ecs;
using UnityEngine;

public class ObstacleActor: Actor
{
    public override void ExpandEntity(EcsEntity entity)
    {
        entity.Get<TransformRef>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        PlayerActor player = other.GetComponent<PlayerActor>();
        if (player != null)
        {
            GetWorld().NewEntity().Get<OnCollisionEvent>();
        }
    }
}
