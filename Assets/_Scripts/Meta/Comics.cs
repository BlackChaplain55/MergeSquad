using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Comics : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private List<FrameData> framesData;
    public float DelayAfterComplete;
    public int CurrentStep;
    private Sequence _sequence;

    private void Start()
    {
        float delayForExtraTween = 0;

        _sequence = DOTween.Sequence(_sequence);
        for (CurrentStep = 0; CurrentStep < framesData.Count; CurrentStep++)
        {
            FrameData data = framesData[CurrentStep];
            _sequence.Append(data.GetTween());
            if (CurrentStep >= 7)
                continue;
            delayForExtraTween += data.Duration;
        }

        var extraTween = framesData[7];
        DOVirtual.DelayedCall(delayForExtraTween, () => extraTween.FrameTransform.DOScale(Vector3.one, extraTween.Duration).SetEase(extraTween.Ease));

        _sequence.OnComplete(() => DOVirtual.DelayedCall(DelayAfterComplete,
            () => GameController.Game.LoadScene(GameStates.Menu)));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _sequence.Complete();
    }
}

[Serializable]
public struct FrameData
{
    public RectTransform FrameTransform;
    public float Duration;
    public Ease Ease;
    public TweenAnimationType AnimationType;
    public Vector3 EndPosition;
    public float Intensity;
    public float SubParam;
    public float SubParamThird;
    public Tween CurrentTween;

    [Header("Audio")]
    public bool IsAllowedPlayAudio;
    public AudioClip Clip;
    public Vector3 InPosition;

    public Tween GetTween()
    {
        CurrentTween = default;
        if (IsAllowedPlayAudio)
            AudioSource.PlayClipAtPoint(Clip, InPosition);

        switch (AnimationType)
        {
            case TweenAnimationType.LocalMove:
                CurrentTween = FrameTransform.DOLocalMove(EndPosition, Duration);
                break;
            case TweenAnimationType.Rotate:
                CurrentTween = FrameTransform.DOLocalRotate(EndPosition, Duration);
                break;
            case TweenAnimationType.Scale:
                CurrentTween = FrameTransform.DOScale(EndPosition, Duration);
                break;
            case TweenAnimationType.Fade:
                var image = FrameTransform.GetComponent<Image>();
                var v3 = EndPosition;
                CurrentTween = image.DOColor(new Color(v3.x, v3.y, v3.z, Intensity), Duration);
                break;
            case TweenAnimationType.Punch:
                CurrentTween = FrameTransform.DOPunchPosition(EndPosition, Duration, (int)SubParam, SubParamThird);
                break;
            case TweenAnimationType.Shake:
                CurrentTween = FrameTransform.DOShakePosition(Duration, Intensity, (int)SubParam, SubParamThird);
                break;
        }
        CurrentTween.SetEase(Ease);
        var frm = FrameTransform;
        CurrentTween.OnStart(() => frm.gameObject.SetActive(true));
        return CurrentTween;
    }
}

public enum TweenAnimationType
{
    LocalMove,
    Rotate,
    Scale,
    Fade,
    Punch,
    Shake
}
