using UnityEngine;
using Leopotam.Ecs;

public class InitialSystem: Injects, IEcsInitSystem
{
    public void Init()
    {
        SceneData.Player.Init(EcsWorld);
        SceneData.Player.GetEntity().Get<FreezeFlag>();
        SceneData.GiftsBag.Init(EcsWorld);
        RealtimeData.GiftBag = SceneData.GiftsBag.GetEntity();
        SceneData.SpawnGiftsView.Init(EcsWorld);
    }
}
