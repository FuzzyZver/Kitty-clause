using UnityEngine;
using Leopotam.Ecs;

public class CollisionSystem: Injects, IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter<OnCollisionEvent> _onCollisionEventFilter;

    public void Init()
    {
        //работа с таймером
    }

    public void Run()
    {
        foreach (int i in _onCollisionEventFilter)
        {
            //работа с таймером
            Debug.Log("Столкновение!");
        }
    }
}
