using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SelectObjectListSO",menuName = "ScriptableObjectListSO",order = 1)]
public class SelectObjectListSO : MonoBehaviour
{
    //环境温度
    [Range(00.0f,45.0f)]
    public float Temperature;

    //环境湿度
    [Range(30.0f,90.0f)]
    public float Humidity;

    public List<SelectObjectData> selectObjects = new List<SelectObjectData>();

}
