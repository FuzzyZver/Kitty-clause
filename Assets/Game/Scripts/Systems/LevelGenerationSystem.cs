using UnityEngine;
using Leopotam.Ecs;
using System.Collections.Generic;

public class LevelGenerationSystem: Injects, IEcsInitSystem, IEcsRunSystem
{
    private PlayerActor _player;
    private List<ChunkActor> _chunks = new List<ChunkActor>();
    private ChunkActor _saveZone;
    private int _levelsCount;
    private int _chunksCount;
    private List<ChunkActor> _spawnedChunks = new List<ChunkActor>();
    private ChunkActor _spawnedSaveZone;
    private float _spawnOffset = 1f;

    public void Init()
    {
        _player = SceneData.Player;
        _chunks = GameConfig.LevelConfig.Chunks;
        _levelsCount = GameConfig.LevelConfig.LevelsCount;
        _chunksCount = GameConfig.LevelConfig.ChunksCount;
        _saveZone = GameConfig.LevelConfig.SaveZone;

        SceneData.FirstChunk.Init(EcsWorld);
        _spawnedChunks.Add(SceneData.FirstChunk);
    }

    public void Run()
    {
        if (_spawnedChunks.Count == 0) return;
        var playerEntity = _player.GetEntity();
        var lastChunk = _spawnedChunks[_spawnedChunks.Count - 1];
        var lastChunkEntity = lastChunk.GetEntity();
        var firstChunk = _spawnedChunks[0];
        var firstChunkEntity = firstChunk.GetEntity();

        if (_spawnedChunks.Count >= _chunksCount && _spawnedSaveZone == null)
        {
            var prefab = _saveZone;
            ChunkActor saveZoneChunk = GameObject.Instantiate(prefab);
            saveZoneChunk.Init(EcsWorld);

            var saveZoneEntity = saveZoneChunk.GetEntity();
            saveZoneEntity.Get<TransformRef>().Transform.position =
                lastChunkEntity.Get<ChunkComponent>().EndSP.position -
                saveZoneEntity.Get<ChunkComponent>().StartSP.position;

            _spawnedSaveZone = saveZoneChunk;
            _spawnedChunks.Add(_spawnedSaveZone);

            List<EcsEntity> cats = RealtimeData.Cats;
            for (int j= 0; j < cats.Count; j++)
            {
                if (cats[j].Get<CatCharComponent>().Mood == 0)
                {
                    CatActor currentCat = _spawnedSaveZone.GetCat();
                    currentCat.SetCat(j, cats[j].Get<CatCharComponent>().CatSprite);
                }
            }
        }
        else
        {
            if (playerEntity.Get<TransformRef>().Transform.position.x >
            lastChunkEntity.Get<ChunkComponent>().StartSP.position.x - _spawnOffset)
            {
                var prefab = _chunks[Random.Range(0, _chunks.Count)];
                ChunkActor chunk = GameObject.Instantiate(prefab);

                chunk.Init(EcsWorld);

                var chunkEntity = chunk.GetEntity();
                chunkEntity.Get<TransformRef>().Transform.position =
                    lastChunkEntity.Get<ChunkComponent>().EndSP.position -
                    chunkEntity.Get<ChunkComponent>().StartSP.position;

                _spawnedChunks.Add(chunk);
            }
        }
        if (_spawnedSaveZone == null) return;
        if (firstChunkEntity.Get<ChunkComponent>().EndSP.position.x <
        _spawnedSaveZone.GetEntity().Get<ChunkComponent>().StartSP.position.x &&
    _spawnedSaveZone.GetEntity().Get<ChunkComponent>().EndSP.position.x <
        playerEntity.Get<TransformRef>().Transform.position.x)
        {
            for (int i = _spawnedChunks.Count - 1; i >= 0; i--)
            {
                var chunk = _spawnedChunks[i];

                if (chunk == _spawnedSaveZone)
                    continue;

                var entity = chunk.GetEntity();

                if (entity.Get<ChunkComponent>().StartSP.position.x <
                    _spawnedSaveZone.GetEntity().Get<ChunkComponent>().EndSP.position.x)
                {
                    GameObject.Destroy(entity.Get<TransformRef>().Transform.gameObject);
                    entity.Destroy();
                    _spawnedChunks.RemoveAt(i);
                }
            }

            if (playerEntity.Get<TransformRef>().Transform.position.x >
                _spawnedSaveZone.GetEntity().Get<ChunkComponent>().EndSP.position.x)
            {
                _spawnedSaveZone = null;
            }

            return;
        }
    }
}
