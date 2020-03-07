using Air.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Event
{


    public interface IOrderCreateEventData : IEventData
    {
        /// <summary>
        /// 订单信息
        /// </summary>
        OrderModel Order { get; set; }
    }

    /// <summary>
    /// 订单创建事件参数
    /// </summary>
    public class OrderCreateEventData : IOrderCreateEventData
    {
        public OrderModel Order { get; set; }
        public object Source { get; set; }
        public DateTime Time { get; set; }
    }
}
