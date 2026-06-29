using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.ComponentModel;


public class MonoManager : BaseManager<MonoManager>
{
    private MonoController controller;

    //1.创建一个物体叫做MonoController 并且给物体添加引用 
    public MonoManager()
    {
        GameObject obj = new GameObject("MonoController");
        controller = obj.AddComponent<MonoController>();
    }

    /// <summary>
    /// 添加帧更新事件函数
    /// </summary>
    /// <param name="fun">添加的事件函数</param>
    public void AddUpdateListener(UnityAction fun)
    {
        controller.AddUpdateListener(fun);
    }

    /// <summary>
    /// 移除帧更新事件函数
    /// </summary>
    /// <param name="fun">移除时间函数</param>
    public void RemoveUpdateListener(UnityAction fun)
    {
        controller.RemoveUpdateListener(fun);
    }


    /// <summary>
    /// 只用于开启Controller内部的协程
    /// </summary>
    /// <param name="methodName"></param>
    /// <returns></returns>
    public Coroutine StartCoroutine(string methodName)
    {
        return controller.StartCoroutine(methodName);
    }
    public Coroutine StartCoroutine(string methodName ,[DefaultValue("null")]object value)
    {
        return controller.StartCoroutine(methodName , value);
    }

    //开启协程
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return controller.StartCoroutine(routine);
    }

    //停止协程
    public void StopCoroutine(IEnumerator routine)
    {
        controller.StopCoroutine(routine);
    }

    public void StopCoroutine(Coroutine routine)
    {
        controller.StopCoroutine(routine);
    }

}
