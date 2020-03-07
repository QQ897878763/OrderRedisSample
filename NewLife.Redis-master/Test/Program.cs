﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NewLife.Caching;
using NewLife.Log;
using NewLife.Security;
using NewLife.Serialization;

namespace Test
{
    class Program
    {
        static void Main(String[] args)
        {
            XTrace.UseConsole();

            // 激活FullRedis，否则Redis.Create会得到默认的Redis对象
            FullRedis.Register();

            try
            {
                //TestHyperLogLog();
                Test1();
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }

            Console.ReadKey();
        }

        static void Test1()
        {
            var ic = Redis.Create("127.0.0.1:6379", 3);
            //var ic = new FullRedis();
            //ic.Server = "127.0.0.1:6379";
            //ic.Db = 3;
            ic.Log = XTrace.Log;

            // 简单操作
            Console.WriteLine("共有缓存对象 {0} 个", ic.Count);

            ic.Set("name", "大石头");
            Console.WriteLine(ic.Get<String>("name"));

            var ks = ic.Execute(null, c => c.Execute<String[]>("KEYS", "*"));
            var keys = ic.Keys;

            ic.Set("time", DateTime.Now, 1);
            Console.WriteLine(ic.Get<DateTime>("time").ToFullString());
            Thread.Sleep(1100);
            Console.WriteLine(ic.Get<DateTime>("time").ToFullString());

            // 列表
            var list = ic.GetList<DateTime>("list");
            list.Add(DateTime.Now);
            list.Add(DateTime.Now.Date);
            list.RemoveAt(1);
            Console.WriteLine(list[list.Count - 1].ToFullString());

            // 字典
            var dic = ic.GetDictionary<DateTime>("dic");
            dic.Add("xxx", DateTime.Now);
            Console.WriteLine(dic["xxx"].ToFullString());

            // 队列
            var mq = ic.GetQueue<String>("queue");
            mq.Add(new[] { "abc", "g", "e", "m" });
            var arr = mq.Take(3);
            Console.WriteLine(arr.Join(","));

            // 集合
            var set = ic.GetSet<String>("181110_1234");
            set.Add("xx1");
            set.Add("xx2");
            set.Add("xx3");
            Console.WriteLine(set.Count);
            Console.WriteLine(set.Contains("xx2"));

            Console.WriteLine("共有缓存对象 {0} 个", ic.Count);
        }

        /// <summary>性能压测</summary>
        static void Test2()
        {
            var ic = Redis.Create("127.0.0.1", 3);

            // 性能压测
            //ic.AutoPipeline = -1;
            ic.Bench();

            Thread.Sleep(1000);

            Console.WriteLine();
            var dic = ic.GetInfo();
            foreach (var item in dic)
            {
                Console.WriteLine("{0}:\t{1}", item.Key, item.Value);
            }
        }

        static void Test3()
        {
            var ic = Redis.Create("127.0.0.1:6379", 3);
            //ic.Log = XTrace.Log;

            var list = ic.GetList<String>("kkk");
            for (var i = 0; i < 100; i++)
            {
                list.Add(Rand.NextString(256));
            }
            ic.SetExpire("kkk", TimeSpan.FromSeconds(120));

            var arr = list.ToArray();
            Console.WriteLine(arr.Length);
            foreach (var item in arr)
            {
                Console.WriteLine(item);
            }
        }

        static void Test4()
        {
            var rds = Redis.Create("127.0.0.1:6001", 0);
            rds.Log = XTrace.Log;
            //rds.Init(null);

            Thread.Sleep(1000);

            var fr = rds as FullRedis;
            var cluster = fr.Cluster;

            cluster.Meet("127.0.0.1", 6002);
            cluster.Meet("127.0.0.1", 6003);
            cluster.Meet("127.0.0.1", 6004);

            Thread.Sleep(1000);

            cluster.Rebalance();

            rds.Set("name", "Stone");

            var name = rds.Get<String>("name");
        }

        class User
        {
            public String Name { get; set; }
            public DateTime CreateTime { get; set; }
        }
        static void Test5()
        {
            var user = new User { Name = "NewLife", CreateTime = DateTime.Now };
            var rds = Redis.Create("127.0.0.1", 2);
            rds.Log = XTrace.Log;
            rds.Set("user", user, 3600);
            var user2 = rds.Get<User>("user");
            XTrace.WriteLine("Json: {0}", user2.ToJson());
            XTrace.WriteLine("Json: {0}", rds.Get<String>("user"));
            if (rds.ContainsKey("user")) XTrace.WriteLine("存在！");
            rds.Remove("user");

        }

        static void Test6()
        {
            var str = @"1d30dccb6ef7daedd79884d4c4fd93cfaf848c17 172.16.10.32:6379@16379 myself,master - 0 1551270675000 1 connected 0-4095 [12222-<-0655825d6cb9148d5bfb9f68bdfb4e1651fac62e] [14960-<-eb2da2a40037265b9f21022d2c6e2ba00e91b67c]
2c24ef8cbe72ac2f987fb15a08d017c6aefe9fab 172.16.10.34:7000@17000 slave 9412d9fba8f7cb5c99f3a29f2abdda44dce8b506 0 1551270679509 8 connected
eb2da2a40037265b9f21022d2c6e2ba00e91b67c 172.16.10.32:7000@17000 master - 0 1551270680511 2 connected 12288-16383
0655825d6cb9148d5bfb9f68bdfb4e1651fac62e 172.16.10.34:6379@16379 master - 0 1551270678000 7 connected 8192-12287
88d1256ee4142e220516508b29b8ebdee80521a0 172.16.10.34:7001@17001 slave 1d30dccb6ef7daedd79884d4c4fd93cfaf848c17 0 1551270677504 9 connected
9412d9fba8f7cb5c99f3a29f2abdda44dce8b506 172.16.10.33:6379@16379 master - 0 1551270677000 4 connected 4096-8191
71cd78e1f4aeee042ea99925c72f0a943a061ed4 172.16.10.32:7001@17001 slave 0655825d6cb9148d5bfb9f68bdfb4e1651fac62e 0 1551270676000 7 connected
8b5e17020b0fcc742341c583518c2ab247b34afa 172.16.10.33:7001@17001 slave eb2da2a40037265b9f21022d2c6e2ba00e91b67c 0 1551270680000 6 connected
220fd8bd50d5329c5ac5b867991df12237f102ed 172.16.10.33:7000@17000 slave 1d30dccb6ef7daedd79884d4c4fd93cfaf848c17 0 1551270676000 5 connected
";

            var cluster = new RedisCluster(null);
            cluster.ParseNodes(str);
        }

        static void TestHyperLogLog()
        {
            var rds = Redis.Create("127.0.0.1", 1);

            rds.Remove("ips");
            var log = new HyperLogLog(rds, "ips");

            XTrace.WriteLine("log.Count={0:n0}", log.Count);

            var count = 1_000_000;
            XTrace.WriteLine("准备添加[{0:n0}]个IP地址", count);
            Parallel.For(0, count, k =>
            {
                var n = Rand.Next();
                var ip = new IPAddress(n);
                log.Add(ip + "");
            });
            XTrace.WriteLine("log.Count={0:n0}", log.Count);
        }
    }
}