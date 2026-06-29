//用于场景加载事件参数 通过加载进度或加载状态变化

namespace ProjectBase.Input
{
    public class SceneLoadingEventArgs
    {
        public static readonly string EventId = typeof(inputEventArgs).GetHashCode().ToString();

        public float Progress;

        //创建事件参数对象 统一封装事件传递的数据
        public static SceneLoadingEventArgs Create(float progress)
        {
            var eventArg = new SceneLoadingEventArgs();
            eventArg.Progress = progress;
            return eventArg;
        }
    }
}