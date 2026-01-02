using UnityEngine;
using Leopotam.Ecs;
using UnityEngine.InputSystem;

public class GiftTakeHandsSystem: Injects, IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter<MouseInteractStartEvent> _mouseInteractStartEventFilter;
    private EcsFilter<MouseInteractEndEvent> _mouseInteractEndEventFilter;

    private Transform _currentGiftTransform;
    private Camera _camera;
    private float _magneticGiftForce;

    public void Init()
    {
        _camera = Camera.main;
        _magneticGiftForce = GameConfig.InputConfig.MagneticGiftForce;
    }

    public void Run()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 worldPos = _camera.ScreenToWorldPoint(mousePos);

        foreach (int i in _mouseInteractStartEventFilter)
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null)
            {
                GiftActor gift = hit.collider.GetComponent<GiftActor>();
                if (gift != null && gift.GetEntity().Has<GiftFlag>())
                {
                    _currentGiftTransform = gift.GetEntity().Get<TransformRef>().Transform;
                }
            }
        }
        foreach(int i in _mouseInteractEndEventFilter)
        {
            _currentGiftTransform = null;
        }

        if(_currentGiftTransform != null)
        {
            _currentGiftTransform.position = Vector2.Lerp(_currentGiftTransform.position, worldPos, _magneticGiftForce);
        }
    }
    
}
