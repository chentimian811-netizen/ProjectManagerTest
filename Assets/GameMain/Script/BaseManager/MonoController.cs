using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Update中转站
public class MonoController : MonoBehaviour
{   

    //存放多个无参数 无返回值的方法
    private event UnityAction updateEvent;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {   
        //避免空引用抱错
        if(updateEvent != null) updateEvent.Invoke();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fun">添加事件的函数</param>
    public void AddUpdateListener(UnityAction fun)
    {   
        //把传入的fun加入updateEvent 保证以后每帧都会调用这个函数
        updateEvent += fun;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fun">移除这个事件的函数</param>
    public void RemoveUpdateListener(UnityAction fun)
    {
        updateEvent -= fun;
    }
}
