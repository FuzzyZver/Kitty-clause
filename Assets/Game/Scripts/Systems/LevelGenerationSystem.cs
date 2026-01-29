using UnityEngine;
using Leopotam.Ecs;
using System.Collections.Generic;

public class LevelGenerationSystem : Injects, IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter<SpawnLastChunkEvent> _spawnLastChunkEventFilter;

    private PlayerActor _player;
    private List<ChunkActor> _chunks = new();
    private ChunkActor _saveZone;

    private List<ChunkActor> _spawnedChunks = new();
    private ChunkActor _spawnedSaveZone;

    private float _spawnOffset = 1f;

    private bool _lastChunkSpawned;
    private int _normalChunksCounter;

    public void Init()
    {
        _player = SceneData.Player;
        _chunks = GameConfig.LevelConfig.Chunks;
        _saveZone = GameConfig.LevelConfig.SaveZone;

        SceneData.FirstChunk.Init(EcsWorld);
        _spawnedChunks.Add(SceneData.FirstChunk);
    }

    public void Run()
    {
        if (RealtimeData.IsGameEnd) return;
        if (_spawnedChunks.Count == 0) return;

        var playerEntity = _player.GetEntity();
        var lastChunkEntity = _spawnedChunks[^1].GetEntity();
        var firstChunkEntity = _spawnedChunks[0].GetEntity();

        if (_lastChunkSpawned &&
            playerEntity.Get<TransformRef>().Transform.position.x >
            lastChunkEntity.Get<ChunkComponent>().StartSP.position.x)
        {
            EcsWorld.NewEntity().Get<EndGameEvent>().LastChunk = lastChunkEntity;
        }

        foreach (int i in _spawnLastChunkEventFilter)
        {
            if (_lastChunkSpawned) continue;

            var realLast = _spawnedChunks[^1].GetEntity();
            ChunkActor last = SpawnChunk(GameConfig.LevelConfig.LastChunk, realLast);
            last.GetEntity().Get<LastChunkFlag>();
            _spawnedChunks.Add(last);
            _lastChunkSpawned = true;

            return;
        }

        if (_lastChunkSpawned) return;

        if (playerEntity.Get<TransformRef>().Transform.position.x >
            lastChunkEntity.Get<ChunkComponent>().StartSP.position.x - _spawnOffset)
        {
            var prefab = _chunks[Random.Range(0, _chunks.Count)];
            ChunkActor chunk = SpawnChunk(prefab, lastChunkEntity);
            _spawnedChunks.Add(chunk);

            _normalChunksCounter++;

            if (_normalChunksCounter % 3 == 0)
            {
                var realLast = _spawnedChunks[^1].GetEntity();
                ChunkActor saveZoneChunk = SpawnChunk(_saveZone, realLast);

                _spawnedSaveZone = saveZoneChunk;
                _spawnedChunks.Add(saveZoneChunk);

                SpawnCatsInSaveZone(saveZoneChunk);
            }
        }

        if (_spawnedSaveZone == null) return;

        if (firstChunkEntity.Get<ChunkComponent>().EndSP.position.x <
            _spawnedSaveZone.GetEntity().Get<ChunkComponent>().StartSP.position.x &&
            _spawnedSaveZone.GetEntity().Get<ChunkComponent>().EndSP.position.x <
            playerEntity.Get<TransformRef>().Transform.position.x)
        {
            ClearChunksBeforeSaveZone();

            if (playerEntity.Get<TransformRef>().Transform.position.x >
                _spawnedSaveZone.GetEntity().Get<ChunkComponent>().EndSP.position.x)
            {
                _spawnedSaveZone = null;
            }
        }
    }

    private ChunkActor SpawnChunk(ChunkActor prefab, EcsEntity lastChunkEntity)
    {
        ChunkActor chunk = GameObject.Instantiate(prefab);
        chunk.Init(EcsWorld);

        var entity = chunk.GetEntity();
        entity.Get<TransformRef>().Transform.position =
            lastChunkEntity.Get<ChunkComponent>().EndSP.position -
            entity.Get<ChunkComponent>().StartSP.position;

        return chunk;
    }

    private void SpawnCatsInSaveZone(ChunkActor saveZone)
    {
        List<EcsEntity> cats = RealtimeData.Cats;

        for (int j = 0; j < cats.Count; j++)
        {
            if (cats[j].Get<CatCharComponent>().Mood == 0)
            {
                CatActor currentCat = saveZone.GetCat();
                currentCat.SetCat(j, cats[j].Get<CatCharComponent>().CatSprite);
            }
        }
    }

    private void ClearChunksBeforeSaveZone()
    {
        for (int i = _spawnedChunks.Count - 1; i >= 0; i--)
        {
            var chunk = _spawnedChunks[i];
            if (chunk == _spawnedSaveZone) continue;

            var entity = chunk.GetEntity();

            if (entity.Get<ChunkComponent>().StartSP.position.x <
                _spawnedSaveZone.GetEntity().Get<ChunkComponent>().EndSP.position.x)
            {
                GameObject.Destroy(entity.Get<TransformRef>().Transform.gameObject);
                entity.Destroy();
                _spawnedChunks.RemoveAt(i);
            }
        }
    }
}
