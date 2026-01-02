using UnityEngine;
using Leopotam.Ecs;

public class MovementSystem: Injects, IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter<MoveInputEvent> _moveInputEventFilter;
    private PlayerConfig _playerConfig;
    private InputConfig _inputConfig;
    private Vector2 _previousInput;
    private PlayerActor _playerRef;

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

        foreach (int i in _moveInputEventFilter)
        {
            var moveInputEvent = _moveInputEventFilter.Get1(i);
            Vector2 lerpedInput = Vector2.Lerp(_previousInput, moveInputEvent.Vector2, _inputConfig.MoveInputGravity * Time.deltaTime);
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
}
