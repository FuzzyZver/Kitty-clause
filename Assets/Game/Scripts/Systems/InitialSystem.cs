using UnityEngine;
using Leopotam.Ecs;

public class InitialSystem: Injects, IEcsInitSystem
{
    public void Init()
    {
        SceneData.Player.Init(EcsWorld);
    }
}
