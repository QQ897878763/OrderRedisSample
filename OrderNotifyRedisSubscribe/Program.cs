using System;
using NewLife.Caching;
using Order;
using Newtonsoft.Json;
using NewLife.Log;
using System.Threading;
using Air.EventBus;
using Order.Event;

namespace OrderNotifyRedisSubscribe
{
    class Program
    {
        static void Main(string[] args)
        {
            XTrace.UseConsole();
            Console.WriteLine("进入Redis消息订阅者模式订单消息推送订阅者客户端!");

            EventBus eventBus = new EventBus();
            eventBus.EventRegister(typeof(OrderCreateEventNotifyHandle), typeof(OrderCreateEventData));
            eventBus.EventRegister(typeof(OrderCreateEventStockLockHandle), typeof(OrderCreateEventData));

            FullRedis fullRedis = new FullRedis("127.0.0.1:6379", "", 1);
            fullRedis.Log = XTrace.Log;
            fullRedis.Timeout = 30000;
            OrderModel order = null;
            while (order == null)
            { 
                order = fullRedis.BLPOP<OrderModel>("orders", 20);
                if (order != null)
                {
                    Console.WriteLine($"得到订单信息:{JsonConvert.SerializeObject(order)}");
                    //执行相关事件
                    eventBus.Trigger(new OrderCreateEventData()
                    {
                        Order = order,
                    });
                    //再次设置为null方便循环读取
                    order = null;
                }
                  
            }
            Console.ReadLine();
        }
    }


}
