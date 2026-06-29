//用于视觉效果事件参数集合 用于触发选择 道路显示 退出全部特效

//最优路径特效
public class ShowOptimumRoadVFXArgs
{
    public static readonly string EventId = typeof(ShowOptimumRoadVFXArgs).GetHashCode().ToString();
    public VisualEffectDesc VisualEffectDesc;

    public static ShowOptimumRoadVFXArgs Create(VisualEffectDesc VisualEffectDesc)
    {
        var e = new ShowOptimumRoadVFXArgs();
        e.VisualEffectDesc = VisualEffectDesc;
        return e;
    }

    
}
//显示选中特效
public class ShowSelectVFXArgs
{
    public static readonly string EventId = typeof(ShowOptimumRoadVFXArgs).GetHashCode().ToString();
    public VisualEffectDesc VisualEffectDesc;

    public static ShowOptimumRoadVFXArgs Create(VisualEffectDesc VisualEffectDesc)
    {
        var e = new ShowOptimumRoadVFXArgs();
        e.VisualEffectDesc = VisualEffectDesc;
        return e;
    }

}

public class QuitAllVFXArgs
{
    public static readonly string EventId = typeof(ShowOptimumRoadVFXArgs).GetHashCode().ToString();

    //创建事件参数对象，统一封装事件传递的数据
    public static QuitAllVFXArgs Create()
    {
        var e = new QuitAllVFXArgs();
        return e;
    }
}