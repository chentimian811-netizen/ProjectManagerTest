using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可视化描述的结构体
/// 用于存储物体需要展示参数 最后可以再Inspector面板进行配置
/// </summary>
[Serializable]
public class VisualEffectDesc
{
    public EcanSelectObject Flag;
    public EVsisualEffectFootTarfficState footTarfficState;
}
