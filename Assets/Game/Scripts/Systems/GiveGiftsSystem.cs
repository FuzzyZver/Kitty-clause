using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

public class GiveGiftsSystem : Injects, IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter<InteractInputEvent> _interactInputEventFilter;
    private EcsFilter<GiveGiftEvent> _giveGiftEventFilter;
    private EcsFilter<CatInteractEvent> _catInteractEventFilter;

    private CatActor _currentCat;
    private GiveGiftView _giveGiftView;

    private readonly List<GiveGiftView> _spawnedGifts = new();

    public void Init()
    {
        _giveGiftView = GameConfig.CommonConfig.GiveGiftView;
    }

    public void Run()
    {
        foreach (int i in _catInteractEventFilter)
        {
            ClearGifts();

            CatActor cat = _catInteractEventFilter.Get1(i).CatActor;

            if (_currentCat == cat)
            {
                _currentCat = null;
                return;
            }

            _currentCat = cat;
            SpawnGifts(cat.GetEntity());
        }

        if (_currentCat == null) return;

        foreach (int i in _giveGiftEventFilter)
        {
            int giftType = _giveGiftEventFilter.Get1(i).GiftType;
            EcsEntity catEntity = _currentCat.GetEntity();

            var cat = RealtimeData.Cats[catEntity.Get<CatTypeComponent>().CatType];
            cat.Get<CatCharComponent>().Mood =
                cat.Get<CatCharComponent>().GiftTipe == giftType ? 1 : 2;
            Debug.Log(cat.Get<CatCharComponent>().Mood);
            var gifts = RealtimeData.GiftBag.Get<GiftBagStorageComponent>().GiftsTypes;

            for (int j = 0; j < gifts.Count; j++)
            {
                if (gifts[j] == giftType)
                {
                    gifts.RemoveAt(j);
                    break;
                }
            }

            ClearGifts();
            _currentCat = null;
        }
    }

    private void SpawnGifts(EcsEntity catEntity)
    {
        Transform spawnPoint = catEntity.Get<TransformRef>().Transform;
        var gifts = RealtimeData.GiftBag.Get<GiftBagStorageComponent>().GiftsTypes;

        foreach (int gift in gifts)
        {
            var view = GameObject.Instantiate(_giveGiftView, spawnPoint);
            view.Init(EcsWorld, GameConfig.CatsCharConfig.GiftsSprites[gift], gift);
            _spawnedGifts.Add(view);
        }
    }

    private void ClearGifts()
    {
        foreach (var gift in _spawnedGifts)
            if (gift) GameObject.Destroy(gift.gameObject);

        _spawnedGifts.Clear();
    }
}
