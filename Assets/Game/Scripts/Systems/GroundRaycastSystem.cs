using UnityEngine;
using Leopotam.Ecs;

public class GroundRaycastSystem: Injects, IEcsInitSystem, IEcsRunSystem
{
    private Transform _stepPosition;
    private float _offset = 0.0001f;
    private float _groundDistanceTreshold;
    public void Init()
    {
        _stepPosition = SceneData.Player.GetEntity().Get<StepPositionRef>().StepPosition;
        _groundDistanceTreshold = GameConfig.InputConfig.GroundDistanceThreshold;
    }

    public void Run()
    {
        if (_stepPosition == null) return;
        RaycastHit2D hitInfo = Physics2D.Raycast(RaycastOffset(), Vector2.down, _groundDistanceTreshold);
        if (hitInfo.collider != null)
        {
            EcsWorld.NewEntity().Get<GroundRaycastEvent>().HitInfo = hitInfo;
        }
    }

    private Vector3 RaycastOffset() => (Vector2)_stepPosition.position + Vector2.down * _offset;
}
