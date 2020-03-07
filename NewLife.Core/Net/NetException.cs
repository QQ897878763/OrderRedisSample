﻿using System;

namespace NewLife.Net
{
    /// <summary>网络异常</summary>
    [Serializable]
    public class NetException : XException
    {
        #region 构造
        /// <summary>初始化</summary>
        public NetException() { }

        /// <summary>初始化</summary>
        /// <param name="message"></param>
        public NetException(String message) : base(message) { }

        /// <summary>初始化</summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public NetException(String format, params Object[] args) : base(format, args) { }

        /// <summary>初始化</summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public NetException(String message, Exception innerException) : base(message, innerException) { }

        /// <summary>初始化</summary>
        /// <param name="innerException"></param>
        public NetException(Exception innerException) : base((innerException?.Message), innerException) { }
        #endregion
    }
}