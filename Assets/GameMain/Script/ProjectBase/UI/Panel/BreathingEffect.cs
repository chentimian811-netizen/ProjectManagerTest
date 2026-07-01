using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//呼吸效果
public class BreathingEffec : MonoBehaviour
{
    [Header("缩放效果")]

    //最小的缩放值
    public float miniScale = 0.95f;

    //最大缩放值
    public float maxScale = 1.05f;

    //单词动画持续时间
    public float duration = 1.5f;

    public void Start()
    {   
        //无线循环
        transform.DOScale(maxScale,duration).SetEase(Ease.InOutSine).SetLoops(-1,LoopType.Yoyo);
    }
}
