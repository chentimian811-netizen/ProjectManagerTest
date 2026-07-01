using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

[RequireComponent(typeof(RectTransform),typeof(CanvasGroup))]
public class UISlectAnimator : MonoBehaviour
{
    [Header("动画设置")]

    //UI初始化设置
    public Vector2 offset = new Vector2(800,0);

    //动画持续时间
    public float duration = 0.5f;

    //透明度渐变动画使用的缓动曲线
    public Ease fadeEasing = Ease.OutCubic;

    public RectTransform rectTrasnform;
    public CanvasGroup canvasGroup;

    private Vector2 originalPosition;

    private void Awake()
    {
        rectTrasnform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        originalPosition = rectTrasnform.anchoredPosition;
    }

    //UI进入动画
    public void Enter(Action onCompelte = null)
    {
        //起始位置 偏移位置 并且透明
        rectTrasnform.anchoredPosition = originalPosition + offset;

        canvasGroup.alpha = 0;

        Sequence seq = DOTween.Sequence();

        seq.Join(rectTrasnform.DOAnchorPos(originalPosition,duration).SetEase(fadeEasing));
        seq.Join(canvasGroup.DOFade(1,duration).SetEase(fadeEasing));
        seq.OnComplete(()=>onCompelte?.Invoke());
    }

    //UI退出动画
    public void Exit(Action onComplete = null)
    {
        Sequence seq = DOTween.Sequence();

        seq.Join(rectTrasnform.DOAnchorPos(originalPosition+offset,duration).SetEase(fadeEasing));
        seq.Join(canvasGroup.DOFade(0,duration).SetEase(fadeEasing));
        seq.OnComplete(()=>onComplete?.Invoke());
    }
}
