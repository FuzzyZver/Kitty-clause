using UnityEngine;
using Leopotam.Ecs;
using UnityEngine.InputSystem;

public class GiftTakeHandsSystem: Injects, IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter<MouseInteractStartEvent> _mouseInteractStartEventFilter;
    private EcsFilter<MouseInteractEndEvent> _mouseInteractEndEventFilter;
    private EcsFilter<TakeOfGiftEvent> _takeOffGiftEventFilter;

    private Transform _currentGiftTransform;
    private Rigidbody2D _currentGiftRigitbody2D;
    private Camera _camera;
    private float _magneticGiftForce;
    private float _maxGiftVelocity;

    public void Init()
    {
        _camera = Camera.main;
        _magneticGiftForce = GameConfig.InputConfig.MagneticGiftForce;
        _maxGiftVelocity = GameConfig.InputConfig.MaxGiftVelocity;
    }

    public void Run()
    {

        if (!UI.StartLevelView.gameObject.activeSelf)
        {
            if (RealtimeData.GiftBag.Get<GiftBagStorageComponent>().GiftsTypes.Count >= 6)
            {
                UI.StartLevelView.gameObject.SetActive(true);
                UI.StartLevelView.Init(EcsWorld);
            }
        }

        foreach (int i in _takeOffGiftEventFilter)
        {
            var giftsBag = _takeOffGiftEventFilter.Get1(i).GiftsBag.GetEntity();
            if (giftsBag.Get<GiftBagStorageComponent>().GiftsTypes.Count < 6)
            {
                var gift = _takeOffGiftEventFilter.Get1(i).Gift.GetEntity();
                giftsBag.Get<GiftBagStorageComponent>().GiftsTypes.Add(gift.Get<GiftTypeComponent>().GiftType);
                TakeOffGift();
            }
        }
        foreach (int i in _mouseInteractEndEventFilter)
        {
            TakeOffGift();
        }

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = _camera.ScreenPointToRay(mousePos);
        Plane plane = new Plane(Vector3.forward, Vector3.zero);
        if (!plane.Raycast(ray, out float enter))
            return;
        Vector3 worldPos3 = ray.GetPoint(enter);
        Vector2 worldPos = worldPos3;

        foreach (int i in _mouseInteractStartEventFilter)
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            if (hit.collider != null)
            {
                GiftActor gift = hit.collider.GetComponent<GiftActor>();
                if (gift != null && gift.GetEntity().Has<GiftFlag>())
                {
                    _currentGiftTransform = gift.GetEntity().Get<TransformRef>().Transform;
                    _currentGiftRigitbody2D = gift.GetEntity().Get<RigidbodyRef>().Rigidbody2D;
                    _currentGiftRigitbody2D.gravityScale = 0f;
                    _currentGiftRigitbody2D.linearVelocity = Vector2.zero;
                    _currentGiftRigitbody2D.angularVelocity = 0f;
                }
            }
        }

        if(_currentGiftRigitbody2D != null)
        {
            Vector2 targetVelocity = (worldPos - _currentGiftRigitbody2D.position) * _magneticGiftForce;
            targetVelocity = Vector2.ClampMagnitude(targetVelocity, _maxGiftVelocity);
            _currentGiftRigitbody2D.linearVelocity = targetVelocity;
        }
    }

    private void TakeOffGift()
    {
        if (_currentGiftRigitbody2D == null)
            return;

        _currentGiftRigitbody2D.gravityScale = 1f;
        _currentGiftRigitbody2D.linearVelocity = Vector2.zero;
        _currentGiftRigitbody2D = null;
        _currentGiftTransform = null;
    }
}
