using Leopotam.Ecs;
using Unity.Cinemachine;
using UnityEngine;

public class EndGameSystem: Injects, IEcsRunSystem
{
    private EcsFilter<EndGameEvent> _endGameEventFilter;
    private bool _endGameEventSent = false;
    
    public void Run()
    {
        foreach (int i in _endGameEventFilter)
        {
            RealtimeData.IsGameEnd = true;
            ref var lastChunk = ref _endGameEventFilter.Get1(i).LastChunk;
            CinemachineCamera endGameCamera = lastChunk.Get<CinemachineCameraRef>().CinemachineCamera;
            SceneData.PlayerCamera.Priority = 0;
            endGameCamera.Priority = 10;
        }

        if (!_endGameEventSent)
        {
            bool allCatsHappy = true;
            foreach (EcsEntity cat in RealtimeData.Cats)
            {
                if (cat.Get<CatCharComponent>().Mood == 0)
                {
                    allCatsHappy = false;
                    break;
                }
            }

            if (allCatsHappy)
            {
                EcsWorld.NewEntity().Get<SpawnLastChunkEvent>();
                _endGameEventSent = true;
            }
        }
    }
}
