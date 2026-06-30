using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;
using UnityEngine.Events;

/// <summary>
/// 资源加载管理器
/// 单例全局管理器，统一管理资源
/// </summary>
public class ResMgr : BaseManager<ResMgr>
{
    /// <summary>
    /// 默认资源包实例(项目全局资源包Default Package)
    /// </summary>
    private ResourcePackage _package;

    public IEnumerator InitPackage(EPlayMode PlayMode)
    {
        // 初始化资源系统
        YooAssets.Initialize();

        // 创建默认的资源包
        _package = YooAssets.CreatePackage("DefaultPackage");

        // 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。
        YooAssets.SetDefaultPackage(_package);

        //空包检验 创建失败直接终止初始化流程
        if(_package == null)
        {
            Debug.LogWarning("资源包初始化失败，未设置默认包");
            yield break;
        }
        
        //根据运行模式区分初始化文件系统参数
        switch (PlayMode)
        {   
            ///编辑器模式 开发阶段使用 直接工程内读取资源文件
            ///可以直接开发调式
            case EPlayMode.EditorSimulateMode:
                {   
                    //自动弓箭编辑器模拟资源目录
                    var buildResult = EditorSimulateModeHelper.SimulateBuild("DefaultPackage");
                    //获取模拟资源根目录的路径
                    var packageRoot = buildResult.PackageRootDirectory;
                    //创建编辑器专用系统参数 指向模拟资源目录
                    var fileSystemParms = FileSystemParameters.CreateDefaultEditorFileSystemParameters(packageRoot);
                    
                    //编辑器模式初始化配置类
                    var createParmeters = new EditorSimulateModeParameters(); 
                    createParmeters.EditorFileSystemParameters = fileSystemParms;
                    //异步执行资源包初始化操作 等待完成
                    var initOperation = _package.InitializeAsync(createParmeters);
                    yield return initOperation;
                    
                    //判断初始化结果
                    if(initOperation.Status == EOperationStatus.Succeed)
                    {
                        Debug.Log("[编辑器模式]资源包初始化成功");
                    }
                    else
                    {
                        Debug.LogError("[编辑器模式]资源包初始化失败"); 
                    }
                    break;
                }
            //离线单机模式 读取内置StreamingAssets打包资源
            //无热更新 所有资源包内置安装包 无法远端热更新
            case EPlayMode.OfflinePlayMode:
                {
                    //创建内置资源文件系统参数 读取StreamingAssets内打包资源
                    var fileSystemParm = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();

                    //离线模式初始化配置类
                    var createParmeters = new OfflinePlayModeParameters();
                    createParmeters.BuildinFileSystemParameters = fileSystemParm;

                    //异步初始化资源
                    var initOperation = _package.InitializeAsync(createParmeters);

                    yield return initOperation;
                    
                    //判断初始化结果
                    if(initOperation.Status == EOperationStatus.Succeed)
                    {
                        Debug.Log("[离线单机模式]资源包初始化成功");
                    }
                    else
                    {
                        Debug.LogError("[离线单机模式]资源包初始化失败"); 
                    }
                    break;
                }
        }

        //======资源版本热更新流程=====
        //1.请求远端服务器最新资源包版本号
        var requestQperation = _package.RequestPackageVersionAsync();
        yield return requestQperation;

        if(requestQperation.Status == EOperationStatus.Succeed)
        {
            //请求成功，打印远端版本号
            string packageVersion = requestQperation.PackageVersion;
            Debug.Log($"拉取远端资源包版本:{packageVersion}");
        }
        else
        {
            //请求失败，直接终止初始化
            Debug.LogError($"拉去远端资源包版本失败:" + requestQperation.Error);
            yield break;
        }

        //2.根据远端最新的版本号 更新本地资源清单Manifest(记录所有路径 资源依赖 哈希)
        var updateOperation = _package.UpdatePackageManifestAsync(requestQperation.PackageVersion);
        yield return updateOperation;

        if(updateOperation.Status == EOperationStatus.Succeed)
        {
            Debug.Log("资源清单Manifes完成");
        }
        else
        {
            Debug.Log("资源清单Manifes更新失败" + updateOperation.Error);
        }
    }

    /// <summary>
    /// 管理器销毁入口：游戏退出 场景销毁
    /// 启动协程异步销毁资源包
    /// </summary>
    public void Ondestroy()
    {
        MonoManager.GetInstance().StartCoroutine(DestroyPackage());
    }

    private IEnumerator DestroyPackage()
    {
        //获取默认资源包
        var package = YooAssets.GetPackage("DefaultPackage");
        //异步销毁包 释放所有加载的资源 纹理 模型 GameObject。。。。
        DestroyOperation operation = package.DestroyAsync();
        yield return operation;

        bool removeResult = YooAssets.RemovePackage(package);
        if (removeResult)
        {
            Debug.Log("DufaultPackage资源包移除成功,内存已释放");
        }
    }
#region 同步加载接口
    /// <summary>
    /// 同步加载资源(无父物体 仅加载资源 GameObject会自动实例化)
    /// </summary>
    /// <typeparam name="T">资源类型：GameObject/Texture/Sprite</typeparam>
    /// <param name="path">YooAsset里的完整路径</param>
    /// <returns></returns>
    public T Load<T>(string path) where T : Object
    {
        return Load<T>(path,null);
    }

    /// <summary>
    /// 同步加载资源 支持指定实例化父物体 只对GameObject生效
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public T Load<T>(string path,Transform parent) where T : Object
    {
        //同步加载资源句柄
        var handle = _package.LoadAssetAsync<T>(path);

        //如果加载的是预制体的GameObject 直接实例化并设置父节点
        if(typeof(T) == typeof(GameObject))
        {
            GameObject go = handle.InstantiateSync();
            if(parent != null)
            {
                go.transform.SetParent(parent,false);
            }
            return go as T;
        }
        //纹理 音效 材质等资源直接返回资源对象 不实例化
        else
        {
            return handle.AssetObject as T;
        }
    }


#endregion

#region 异步加载接口
    /// <summary>
    /// 异步
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void LoadAsvnc<T>(string name,UnityAction<T> action) where T : Object
        {
            //胃痛Mono管理器开启协程处理异步加载
            MonoManager.GetInstance().StartCoroutine(LoadAsvncIE(name,action));
        }

    /// <summary>
    /// 异步加载内部协程实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    private IEnumerator LoadAsvncIE<T>(string path,UnityAction<T> action) where T : Object
    {
        //发起异步资源加载句柄
        AssetHandle handle = _package.LoadAssetAsync<T>(path);

        yield return handle;

        //获取原始资源对象
        T assetObject = handle.AssetObject as T;

        if(typeof(T) == typeof(GameObject))
        {
            GameObject go = handle.InstantiateSync();
            action?.Invoke(go as T);
            yield break;
        }
        action?.Invoke(assetObject);
    }
#endregion
}
