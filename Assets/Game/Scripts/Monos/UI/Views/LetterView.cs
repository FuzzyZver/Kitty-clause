using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Leopotam.Ecs;
using TMPro;

public class LetterView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private Sprite _openEnvelopeSprite;
    [SerializeField] private Sprite _closeEnvelopeSprite;
    [SerializeField] private Sprite _letterSprite;
    private EcsEntity _cat;

    [Header("Animation properties")]
    [SerializeField] private RectTransform _rect;
    [SerializeField] private Image _image;
    [SerializeField] private Image _letterImage;
    [SerializeField] private RectTransform _letterRect;
    [SerializeField] private Image _catImage;
    [SerializeField] private Image _giftImage;
    [SerializeField] private TextMeshProUGUI _senderName;
    private Vector2 _sizeUp = new Vector2(450f, 300f);
    private Vector2 _sizeDefault = new Vector2(400f, 250f);
    private Vector2 _defaultLetterScale = new Vector2(335f, 475f);
    private Vector2 _hideLetterScale = new Vector2(335f, 100f);

    private Tween _tween;
    private Sequence _letterSequence;

    public void GetCats(EcsEntity cat)
    {
        _cat = cat;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _tween?.Kill();
        _tween = _rect
            .DOSizeDelta(_sizeUp, 0.25f)
            .SetEase(Ease.OutBack);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _tween?.Kill();
        _tween = _rect
            .DOSizeDelta(_sizeDefault, 0.25f)
            .SetEase(Ease.OutBack);
    }

    public void OpenLetter()
    {
        _letterImage.gameObject.SetActive(true);
        _image.sprite = _openEnvelopeSprite;
        _letterSequence?.Kill();
        _letterSequence = DOTween.Sequence()
            .Append(_letterRect.DOSizeDelta(_defaultLetterScale, 0.5f));
        _giftImage.sprite = _cat.Get<CatCharComponent>().GiftSprite;
        _catImage.sprite = _cat.Get<CatCharComponent>().CatSprite;
        _senderName.text = _cat.Get<CatCharComponent>().CatName;
        _letterImage.sprite = _letterSprite;
    }

    public void CloseLetter()
    {
        _image.sprite = _closeEnvelopeSprite;
        _letterSequence?.Kill();
        _letterSequence = DOTween.Sequence()
            .Append(_letterRect.DOSizeDelta(_hideLetterScale, 0.5f));
        _letterImage.gameObject.SetActive(false);
    }
}
