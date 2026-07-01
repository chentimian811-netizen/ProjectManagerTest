using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

//管理当前选中的物体 加载可选物体的数据 鼠标点击选择逻辑 根据选择结果给出事件
public class GameManager : SingletonAutoMono<GameManager>
{
    //保存可选择物体的数据列表
    private SelectObjectListSO selectObjectListSO;

    //对外提高制只读访问
    public SelectObjectListSO SelectObjectListSO => SelectObjectListSO;

    //当前选择物体的标识
    private EcanSelectObjectFlag currenSelectObjectFlag;

    //对外提供当前选中物体标识的只读访问
    public EcanSelectObjectFlag CurrentSelectObjectFlag => currenSelectObjectFlag;

    private void Awake()
    {
        //加载SelectObjectListSO数据资产
        selectObjectListSO = ResMgr.GetInstance().Load<SelectObjectListSO>("Asset/GameMain/SO/SelectObjectListSO.asset");
    }

    private void Update()
    {
        //判断鼠标左键是否在当前帧按下
        if (Input.GetMouseButtonDown(0))
        {
            Ray mainCameraRay = Camera.main!.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(mainCameraRay,out RaycastHit hitInfo, 1000.0f))
            {
                //判断被射线击中的物体是否挂载了VisualEffectController组件
                GameManager.GetInstance().ChangeSelectObjectByFlag();

                Debug.DrawRay
            }
        }
    }

    //根据物体标识切换当前选中的物体
    private void ChangeSelectObjectByFlag(EcanSelectObjectFlag objectFlag)
    {
        currenSelectObjectFlag = objectFlag;

        EventCenter.GetInstance().Send(ChangedSelectObjectEventAargs.EventId,ChangedSelectObjectEventAargs.Create(objectFlag));
    }

    public SelectObjectData GetSelectObjectDataByObjectFlag(EcanSelectObjectFlag buildType)
    {
        //遍历ListSO中的配置的所有可选择物体数据
        foreach(var data in SelectObjectListSO.selectObjects)
        {
            //判断Flag是否等于传入的Type
            if(data.VisualEffectDesc.Flag == buildType)
            {
                return data;
            }
        }
        return null;
    }

    public VideoClip GetCameraVideoByObjectFlags(EcanSelectObjectFlag buildType)
    {
        //根据物体的标识找到对应的Data
        var data = GetSelectObjectDataByObjectFlag(buildType);

        //如果
        
    }


    
}
