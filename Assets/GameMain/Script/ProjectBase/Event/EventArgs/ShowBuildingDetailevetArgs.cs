//用于建筑详情显示事件参数 用于通知界面展示制定建筑信息

public class ShowBuildingDetaileventArgs
{
    public static readonly string EventId = typeof(ShowOptimumRoadVFXArgs).GetHashCode().ToString();


    public static ShowBuildingDetaileventArgs Create()
    {
        var eventArgs = new ShowBuildingDetaileventArgs();
        return eventArgs;
    }

    
}