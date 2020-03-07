using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Air.EventBus
{
    /// <summary>
    /// 基于反射实现的事件仓储
    /// 存储事件处理对象和事件参数
    /// </summary>
    public class ReflectEventStore : IEventStore
    {
        private static Dictionary<Type, Type> StoreLst;

        public ReflectEventStore()
        {
            StoreLst = new Dictionary<Type, Type>();
        }

        public void EventRegister(Type handle, Type data)
        {
            if (handle == null || data == null) throw new NullReferenceException();
            if (StoreLst.Keys.Contains(handle))
                throw new ArgumentException($"事件总线已注册类型为{nameof(handle)} !");

            if (!StoreLst.TryAdd(handle, data))
                throw new Exception($"注册{nameof(handle)}类型到事件总线失败！");
        }


        public void EventUnRegister(Type handle)
        {
            if (handle == null) throw new NullReferenceException();
            StoreLst.Remove(handle);
        }

        public Type GetEventHandle(Type data)
        {
            if (data == null) throw new NullReferenceException();
            Type handle = StoreLst.FirstOrDefault(p => p.Value == data).Key;
            return handle;
        }

        public IEnumerable<Type> GetEventHandleList<TEventData>(TEventData data)
        {
            if (data == null) throw new NullReferenceException();
            var items = StoreLst.Where(p => p.Value == data.GetType())
                                  .Select(k => k.Key);
            return items;
        }
    }



    //public List<TEventHandle> GetEventHandleList<TEventHandle, TEventData>(TEventData data, SortType sort = SortType.Asc) where TEventHandle : IEventHandle<TEventData> where TEventData : IEventData
    //{

    //    if (data == null) throw new NullReferenceException();
    //    var items = StoreLst.Where(p => p.Value == data.GetType())
    //                          .Select(k => k.Key)
    //                          .Select(k =>
    //                          {
    //                              TEventHandle handle = (TEventHandle)Activator.CreateInstance(k);
    //                              return handle;
    //                          });
    //    var resilt = sort == SortType.Asc ? items.OrderBy(p => p.ExecuteLevel).ToList() : items.OrderByDescending(p => p.ExecuteLevel).ToList();
    //    return resilt;
    //}
}
