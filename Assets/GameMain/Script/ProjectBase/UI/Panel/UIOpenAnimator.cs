using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

//强制要求 必须挂载RectTransform CanvasGroup 如果没有就自动添加
[RequireComponent(typeof(RectTransform),typeof(CanvasGroup))]
public class UIOpenAnimator : MonoBehaviour
{   
    [Header("动画设置")]
    //初始的缩放比例
    public float startScale = 0.6f;

    //初始的缩放时间
    public float duration = 0.4f;

    //缩放动画的缓动曲线
    public Ease easing = Ease.OutBack;

    //透明度渐变动画使用的缓动曲线
    public Ease fadeEasing = Ease.OutCubic;

    public RectTransform rectTrasnform;
    public CanvasGroup canvasGroup;

    void Awake()
    {
        rectTrasnform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }


    //UI传入动画
    public void Enter(Action onComplete = null)
    {   
        //将UI初始缩放设置为startScale
        rectTrasnform.localScale = Vector3.one * startScale;

        //将UI初始透明度设置为0
        canvasGroup.alpha = 0;

        Sequence seq = DOTween.Sequence();

        seq.Join(rectTrasnform.DOScale(1f,duration).SetEase(easing));

        seq.Join(canvasGroup.DOFade(1f,duration).SetEase(fadeEasing));

        seq.OnComplete(()=>onComplete?.Invoke());

    }

    //UI退出动画
    public void Exit(Action onComplete = null)
    {
        Sequence seq = DOTween.Sequence();

        seq.Join(rectTrasnform.DOScale(startScale,duration).SetEase(easing));
        seq.Join(canvasGroup.DOFade(0,duration).SetEase(fadeEasing));
        seq.OnComplete(()=>onComplete?.Invoke());
        
    }
}
