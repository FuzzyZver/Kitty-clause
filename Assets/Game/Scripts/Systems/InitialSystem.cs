using UnityEngine;
using Leopotam.Ecs;

public class InitialSystem: Injects, IEcsInitSystem
{
    public void Init()
    {
        SceneData.Player.Init(EcsWorld);
        SceneData.Player.GetEntity().Get<FreezeFlag>();
        SceneData.GiftsBag.Init(EcsWorld);
        SceneData.SpawnGiftsView.Init(EcsWorld);
        UI.StartLevelView.Init(EcsWorld);
    }
}
