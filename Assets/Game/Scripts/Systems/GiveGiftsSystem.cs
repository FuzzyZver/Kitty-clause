using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

public class GiveGiftsSystem : Injects, IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter<InteractInputEvent> _interactInputEventFilter;
    private EcsFilter<GiveGiftEvent> _giveGiftEventFilter;
    private EcsFilter<CatInteractEvent> _catInteractEventFilter;
    private List<GameObject> _moodsSprites = new List<GameObject>();
    private Transform _dialogWindow;

    private CatActor _currentCat;
    private GiveGiftView _giveGiftView;

    private GameObject _currentMoodView;
    private readonly List<GiveGiftView> _spawnedGifts = new();

    public void Init()
    {
        _giveGiftView = GameConfig.CommonConfig.GiveGiftView;
        _moodsSprites = GameConfig.CatsCharConfig.MoodsSprites;
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
            var catEntity = cat.GetEntity();
            int mood = RealtimeData.Cats[catEntity.Get<CatTypeComponent>().CatType].Get<CatCharComponent>().Mood;
            if(mood == 0) SpawnGifts(catEntity);
            else SpawnMood(catEntity);
        }

        if (_currentCat == null) return;

        foreach (int i in _giveGiftEventFilter)
        {
            int giftType = _giveGiftEventFilter.Get1(i).GiftType;
            var catEntity = _currentCat.GetEntity();

            var cat = RealtimeData.Cats[catEntity.Get<CatTypeComponent>().CatType];
            cat.Get<CatCharComponent>().Mood =
                cat.Get<CatCharComponent>().GiftTipe == giftType ? 1 : 2;
            if (cat.Get<CatCharComponent>().Mood == 1) RealtimeData.SuccessCats++;
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
            SpawnMood(catEntity);
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

    private void SpawnMood(EcsEntity catEntity)
    {
        var cat = RealtimeData.Cats[catEntity.Get<CatTypeComponent>().CatType];
        int mood = cat.Get<CatCharComponent>().Mood;

        if (_currentMoodView != null)
            GameObject.Destroy(_currentMoodView);

        if (mood == 0) return;

        if (mood < 0 || mood >= _moodsSprites.Count)
        {
            Debug.LogError("Mood index out of range: " + mood);
            return;
        }

        Transform parent = _dialogWindow != null
            ? _dialogWindow
            : catEntity.Get<TransformRef>().Transform;

        _currentMoodView = GameObject.Instantiate(
            _moodsSprites[mood],
            parent
        );
    }

    private void ClearGifts()
    {
        foreach (var gift in _spawnedGifts)
            if (gift) GameObject.Destroy(gift.gameObject);

        _spawnedGifts.Clear();
    }
}
