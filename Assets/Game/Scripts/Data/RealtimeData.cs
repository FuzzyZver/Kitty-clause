using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

public class RealtimeData
{
    public List<EcsEntity> Cats;
    public EcsEntity GiftBag;
    public float StartLevelTime;
    public TimeSpan Timer;
    public int SuccessCats;
    public int ObstacleHits;
    public bool IsGameEnd;
}
