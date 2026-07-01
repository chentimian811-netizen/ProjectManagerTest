using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[Serializable]
public class SelectObjectData
{
    //可选对象的视觉特效描述数据
    public VisualEffectDesc VisualEffectDesc;

    //建筑物名称
    public string BuildingName;

    //建筑物描述信息
    public string BuildingDescription;

    //建筑物对应的图片资源
    public Sprite BuildingSprite;

    //区域摄像机视频片段
    public VideoClip AreaCameraVideoClip;
}
