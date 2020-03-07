using System.Collections.Generic;
using Air.EventBus;
using Order.Event;
using System;
using System.Diagnostics;
using System.Threading;
using NewLife.Caching;
using Newtonsoft.Json;
using Order;

namespace OrderAsynSample
{
    class Program
    {
        /*
              框架需求: 
              1、开发EventBus,实现可通用化的事件总线;
              2、采用Redis实现发布订阅模式;

              业务需求:

              1、订单提交后，通知订阅了订单提交信息的后台工作人员;
              2、支付订单后, 减少库存、通知订阅了通知订单信息的工作人员
           */

        /*
         TODO 这里的流程应该是如下:
         1、将订单以Redis发布订阅模式下的发布者方式推送到指定通道供消费者消费;
         2、在订阅端在收到订单下单消息后触发事件总线，执行相应的事件处理;
         */

             
        static void Main(string[] args)
        {
            AsynEventHandle();

            // EventHandle();
        }

        /// <summary>
        /// 异步方式触发订单相关事件
        /// </summary>
        public static void AsynEventHandle()
        {
            Guid userId = Guid.NewGuid();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var order = new OrderModel()
            {
                CreateTime = DateTime.Now,
                Id = Guid.NewGuid(),
                Money = (decimal)300.00,
                Number = 1,
                ProductName = "鲜花一束",
                UserId = userId
            };
            Console.WriteLine($"模拟存储订单【采取Redis做消息队列的异步方式】");
            Thread.Sleep(1000);

            FullRedis fullRedis = new FullRedis("127.0.0.1:6379", "", 1);
            //这里尝试过使用redis 的订阅发布模式,在执行发布命令时候发现值但凡出现空格或者"符号则会异常...         
            fullRedis.LPUSH("orders", new OrderModel[] { order });

            stopwatch.Stop();
            Console.WriteLine($"下单总耗时:{stopwatch.ElapsedMilliseconds}毫秒");
            Console.ReadLine();
        }

        /// <summary>
        /// 同步方式触发订单相关事件
        /// </summary>
        public static void EventHandle()
        {
            Guid userId = Guid.NewGuid();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            EventBus eventBus = new EventBus();
            eventBus.EventRegister(typeof(OrderCreateEventNotifyHandle), typeof(OrderCreateEventData));
            eventBus.EventRegister(typeof(OrderCreateEventStockLockHandle), typeof(OrderCreateEventData));
            var order = new OrderModel()
            {
                CreateTime = DateTime.Now,
                Id = Guid.NewGuid(),
                Money = (decimal)300.00,
                Number = 1,
                ProductName = "鲜花一束",
                UserId = userId
            };
            Console.WriteLine($"模拟存储订单【同步方式】");
            Thread.Sleep(1000);
            //触发订单存储后相关事件
            eventBus.Trigger(new OrderCreateEventData()
            {
                Order = order,
            });
            stopwatch.Stop();
            Console.WriteLine($"下单总耗时:{stopwatch.ElapsedMilliseconds}毫秒");
            Console.ReadLine();
        }

     
    }
}
