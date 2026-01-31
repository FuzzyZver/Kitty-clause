using System;
using Leopotam.Ecs;
using UnityEngine;

public class TimerSystem: Injects, IEcsRunSystem
{
    private EcsFilter<CameraToPlayerEvent> _startLevelEventFilter;
    private float _localTimer;
    private bool isLevelStarted = false;

    public void Run()
    {
        foreach (int i in _startLevelEventFilter)
        {
            RealtimeData.StartLevelTime = Time.time;
            isLevelStarted = true;
        }
        if (isLevelStarted && !RealtimeData.IsGameEnd)
        {
            _localTimer = Time.time - RealtimeData.StartLevelTime;
            TimeSpan t = TimeSpan.FromSeconds(_localTimer);
            RealtimeData.Timer = t;
            UI.TimerText.text = $"{t.Minutes:00}:{t.Seconds:00}:{t.Milliseconds:000}";
        }
    }
}
