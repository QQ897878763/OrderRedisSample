using Air.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Event
{
    /// <summary>
    /// 订单支付事件参数
    /// </summary>
    public class OrderPaymentEventData : IEventData
    {
        /// <summary>
        /// 支付信息
        /// </summary>
        public PayModel PayInfo { get; set; }

        /// <summary>
        /// 事件源对象
        /// </summary>
        public object Source { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime Time { get; set; }
    }
}
