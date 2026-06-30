using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Reflection;

public class UIStackManager
{
    private class PanelStackDate
    {
        //面板名称
        public string panelName;

        //面板所在的UI层级
        public E_UI_Layer layer;

        //面板的真实脚本类型
        public Type panelType;

        //面板显示完成后的回调
        public UnityAction<BasePanel> callback;

        //PanelStackData的构造函数
        public PanelStackDate(string panelName,E_UI_Layer layer ,Type panelType,UnityAction<BasePanel> callback)
        {
            this.panelName = panelName;
            this.layer = layer;
            this.panelType = panelType;
            this.callback = callback;
        }
    }

    private Stack<PanelStackDate> backStack = new Stack<PanelStackDate>();//返回栈
    private UIManager uiManager;
 

    public UIStackManager(UIManager manager)
    {
        uiManager = manager;
    }

        
    public void PushAndShowPanel<T>(string panelName,E_UI_Layer layer,UnityAction<T> callback = null) where T : BasePanel
    {
        string panelPath = $"Assets/GameMain/UI/{panelName}.perfab";

        //调用UIManager显示面板
        uiManager.PushAndShowPanle<T>(panelName,layer,(t) =>
        {
           if(backStack.Count == 0 || backStack.Peek().panelName != panelName)
            {
                   backStack.Push(new PanelStackDate(
                    panelName,
                    layer,
                    typeof(T),
                    panel => callback?.Invoke(panel as T)
                   ));
                Debug.LogWarning("将面板信息压入栈中" + panelName);
            }
            callback?.Invoke(t as T);
        });
    }
    
    public void OnBackButtonPressed()
    {
        if(backStack.Count <= 1)
        {
            Debug.LogWarning("没有上一个面板可以返回");
            return;
        }

        var currentPanel = backStack.Pop();

        uiManager.HidePanel(currentPanel.panelName);

        var previousPanel = backStack.Peek();

        try
        {
            MethodInfo method = uiManager.GetType().GetMethod("ShowPanle",BindingFlags.Public|BindingFlags.Instance)?.MakeGenericMethod(previousPanel.panelType);

            //判断是否成功获取到了ShowPanel方法
            if(method != null)
            {
                //创造回调委托
                var callbackDelegate = Delegate.CreateDelegate(typeof(UnityAction<>).MakeGenericType(previousPanel.panelType),
                previousPanel.callback.Target,previousPanel.callback.Method);

                //反射调用
                method.Invoke(uiManager,new object[]{
                    previousPanel.panelName,
                    previousPanel.layer,
                    callbackDelegate});

                    Debug.Log($"成功返回上一个面板:{previousPanel.panelName}");
            }
            else
            {
                Debug.Log("未找到匹配的ShowPanel<T>方法");
            }
        }
        catch(Exception ex)
        {
            Debug.LogError($"反射调用ShowPanle时发生错误:{ex.Message}\n{ex.StackTrace}");
        }
    }

    public void ClaerStack()
    {
        backStack.Clear();
        Debug.Log("UI栈已经清空");
    }
}
