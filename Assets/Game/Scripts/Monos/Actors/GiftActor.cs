using UnityEngine;
using Leopotam.Ecs;

public class GiftActor: Actor
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Rigidbody2D _rigidbody;

    public override void ExpandEntity(EcsEntity entity)
    {
        entity.Get<GiftFlag>();
        entity.Get<TransformRef>().Transform = _transform;
        entity.Get<GiftTypeComponent>();
        entity.Get<RigidbodyRef>().Rigidbody2D = _rigidbody;
    }
}
