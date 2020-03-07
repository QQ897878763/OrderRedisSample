using System;
using System.Collections.Generic;
using System.Text;

namespace Air.EventBus
{
    /// <summary>
    /// 事件实现接口
    /// </summary>
    public interface IEventHandle<T> where T : IEventData
    {
        /// <summary>
        /// 处理等级
        /// 方便事件总线触发时候可以有序的执行相应
        /// </summary>
        /// <returns></returns>
        int ExecuteLevel { get; }

        /// <summary>
        /// 事件执行
        /// </summary>
        /// <param name="eventData">事件参数</param>
        void Execute(T eventData);
    }
}
