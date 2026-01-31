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

            float timePenalty = Mathf.Max(0, 5000 - (int)((float)(RealtimeData.Timer.TotalSeconds) * 10));
            float score = RealtimeData.SuccessCats * timePenalty;
            score -= RealtimeData.ObstacleHits * 100;
            Race race = new Race
            {
                Time = RealtimeData.Timer,
                SuccessCats = RealtimeData.SuccessCats,
                CollisionsFailed = RealtimeData.ObstacleHits,
                TotalScore = score
            };
            GameConfig.DataConfig.Races.Add(race);
            RaceView endGameScreen = GameObject.Instantiate(GameConfig.CommonConfig.EndGameScreen, UI.transform);
            endGameScreen.Init(race);
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
