using UnityEngine;
using Leopotam.Ecs;
using System.Collections.Generic;

public class GiftsBagActor: Actor
{
    public override void ExpandEntity(EcsEntity entity)
    {
        entity.Get<GiftBagStorageComponent>().GiftsTypes = new List<int>();
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        GiftActor gift = other.gameObject.GetComponent<GiftActor>();
        if (gift != null)
        {
            GetEntity().Get<GiftBagStorageComponent>().GiftsTypes.Add(gift.GetEntity().Get<GiftTypeComponent>().GiftType);
            gift.GetEntity().Del<GiftFlag>();
        }
    }
}
