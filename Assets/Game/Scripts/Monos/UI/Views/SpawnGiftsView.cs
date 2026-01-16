using UnityEngine;
using Leopotam.Ecs;

public class SpawnGiftsView : MonoBehaviour
{
    private EcsWorld _world;
    public void Init(EcsWorld world)
    {
        _world = world;
    }

    public void SpawnGift(int giftType)
    {
        _world.NewEntity().Get<SpawnGiftEvent>().GiftType = giftType;
    }
}
