using Leopotam.Ecs;
using UnityEngine;

public class PlayerAniimationSystem: Injects, IEcsInitSystem, IEcsRunSystem
{
    private PlayerActor _player;
    private Animator _playerAnimator;
    private bool _isPlayerJumped = false;
    public void Init()
    {
        _player = SceneData.Player;
        _playerAnimator = _player.GetComponent<Animator>();
    }

    public void Run()
    {
        EcsEntity playerEntity = _player.GetEntity();
        if (playerEntity.Has<MoveFlag>())
        {
            _playerAnimator.SetBool("IsMove", true);
        }
        else if (!playerEntity.Has<MoveFlag>())
        {
            _playerAnimator.SetBool("IsMove", false);
        }

        if (playerEntity.Has<JumpFlag>())
        {
            _isPlayerJumped =true;
            _playerAnimator.SetTrigger("StartJump");
        }

        if (playerEntity.Has<IsGroundFlag>() && _isPlayerJumped)
        {
            _isPlayerJumped = false;
            _playerAnimator.SetTrigger("EndJump");
        }
    }
}
