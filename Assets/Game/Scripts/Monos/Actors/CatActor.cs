using System;
using Leopotam.Ecs;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CatActor: Actor
{
    [SerializeField] private Transform _catViewTransform;
    [SerializeField] private SpriteRenderer _catSprite;
    [SerializeField] private GameObject _leftDoor;
    [SerializeField] private GameObject _rightDoor;
    [SerializeField] private Animator _animator;
    public override void ExpandEntity(EcsEntity entity)
    {
        entity.Get<TransformRef>().Transform = _catViewTransform;
    }

    public void OpenDoor()
    {
        _leftDoor.SetActive(true);
        _rightDoor.SetActive(false);
    }
    public void SetCat(int catType, Sprite catSprite)
    {
        GetEntity().Get<CatTypeComponent>().CatType = catType;
        _catSprite.sprite = catSprite;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        PlayerActor player = other.GetComponent<PlayerActor>();
        if (player)
        {
            GetWorld().NewEntity().Get<CatInteractEvent>().CatActor = this;
            _animator.SetBool("IsPlayerHere", true);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        PlayerActor player = other.GetComponent<PlayerActor>();
        if (player)
        {
            GetWorld().NewEntity().Get<CatInteractEvent>().CatActor = this;
            _animator.SetBool("IsPlayerHere", false);
        }
    }
}
