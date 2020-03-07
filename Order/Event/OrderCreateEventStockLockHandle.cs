using Air.EventBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Order.Event
{
    /// <summary>
    /// 订单创建事件 锁定库存 处理类
    /// </summary>
    public class OrderCreateEventStockLockHandle : IEventHandle<IOrderCreateEventData>
    {
        public int ExecuteLevel { get; private set; }

        public OrderCreateEventStockLockHandle()
        {
            Console.WriteLine($"创建OrderCreateEventStockLockHandle对象");
            this.ExecuteLevel = 1;
        }


        public void Execute(IOrderCreateEventData eventData)
        {
            Thread.Sleep(1000);
            Console.WriteLine($"执行订单创建事件之库存锁定!订单ID:{eventData.Order.Id.ToString()},商品名称:{eventData.Order.ProductName}");
        }
    }
}
