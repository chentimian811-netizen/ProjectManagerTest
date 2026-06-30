using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI面板基类
/// 自动查找当前面板子物体的UI控件 统一管理相关事件
/// </summary>
public class BasePanel : MonoBehaviour
{   
    //定义一个字典 保持当前面板下所有找到的UI控件
    private Dictionary<string,List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();


    /// <summary>
    /// 隐藏面板时间
    /// </summary>
    [HideInInspector] public float HideMeTime = 0;

    //Unity生命周期函数
    //脚本实例再加载时会自动执行
    protected virtual void Awake()
    {  
       //Button
       FindChildrenControl<Button>(); 

       //Image
       FindChildrenControl<Image>(); 

       //Text
       FindChildrenControl<Text>(); 

       //ScrollRect
       FindChildrenControl<ScrollRect>(); 

       //Slider
       FindChildrenControl<Slider>(); 

       //Toggle
       FindChildrenControl<Toggle>(); 

       //InputField
       FindChildrenControl<InputField>(); 

       //TMP文件
       FindChildrenControl<TextMeshProUGUI>();

       //TMP输入
       FindChildrenControl<TMP_InputField>();

       //ToggleGroup
       FindChildrenControl<ToggleGroup>();

       //RawImage
       FindChildrenControl<RawImage>();

       //VerticalLayoutGroup
       FindChildrenControl<VerticalLayoutGroup>();

       //HorizontalLayoutGroup
       FindChildrenControl<HorizontalLayoutGroup>();

       //LayoutGroup
       FindChildrenControl<LayoutGroup>();
    }

    //按钮点击事件的处理方法
    protected virtual void OnClicked(string btnName)
    {
        
    }

    protected virtual void OnValueChanged(string btnName,bool value)
    {
        
    }
    private void FindChildrenControl<T>() where T : UIBehaviour
    {
        //获得当前物体以及所有子物体的类型为T的组件
        T[] controls = GetComponentsInChildren<T>();

        //遍历查找所有UI控件
        for(int i = 0; i < controls.Length; i++)
        {
            //string 用来保存当前空间所在GameObject的名字
            string objName;

            //获取当前控件所在的GameObject的名字
            objName = controls[i].gameObject.name;

            //判断字典中是否有这个控件的名字存在
            if (controlDic.ContainsKey(objName))
            {
                //如果存在 直接加入对于的表中
                controlDic[objName].Add(controls[i]);
            }
            else
            {
                //如果不存在 新建一条记录
                controlDic.Add(objName,new List<UIBehaviour>()
                {
                    controls[i]
                });
            }

            //判断当前控件是否为按钮
            if(controls[i] is Button)
            {
                (controls[i] as Button).onClick.AddListener(() => {OnClicked(objName);});
            }
            //判断toggle类型
            else if(controls[i] is Toggle)
            {
                (controls[i] as Toggle).onValueChanged.AddListener((value)=>{OnValueChanged(objName,value);});
            }
        }
    }

    protected T GetControl<T>(string controlName) where T : UIBehaviour
    {
        //判断字典中是否存在这个控件名字
        if (controlDic.ContainsKey(controlName))
        {
            //如果存在，就遍历整个名字对应的所有组件
            for(int i = 0; i < controlDic[controlName].Count; i++)
            {
                //判断当前组件是否是需要的类型T
                if(controlDic[controlName][i] is T)
                {
                    return controlDic[controlName][i] as T;
                }
            }
        }
        return null;
    }

    //
    public virtual void ShowMe()
    {
        
    }

    public virtual void HideMe()
    {
        
    }
}
