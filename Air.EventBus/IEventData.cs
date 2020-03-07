using System;
using System.Collections.Generic;
using System.Text;

namespace Air.EventBus
{
    /// <summary>
    /// 事件参数接口
    /// </summary>
    public interface IEventData
    {
        /// <summary>
        /// 事件源对象
        /// </summary>
        object Source { get; set; }

        ///// <summary>
        ///// 事件发生的数据
        ///// </summary>
        //TDataModel Data { get; set; }

        /// <summary>
        /// 事件发生时间
        /// </summary>
        DateTime Time { get; set; }
    }
}
