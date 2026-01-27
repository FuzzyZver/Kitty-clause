using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

public class GiveGiftView : MonoBehaviour
{
    [SerializeField] private Image _giftImage;
    private int _giftType;
    private EcsWorld _world;
    public void Init(EcsWorld world, Sprite giftSprite, int giftType)
    {
        _world = world;
        _giftImage.sprite = giftSprite;
        _giftType = giftType;
    }

    public void GiveGift()
    {
        _world.NewEntity().Get<GiveGiftEvent>().GiftType = _giftType;
    }
}
