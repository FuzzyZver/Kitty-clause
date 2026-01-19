using UnityEngine;
using Leopotam.Ecs;

public class StartLevelView : MonoBehaviour
{
    private EcsWorld _world;
    public void Init(EcsWorld world)
    {
        _world = world;
    }

    public void ChangeCamera()
    {
        _world.NewEntity().Get<CameraToPlayerEvent>();
    }
}
