using Air.EventBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Order.Event
{
    /// <summary>
    /// 订单创建事件之消息处理类
    /// </summary>
    public class OrderCreateEventNotifyHandle : IEventHandle<IOrderCreateEventData>
    {
        public int ExecuteLevel { get; private set; }

        public OrderCreateEventNotifyHandle()
        {
            Console.WriteLine($"创建OrderCreateEventNotifyHandle对象");
            this.ExecuteLevel = 2;
        }

        public void Execute(IOrderCreateEventData eventData)
        {
            //这一块的逻辑处理代码应该迁移到采用MQ
            Thread.Sleep(1000);
            Console.WriteLine($"执行订单创建事件之消息推送!订单ID:{eventData.Order.Id.ToString()},商品名称:{eventData.Order.ProductName}");
        }
       
    }
}
