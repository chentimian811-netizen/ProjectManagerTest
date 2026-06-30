using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static UnityEditor.Experimental.GraphView.GraphView;

//定义UI层级的枚举
public enum E_UI_Layer
{
    Bot,
    Mid,
    Top,
    System,
}

public class UIManager : BaseManager<UIManager>
{
    //字典保存已经加载出来的面板
    Dictionary<string,BasePanel> PanelDic = new Dictionary<string, BasePanel>();


    //保存正在异步加载的面板名字 防止同一个面板再异步加载还没有完成时被重复加载
    private List<string> LoadingPanelName = new List<string>();

    //Bot
    private Transform bot;
    //mid
    private Transform mid;
    //top
    private Transform top;
    //system
    private Transform system;

    //Canvas的Transform
    public RectTransform canvas;

    //UI栈管理器
    private UIStackManager uiStackManager;

    //UIManager
    public UIManager()
    {
        //创建UI栈管理器 并把当前UIManage传进去
        uiStackManager = new UIStackManager(this);

        //通过资源管理器加载Canvas预制体
        GameObject obj =  ResMgr.GetInstance().Load<GameObject>("Asset/GameMain/UI/Canvas.prefab");

        //将加载出来的Canvas对象的Transform转换成RectTransform
        RectTransform canvas = obj.transform as RectTransform;

        //设置Canvas 不会因为场景的变换而销毁
        GameObject.DontDestroyOnLoad(obj);

        //找到Canvas下的子物体
        bot = canvas.Find("Bot");
        mid = canvas.Find("Mid");
        top = canvas.Find("Top");
        system = canvas.Find("System");
        
        //通过资源管理器加载EventSystem预制体
        obj = ResMgr.GetInstance().Load<GameObject>("Asset/GameMain/UI/System.prefab");

        //设置EventSystem不会应为场景切换而销毁
        GameObject.DontDestroyOnLoad(obj);
    }

    public Transform GetLayerFather(E_UI_Layer layer)
    {
        switch (layer)
        {
            case E_UI_Layer.Bot:
                return this.bot;
            case E_UI_Layer.Mid:
                return this.mid;
            case E_UI_Layer.Top:
                return this.top;
            case E_UI_Layer.System:
                return this.system;
        }
        return null;
    }

    /// <summary>
    /// 将面板入栈显示
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="panelName"></param>
    /// <param name="layer"></param>
    /// <param name="callback"></param>
    public void PushAndShowPanle<T>(string panelName,E_UI_Layer layer,UnityAction<T> callback = null) where T : BasePanel
    {
        
    }


    /// <summary>
    /// 展示面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="panelName"></param>
    /// <param name="layer"></param>
    /// <param name="callback"></param>
    public void ShowPanel<T>(string panelName,E_UI_Layer layer,UnityAction<T>callback) where T : BasePanel
    {
        string panelPath = $"Assets/GameMain/UI/{panelName}.perfab";

        //要求 存在于面板字典中且为加载
        if (PanelDic.ContainsKey(panelName) && !LoadingPanelName.Contains(panelName))
        {
            if (PanelDic[panelName].gameObject.activeSelf)
            {
                return;
            }
            //如果面板已经加载过 但当前是隐藏状态我们就重新激活
            PanelDic[panelName].gameObject.SetActive(true);

            PanelDic[panelName].ShowMe();

            Transform father = GetLayerFather(layer);

            if(father != null)
            {
                PanelDic[panelName].transform.SetParent(father);
            }

            if(callback != null)
            {
                callback(PanelDic[panelName] as T);
            }

            return;
        }

        if (LoadingPanelName.Contains(panelName))
        {
            return;
        }
        else
        {
            LoadingPanelName.Add(panelName);
        }

        ResMgr.GetInstance().LoadAsvnc<GameObject>(panelName, (obj) =>
        {
            LoadingPanelName.Remove(panelName);

            if(obj == null)
            {
                Debug.Log($"\"{panelName}\"未加载成功");
                return;
            }

            Transform father = GetLayerFather(layer);

            if(father == null)
            {
                Debug.Log($"未找到UI层级父物体");
                return;
            }    

            obj.transform.SetParent(father);

            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            RectTransform rectTransform = obj.transform.GetComponent<RectTransform>();

            if(rectTransform != null)
            {
                rectTransform.offsetMax = Vector2.zero;
                rectTransform.offsetMin = Vector2.zero;
            }

            T panel = obj.GetComponent<T>();

            if(panel == null)
            {
                Debug.Log($"未获得组件{typeof(T)}");
                return;
            }

            panel.ShowMe();

            if(callback != null)
            {
                callback(panel);
            }

            PanelDic.Add(panelName,panel);
        });
    }

    public void HidePanel(string panelName,bool onlyhide = false)
    {
        string panelPath = $"Assets/GameMain/UI/{panelName}.perfab";

        //要求 不存在这个面板 说明这个面板没有被加载过
        if (!PanelDic.ContainsKey(panelName))
        {
            return;
        }
        PanelDic[panelName].HideMe();

        if (!onlyhide)
        {
            if(PanelDic[panelName].HideMeTime == 0)
            {
                GameObject.Destroy(PanelDic[panelName].gameObject);
            }
            else
            {
                GameObject.Destroy(PanelDic[panelName].gameObject,PanelDic[panelName].HideMeTime);
            }
             PanelDic.Remove(panelName);
            return;
        }
        //只隐藏面板
        if(PanelDic[panelName].HideMeTime == 0)
        {
            PanelDic[panelName].gameObject.SetActive(false);
        }
        else
        {
            MonoManager.GetInstance().StartCoroutine(Wait(PanelDic[panelName].HideMeTime,() =>
            {
                PanelDic[panelName].gameObject.SetActive(false);
            }));
        }
    }  
    /// <summary>
    /// 等待指定时间后执行回调
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator Wait(float delay,UnityAction callback)
    {
        yield return new WaitForSeconds(delay);

        callback?.Invoke(); 
    }

    /// <summary>
    /// 得到一个已经显示或加载的面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="panleName"></param>
    /// <returns></returns>
    public T GetPanel<T>(string panleName)where T : BasePanel
    {
        panleName = $"Assets/GameMain/UI/{panleName}.perfab";

        if (PanelDic.ContainsKey(panleName))
        {
            return PanelDic[panleName] as T;
        }
        return null;
    }

    public static void AddCustomEventListener(UIBehaviour control,EventTriggerType type,UnityAction<BaseEventData> callBack)
    {
        EventTrigger trigger = control.GetComponent<EventTrigger>();

        if (trigger == null)
        {
            trigger = control.gameObject.AddComponent<EventTrigger>();
        }
        
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callBack);
        trigger.triggers.Add(entry);
    }
}

