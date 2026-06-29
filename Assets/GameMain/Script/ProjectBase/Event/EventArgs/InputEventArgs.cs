//用于输入事件参数 区分按下 持续按住和抬起三个状态

//表示这些类都属于ProjectBase.Inpur这个输入模块
namespace ProjectBase.Input
{
    //定义输入事件参数类 表示“输入持续触发”
    public class inputEventArgs
    {
        public static readonly string EventId = typeof(inputEventArgs).GetHashCode().ToString();

        public InputAction inputAction;
        public static inputEventArgs Create(InputAction action)
        {
            var eventArgs = new inputEventArgs();
            eventArgs.inputAction = action;
            return eventArgs;
        }
    }

    //定义输入事件参数类 表示“抬起”
    public class inputUpEventArgs
    {
        public static readonly string EventId = typeof(inputUpEventArgs).GetHashCode().ToString();

        public InputAction inputAction;
        public static inputUpEventArgs Create(InputUpAction action)
        {
            var eventArgs = new inputUpEventArgs();
            eventArgs.inputAction = action;
            return eventArgs;
        }
    }
    //定义输入事件参数类 表示“按下”
    public class inputDownEventArgs
    {
        public static readonly string EventId = typeof(inputDownEventArgs).GetHashCode().ToString();

        public InputAction inputAction;
        public static inputDownEventArgs Create(InputDownAction action)
        {
            var eventArgs = new inputDownEventArgs();
            eventArgs.inputAction = action;
            return eventArgs;
        }
    }
}