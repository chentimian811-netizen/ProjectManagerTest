using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 选择对象发生变化时使用的事件参数
/// </summary>
public class ChangedSelectObjectEventAargs
{
   //当前事件参数类对应的事件ID
   private static string Eventid = typeof(ChangedSelectObjectEventAargs).GetHashCode().ToString();

   //新的选中对象的标识
   public EcanSelectObjectFlag NewSelectObjectFlag {get;private set;}

   //创建ChangeSelectObjectEventAargs事件参数的方法
   public static ChangedSelectObjectEventAargs Create(EcanSelectObjectFlag NewSelectObjectFlag)
    {
        var e = new ChangedSelectObjectEventAargs();


        e.NewSelectObjectFlag = NewSelectObjectFlag;

        return e;
    }   
}
