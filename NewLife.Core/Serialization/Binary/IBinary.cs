﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NewLife.Serialization
{
    /// <summary>二进制序列化接口</summary>
    public interface IBinary : IFormatterX
    {
        #region 属性
        /// <summary>编码整数</summary>
        Boolean EncodeInt { get; set; }

        /// <summary>小端字节序。默认false大端</summary>
        Boolean IsLittleEndian { get; set; }

        /// <summary>使用指定大小的FieldSizeAttribute特性，默认false</summary>
        Boolean UseFieldSize { get; set; }

        /// <summary>要忽略的成员</summary>
        ICollection<String> IgnoreMembers { get; set; }

        ///// <summary>是否写入名称。默认false</summary>
        //Boolean UseName { get; set; }

        /// <summary>处理器列表</summary>
        IList<IBinaryHandler> Handlers { get; }
        #endregion

        #region 写入
        /// <summary>写入字节</summary>
        /// <param name="value"></param>
        void Write(Byte value);

        /// <summary>将字节数组部分写入当前流，不写入数组长度。</summary>
        /// <param name="buffer">包含要写入的数据的字节数组。</param>
        /// <param name="offset">buffer 中开始写入的起始点。</param>
        /// <param name="count">要写入的字节数。</param>
        void Write(Byte[] buffer, Int32 offset, Int32 count);

        /// <summary>写入大小</summary>
        /// <param name="size">要写入的大小值</param>
        /// <returns>返回特性指定的固定长度，如果没有则返回-1</returns>
        Int32 WriteSize(Int32 size);

        ///// <summary>写入名值对</summary>
        ///// <param name="name"></param>
        ///// <param name="value"></param>
        //Boolean WritePair(String name, Object value);
        #endregion

        #region 读取
        /// <summary>读取字节</summary>
        /// <returns></returns>
        Byte ReadByte();

        /// <summary>从当前流中将 count 个字节读入字节数组</summary>
        /// <param name="count">要读取的字节数。</param>
        /// <returns></returns>
        Byte[] ReadBytes(Int32 count);

        /// <summary>读取大小</summary>
        /// <returns></returns>
        Int32 ReadSize();

        ///// <summary>读取原始名值对</summary>
        ///// <returns></returns>
        //IDictionary<String, Byte[]> ReadPair();

        ///// <summary>从原始名值对读取数据</summary>
        ///// <param name="dic"></param>
        ///// <param name="name"></param>
        ///// <param name="type"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //Boolean TryReadPair(IDictionary<String, Byte[]> dic, String name, Type type, ref Object value);
        #endregion
    }

    /// <summary>二进制读写处理器接口</summary>
    public interface IBinaryHandler : IHandler<IBinary>
    {
    }

    /// <summary>二进制读写处理器基类</summary>
    public abstract class BinaryHandlerBase : HandlerBase<IBinary, IBinaryHandler>, IBinaryHandler
    {
    }
}