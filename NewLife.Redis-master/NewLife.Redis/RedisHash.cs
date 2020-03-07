﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NewLife.Data;

namespace NewLife.Caching
{
    /// <summary>哈希结构</summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class RedisHash<TKey, TValue> : RedisBase, IDictionary<TKey, TValue>
    {
        #region 构造
        /// <summary>实例化</summary>
        /// <param name="redis"></param>
        /// <param name="key"></param>
        public RedisHash(Redis redis, String key) : base(redis, key) { }
        #endregion

        #region 字典接口
        /// <summary>个数</summary>
        public Int32 Count => Execute(r => r.Execute<Int32>("HLEN", Key));

        Boolean ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        /// <summary>获取所有键</summary>
        public ICollection<TKey> Keys => Execute(r => r.Execute<TKey[]>("HKEYS", Key));

        /// <summary>获取所有值</summary>
        public ICollection<TValue> Values => Execute(r => r.Execute<TValue[]>("HVALS", Key));

        /// <summary>获取 或 设置 指定键的值</summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get => Execute(r => r.Execute<TValue>("HGET", Key, key));
            set => Execute(r => r.Execute<Int32>("HSET", Key, key, value), true);
        }

        /// <summary>是否包含指定键</summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Boolean ContainsKey(TKey key) => Execute(r => r.Execute<Boolean>("HEXISTS", Key, key));

        /// <summary>添加</summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value) => Execute(r => r.Execute<Int32>("HSET", Key, key, value), true);

        /// <summary>删除</summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Boolean Remove(TKey key) => HDel(key) > 0;

        /// <summary>尝试获取</summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Boolean TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);

            var pk = Execute(r => r.Execute<Packet>("HGET", Key, key));
            if (pk == null || pk.Total == 0) return false;

            value = FromBytes<TValue>(pk);

            return true;
        }

        /// <summary>清空</summary>
        public void Clear() => Redis.Remove(Key);

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

        Boolean ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) => ContainsKey(item.Key);

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, Int32 arrayIndex) => throw new NotSupportedException();

        Boolean ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);

        /// <summary>迭代</summary>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            var count = Count;
            if (count > 1000) throw new NotSupportedException($"[{Key}]的元素个数过多，不支持遍历！");

            return GetAll().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        #region 高级操作
        /// <summary>批量删除</summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public Int32 HDel(params TKey[] fields)
        {
            var args = new List<Object>
            {
                Key
            };
            foreach (var item in fields)
            {
                args.Add(item);
            }

            return Execute(r => r.Execute<Int32>("HDEL", args.ToArray()), true);
        }

        /// <summary>只在 key 指定的哈希集中不存在指定的字段时，设置字段的值</summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public TValue[] HMGet(params TKey[] fields)
        {
            var args = new List<Object>
            {
                Key
            };
            foreach (var item in fields)
            {
                args.Add(item);
            }

            return Execute(r => r.Execute<TValue[]>("HMGET", args.ToArray()));
        }

        /// <summary>批量插入</summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public Boolean HMSet(IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
        {
            var args = new List<Object>
            {
                Key
            };
            foreach (var item in keyValues)
            {
                args.Add(item.Key);
                args.Add(item.Value);
            }

            return Execute(r => r.Execute<Boolean>("HMSET", args.ToArray()), true);
        }

        /// <summary>获取所有名值对</summary>
        /// <returns></returns>
        public IDictionary<TKey, TValue> GetAll()
        {
            var rs = Execute(r => r.Execute<Packet[]>("HGETALL", Key));

            var dic = new Dictionary<TKey, TValue>();
            for (var i = 0; i < rs.Length; i++)
            {
                var key = FromBytes<TKey>(rs[i]);
                var value = FromBytes<TValue>(rs[++i]);
                dic[key] = value;
            }

            return dic;
        }

        /// <summary>增加指定字段值</summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Int64 HIncrBy(TKey field, Int64 value) => Execute(r => r.Execute<Int64>("HINCRBY", Key, field, value), true);

        /// <summary>增加指定字段值</summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Double HIncrBy(TKey field, Double value) => Execute(r => r.Execute<Double>("HINCRBY", Key, field, value), true);

        /// <summary>只在 key 指定的哈希集中不存在指定的字段时，设置字段的值</summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Int32 HSetNX(TKey field, TValue value) => Execute(r => r.Execute<Int32>("HSETNX", Key, field, value), true);

        /// <summary>返回hash指定field的value的字符串长度</summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public Int32 HStrLen(TKey field) => Execute(r => r.Execute<Int32>("HSTRLEN", Key, field));

        /// <summary>模糊搜索，支持?和*</summary>
        /// <param name="pattern"></param>
        /// <param name="count"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public virtual String[] Search(String pattern, Int32 count, ref Int32 position)
        {
            var p = position;
            var rs = Execute(r => r.Execute<Object[]>("HSCAN", Key, p, "MATCH", pattern + "", "COUNT", count));

            if (rs != null)
            {
                position = (rs[0] as Packet).ToStr().ToInt();

                var ps = rs[1] as Object[];
                var ss = ps.Select(e => (e as Packet).ToStr()).ToArray();
                return ss;
            }

            return null;
        }
        #endregion
    }
}