using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//定义泛型函数BaseManager<T>  泛型约束：无参构造函数 
public class BaseManager<T> where T : new() 
{   
    //定义泛型函数
    private static T instance;

    //受保护的构造函数 外部不能调用 但能被子类继承
    protected BaseManager(){  }

    public static T GetInstance()
    {
        if(instance == null)
        {
            instance = new T();
        }

        return instance;
    }
}
