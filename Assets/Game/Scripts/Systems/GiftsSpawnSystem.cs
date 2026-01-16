using UnityEngine;
using Leopotam.Ecs;
using System.Collections.Generic;

public class GiftsSpawnSystem: Injects, IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter<SpawnGiftEvent> _spawnGiftEventFilter;
    private List<GiftActor> _giftsPrefabs = new List<GiftActor>();
    private Transform _giftsSpawnPoint;

    public void Init()
    {
        _giftsPrefabs = GameConfig.CatsCharConfig.GiftsPrefabs;
        _giftsSpawnPoint = SceneData.GiftSpawnPoint;
    }

    public void Run()
    {
        foreach(int i in _spawnGiftEventFilter)
        {
            int type = _spawnGiftEventFilter.Get1(i).GiftType;
            GiftActor gift = GameObject.Instantiate(_giftsPrefabs[type],_giftsSpawnPoint);
            gift.Init(EcsWorld);
        }
    }
}
