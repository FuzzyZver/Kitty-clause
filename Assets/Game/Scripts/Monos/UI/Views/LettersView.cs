using UnityEngine;
using Leopotam.Ecs;
using System.Collections.Generic;
using DG.Tweening;

public class LettersView : MonoBehaviour
{
    [SerializeField] private List<LetterView> _letters;
    private EcsWorld _world;
    private List<EcsEntity> _cats = new List<EcsEntity>();
    public void Init(EcsWorld world, List<EcsEntity> cats)
    {
        _world = world;
        _cats = cats;
        for (int i = 0; i < _letters.Count; i++)
        {
            _letters[i].GetCats(_cats[i]);
        }
    }

    public void WishlistUpdate(List<EcsEntity> cats)
    {
        _cats = cats;
    }
}
