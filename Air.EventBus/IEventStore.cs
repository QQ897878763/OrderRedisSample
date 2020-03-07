using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Air.EventBus
{
 
    /// <summary>
    /// 排序类型
    /// </summary>
    public enum SortType
    {
        /// <summary>
        /// 升序
        /// </summary>
        Asc = 1,
        /// <summary>
        /// 降序
        /// </summary>
        Desc = 2
    }

    /// <summary>
    /// 事件仓库
    /// </summary>
    public interface IEventStore
    {
        /// <summary>
        /// 事件注册
        /// </summary>
        /// <param name="handle">事件实现对象</param>
        /// <param name="data">事件参数</param>
        void EventRegister(Type handle, Type data);

        /// <summary>
        /// 事件取消注册
        /// </summary>
        /// <param name="handle">事件实现对象</param>
        void EventUnRegister(Type handle);

        /// <summary>
        /// 获取事件处理对象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Type GetEventHandle(Type data);

        /// <summary>
        /// 根据事件参数获取事件处理集合
        /// </summary>
        /// <typeparam name="TEventData">事件参数类型</typeparam>
        /// <param name="data">事件参数</param>
        /// <returns></returns>
        IEnumerable<Type> GetEventHandleList<TEventData>(TEventData data);
    }
}
