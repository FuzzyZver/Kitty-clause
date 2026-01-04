using UnityEngine;
using Leopotam.Ecs;
using System.Collections.Generic;

public class CatsWishesSystem: Injects, IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter<WishlistUpdateEvent> _wishlistUpdateEventFilter;
    private List<EcsEntity> _cats = new List<EcsEntity>();

    public void Init()
    {
        CatsCharConfig catsCharConfig = GameConfig.CatsCharConfig;
        List<string> catsNames = new List<string>(catsCharConfig.CatsNames);
        List<Sprite> catsSprites = new List<Sprite>(catsCharConfig.CatsSprites);
        List<Sprite> giftsSprites = new List<Sprite>(catsCharConfig.GiftsSprites); ;

        for (int i = 0; i < catsCharConfig.CatsCount; i++)
        {
            int catNameId = Random.Range(0, catsNames.Count);
            int catSpriteId = Random.Range(0, catsSprites.Count);
            int giftType = Random.Range(0, giftsSprites.Count);

            var cat = EcsWorld.NewEntity();
            _cats.Add(cat);
            cat.Get<CatCharComponent>() = new CatCharComponent
            {
               CatName = catsNames[catNameId],
               GiftTipe = giftType,
               CatSprite = catsSprites[catSpriteId],
               GiftSprite = giftsSprites[giftType]
            };
            catsNames.RemoveAt(catNameId);
            catsSprites.RemoveAt(catSpriteId);
        }
        UI.LettersView.Init(EcsWorld, _cats);
    }

    public void Run()
    {
        foreach(int i in _wishlistUpdateEventFilter)
        {
            UI.LettersView.WishlistUpdate(_cats);
        }
    }
}
