using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Air.EventBus
{
    /// <summary>
    /// 事件总线服务
    /// </summary>
    public class EventBus : ReflectEventStore
    {

        public void Trigger<TEventData>(TEventData data, SortType sort = SortType.Asc) where TEventData : IEventData
        {
            // 这里如需保证顺序执行则必须循环两次 - -....
            var items = GetEventHandleList(data).ToList();
            Dictionary<object, Tuple<Type, int>> ds = new Dictionary<object, Tuple<Type, int>>();

            foreach (var item in items)
            {
                var instance = Activator.CreateInstance(item);
                MethodInfo method = item.GetMethod("get_ExecuteLevel");
                int value = (int)method.Invoke(instance, null);
                ds.Add(instance, new Tuple<Type, int>(item, value));
            }

            var lst = sort == SortType.Asc ? ds.OrderBy(p => p.Value.Item2).ToList() : ds.OrderByDescending(p => p.Value.Item2).ToList();

            foreach (var k in lst)
            {
                MethodInfo method = k.Value.Item1.GetMethod("Execute");
                method.Invoke(k.Key, new object[] { data });
            }
        }
    }

}
