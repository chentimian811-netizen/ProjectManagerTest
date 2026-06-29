using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//定义泛型单例类
public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T GetInstance()
    {

        //查找场景中T组件 并强制转换
        if(instance == null)
        {
            instance = (T)FindObjectOfType(typeof(T));
            
            //如果场景中没有 新建一个
            if(instance == null)
            {
                GameObject obj = new GameObject();
                instance = obj.AddComponent<T>();
            }

            //场景调转是保留这个object
            DontDestroyOnLoad(instance.gameObject);
        }


        return instance;
    }
}
