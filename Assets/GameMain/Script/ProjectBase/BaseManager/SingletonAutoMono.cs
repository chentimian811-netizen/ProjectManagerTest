using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//新建GameObject的名字“typeof(T)” 如果场景里面有这个 这个函数不会被调用
public class SingletonAutoMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T GetInstance()
    {
        //查找场景中T组件
        if(instance == null)
        {
            instance = FindObjectOfType<T>();

            //如果没有 就新建一个
            if(instance == null)
            {
                GameObject singletonObject  = new GameObject(typeof(T).Name);
                instance = singletonObject.AddComponent<T>();

                DontDestroyOnLoad(instance.gameObject);
            }
        }
        return instance;
    }
}
