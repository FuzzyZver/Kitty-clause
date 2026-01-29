using UnityEngine;
using Leopotam.Ecs;

public class MovementSystem: Injects, IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter<MoveInputEvent> _moveInputEventFilter;
    private EcsFilter<EndGameEvent> _endGameEventFilter;
    private PlayerConfig _playerConfig;
    private InputConfig _inputConfig;
    private Vector2 _previousInput;
    private PlayerActor _playerRef;

    private Vector2 _endGameTargetPosition;

    public void Init()
    {
        _playerConfig = GameConfig.PlayerConfig;
        _inputConfig = GameConfig.InputConfig;
        _playerRef = SceneData.Player;
    }

    public void Run()
    {
        var playerEntity = _playerRef.GetEntity();
        if (playerEntity.Has<DeadFlag>()) return;
        if (playerEntity.Has<FreezeFlag>()) return;

        foreach (int i in _endGameEventFilter)
        {
            var lastChunk = _endGameEventFilter.Get1(i).LastChunk;
            _endGameTargetPosition = lastChunk.Get<ChunkComponent>().EndSP.position;
        }

        foreach (int i in _moveInputEventFilter)
        {
            Vector2 targetVector = _moveInputEventFilter.Get1(i).Vector2;
            Movement(playerEntity, targetVector);
        }

        if(!RealtimeData.IsGameEnd) return;
        Vector2 playerPos = playerEntity.Get<TransformRef>().Transform.position;
        if (playerEntity.Get<TransformRef>().Transform.position.x < _endGameTargetPosition.x)
        {
            Vector2 dir = (_endGameTargetPosition - playerPos).normalized;
            Movement(playerEntity, dir);
        }
    }

    private void Movement(EcsEntity playerEntity, Vector2 targetVector)
    {
        Vector2 lerpedInput = Vector2.Lerp(_previousInput, targetVector, _inputConfig.MoveInputGravity * Time.deltaTime);
        _previousInput = lerpedInput;
        lerpedInput.y = 0;
        Vector2 targetVelocity = new Vector2(lerpedInput.x * _playerConfig.Speed, lerpedInput.y);
        playerEntity.Get<RigidbodyRef>().Rigidbody2D.linearVelocity = targetVelocity;
        if (targetVelocity.x > 0)
        {
            playerEntity.Get<MoveFlag>();
        }
        else
        {
            playerEntity.Del<MoveFlag>();
        }
    }
}
