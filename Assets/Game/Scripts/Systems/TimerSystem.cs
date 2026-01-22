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
        if (isLevelStarted)
        {
            _localTimer = Time.time - RealtimeData.StartLevelTime;
            RealtimeData.Timer = _localTimer;
            TimeSpan t = TimeSpan.FromSeconds(_localTimer);
            UI.TimerText.text = $"{t.Minutes:00}:{t.Seconds:00}:{t.Milliseconds:000}";
        }
    }
}
