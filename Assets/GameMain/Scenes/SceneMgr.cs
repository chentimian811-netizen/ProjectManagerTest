using System.Collections;
using System.Collections.Generic;
using ProjectBase.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using YooAsset;

/// <summary>
/// 全局单列 场景管理器
/// 基于YooAsset框架加载打包后的场景资源
/// 对外提供异步加载的接口，能够同步发送加载进度给UI进度条
/// 继承BaseManager单列模板
/// </summary>
public class SceneMgr : BaseManager<SceneMgr>
{
    private ResourcePackage _package;

    public SceneMgr()
    {
        //根据包名获得唯一的DefaultPackage资源包
        _package = YooAssets.GetPackage("DefaultPackage");
    }

    /// <summary>
    /// 对外暴露的异步加载场景方法
    /// </summary>
    /// <param name="name"></param>
    /// <param name="onComplete"></param>
    public void LoadSceneAsync(string name,UnityAction onComplete)
    {
        //调用全局协程管理器启动加载场景协程
        MonoManager.GetInstance().StartCoroutine(LoadSceneAsyncIE(name,onComplete));
    }

    /// <summary>
    /// 内部协程 执行YooAsset 异步场景加载逻辑
    /// 加载过程中每帧发送加载进度事件 供UI进度条读取显示
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    private IEnumerator LoadSceneAsyncIE(string name, UnityAction action)
    {
        //拼接场景再YooAsset内的完整资源路径
        string location = $"Assets/GameMain/Scenes/{name}.unity";

        //single 单场景模式 加载后销毁所有旧场景
        //Additive 叠加场景 不会销毁原场景 一般用于游戏副本
        var sceneMode = UnityEngine.SceneManagement.LoadSceneMode.Single;

        //局部物理模式 None为不开启独立物理世界 使用同一套物理引擎
        var physicsMode = LocalPhysicsMode.None;

        //是否暂停加载 false正常加载 true暂停加载
        bool suspendLoad = false;

        //调用YooAssret接口发起异步场景加载 返回场景加载句柄
        //句柄包含加载进度 是否完成 场景实例等全部信息
        SceneHandle handle = _package.LoadSceneAsync(location,sceneMode,physicsMode,suspendLoad);

        //循环 只需要场景没有加载完成 每帧执行一次
        while (!handle.IsDone)
        {
            //事件中心发送场景加载进度 把当前进度发送给UI层
            //UI进度条监听这个事件 读取Progress 进度条界面
            EventCenter.GetInstance().Send(
                SceneLoadingEventArgs.EventId,
                SceneLoadingEventArgs.Create(handle.Progress)
            );
            //暂停协程 等到下一帧再继续循环 不阻塞主线程
            yield return null;
        }
        
        yield return handle;

        //加载完成 执行外部传入回调函数 避免没有传回掉时报空指针
        action?.Invoke();

        //打印日志 输出当前加载完成多的场景名称，方便调式
        Debug.Log($"Scene name is {handle.SceneName}");
    }
}
