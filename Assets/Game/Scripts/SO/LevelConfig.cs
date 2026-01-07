using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public int LevelsCount;
    public int ChunksCount;

    public List<ChunkActor> Chunks;
    public ChunkActor SaveZone;

    public GameObject SkyPrefab;
    public GameObject BackgroundPrefab;
    public GameObject MiddlegroundPrefab;
    public GameObject ForegroundPrefab;
    public GameObject GroundPrefab;
}