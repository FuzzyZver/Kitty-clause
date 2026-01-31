using System;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DataConfig", menuName = "Configs/DataConfig")]
public class DataConfig : ScriptableObject
{
    [Header("Score")] 
    public List<Race> Races;
    public RaceView RaceView;

    [Header("Settings")]
    public float MuzikVolume;
    public float SoundVolume;
}

[System.Serializable]
public class Race
{
    public TimeSpan Time;
    public int SuccessCats;
    public int CollisionsFailed;
    public float TotalScore;
}
