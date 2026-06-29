using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

interface IEventInfo
{
    
}

//单参数
public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;

    //构造函数：创造EventInfo实例时初始化依赖
    public EventInfo(UnityAction<T> action)
    {
        action += action;
    }
}

//双参数
public class EventInfo<T,N> : IEventInfo
{
    public UnityAction<T,N> actions;

    //构造函数：创造EventInfo实例时初始化依赖
    public EventInfo(UnityAction<T,N> action)
    {
        action += action;
    }
}

//三参数
public class EventInfo<T,N,M> : IEventInfo
{
    public UnityAction<T,N,M> actions;

    //构造函数：创造EventInfo实例时初始化依赖
    public EventInfo(UnityAction<T,N,M> action)
    {
        action += action;
    }
}

//无参
public class EventInfo : IEventInfo
{
    public UnityAction actions;

    //构造函数：创造EventInfo实例时初始化依赖
    public EventInfo(UnityAction action)
    {
        action += action;
    }
}

//事件名称——>事件对应的回调函数集合
public class EventCenter : BaseManager<EventCenter>
{
    private Dictionary<string,IEventInfo> eventDic = new Dictionary<string, IEventInfo>();
    
    /// <summary>
    /// 添加事件监听
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name">事件的名字</param>
    /// <param name="action">处理事件的函数</param>
    public void AddEventListener<T>(string name,UnityAction<T> action)
    {
        //通过字典查询name 如果符合就获取这个名称所对应的回调函数集合
        if (eventDic.ContainsKey(name))
        {   
            //把新的回调函数追加进 对应的事件回调函数集合
            (eventDic[name] as EventInfo<T>).actions += action;
        }
        else
        {
            //如果没有查询到对应的事件名称 就直接在字典中新建一个
            eventDic.Add(name,new EventInfo<T>(action));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="N"></typeparam>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void AddEventListener<T,N>(string name,UnityAction<T,N> action)
    {
        //通过字典查询name 如果符合就获取这个名称所对应的回调函数集合
        if (eventDic.ContainsKey(name))
        {   
            //把新的回调函数追加进 对应的事件回调函数集合
            (eventDic[name] as EventInfo<T,N>).actions += action;
        }
        else
        {
            //如果没有查询到对应的事件名称 就直接在字典中新建一个
            eventDic.Add(name,new EventInfo<T,N>(action));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="N"></typeparam>
    /// <typeparam name="M"></typeparam>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void AddEventListener<T,N,M>(string name,UnityAction<T,N,M> action)
    {
        //通过字典查询name 如果符合就获取这个名称所对应的回调函数集合
        if (eventDic.ContainsKey(name))
        {   
            //把新的回调函数追加进 对应的事件回调函数集合
            (eventDic[name] as EventInfo<T,N,M>).actions += action;
        }
        else
        {
            //如果没有查询到对应的事件名称 就直接在字典中新建一个
            eventDic.Add(name,new EventInfo<T,N,M>(action));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void AddEventListener(string name,UnityAction action)
    {
        //通过字典查询name 如果符合就获取这个名称所对应的回调函数集合
        if (eventDic.ContainsKey(name))
        {   
            //把新的回调函数追加进 对应的事件回调函数集合
            (eventDic[name] as EventInfo).actions += action;
        }
        else
        {
            //如果没有查询到对应的事件名称 就直接在字典中新建一个
            eventDic.Add(name,new EventInfo(action));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name">触发名为name的事件</param>
    /// <param name="info"></param>
    public void Send<T>(string name,T info)
    {
        if (eventDic.ContainsKey(name))
        {
            if((eventDic[name] as EventInfo<T>).actions != null)
            {
                (eventDic[name] as EventInfo<T>).actions.Invoke(info);
            }
        }
    }
    public void Send<T,N>(string name,T infoT,N infoN)
    {
        if (eventDic.ContainsKey(name))
        {
            if((eventDic[name] as EventInfo<T,N>).actions != null)
            {
                (eventDic[name] as EventInfo<T,N>).actions.Invoke(infoT,infoN);
            }
        }
    }
    public void Send<T,N,M>(string name,T infoT,N infoN,M infoM)
    {
        if (eventDic.ContainsKey(name))
        {
            if((eventDic[name] as EventInfo<T,N,M>).actions != null)
            {
                (eventDic[name] as EventInfo<T,N,M>).actions.Invoke(infoT,infoN,infoM);
            }
        }
    }
    public void Send(string name)
    {
        if (eventDic.ContainsKey(name))
        {
            if((eventDic[name] as EventInfo).actions != null)
            {
                (eventDic[name] as EventInfo).actions.Invoke();
            }
        }
    }

    /// <summary>
    /// 移除对应的事件监听
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="action">对应之前添加的委托函数</param>
    public void RemoveEventListener<T>(string name,UnityAction<T> action)
    {   
        //通过字典查询NAme 如果符合就获得这个名称所
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions -= action;
        }
    }
    public void RemoveEventListener<T,N>(string name,UnityAction<T,N> action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T,N>).actions -= action;
        }
    }
    public void RemoveEventListener<T,N,M>(string name,UnityAction<T,N,M> action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T,N,M>).actions -= action;
        }
    }
    public void RemoveEventListener(string name,UnityAction action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions -= action;
        }
    }

    public void Clear()
    {
        eventDic.Clear();
    }
}
