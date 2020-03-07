﻿using System;
using System.Net;
using System.Net.Sockets;
using NewLife.Data;
using NewLife.Net;

namespace NewLife.Http
{
    /// <summary>Http会话</summary>
    public class HttpSession : TcpSession
    {
        #region 属性
        /// <summary>是否WebSocket</summary>
        public Boolean IsWebSocket { get; set; }

        ///// <summary>是否启用SSL</summary>
        //public Boolean IsSSL { get; set; }

        /// <summary>请求</summary>
        public HttpRequest Request { get; set; }

        /// <summary>响应</summary>
        public HttpResponse Response { get; set; }
        #endregion

        #region 构造
        internal HttpSession(ISocketServer server, Socket client) : base(server, client)
        {
            Name = GetType().Name;
            //Remote.Port = 80;
            Remote.Type = server.Local.Type;
            Remote.EndPoint = client.RemoteEndPoint as IPEndPoint;

            //DisconnectWhenEmptyData = false;
            ProcessAsync = false;
            MatchEmpty = true;

            // 添加过滤器
            if (SendFilter == null) SendFilter = new HttpResponseFilter { Session = this };
        }
        #endregion

        #region 收发数据
        /// <summary>处理收到的数据</summary>
        /// <param name="pk"></param>
        /// <param name="remote"></param>
        protected override Boolean OnReceive(Packet pk, IPEndPoint remote)
        {
            if (pk == null || pk.Count == 0) return true;

            /*
             * 解析流程：
             *  首次访问或过期，创建请求对象
             *      判断头部是否完整
             *          --判断主体是否完整
             *              触发新的请求
             *          --加入缓存
             *      加入缓存
             *  触发收到数据
             */

            var header = Request;

            // 是否全新请求
            if (header == null || !IsWebSocket && (header.Expire < DateTime.Now || header.IsCompleted))
            {
                var req = new HttpRequest { Expire = DateTime.Now.AddSeconds(5) };

                // 分析头部
                if (req.ParseHeader(pk))
                {
                    Request = header = req;
                    Response = new HttpResponse();
#if DEBUG
                    WriteLog("{0} {1}", header.Method, header.Url);
#endif
                }
            }

            // 增加主体长度
            header.BodyLength += pk.Count;

            // WebSocket
            if (CheckWebSocket(ref pk, remote)) return true;

            if (!IsWebSocket && !header.ParseBody(ref pk)) return true;

            base.OnReceive(pk, remote);

            // 如果还有响应，说明还没发出
            var rs = Response;
            if (rs == null) return true;

            // 请求内容为空
            //var html = "请求 {0} 内容未处理！".F(Request.Url);
            var html = "{0} {1} {2}".F(Request.Method, Request.Url, DateTime.Now);
            Send(new Packet(html.GetBytes()));

            return true;
        }
        #endregion

        #region Http服务端
        class HttpResponseFilter : FilterBase
        {
            public HttpSession Session { get; set; }

            protected override Boolean OnExecute(FilterContext context)
            {
                var pk = context.Packet;
                var ss = Session;

                if (ss.IsWebSocket)
                    pk = HttpHelper.MakeWS(pk);
                else
                    pk = ss.Response.Build(pk);
                ss.Response = null;

                context.Packet = pk;
#if DEBUG
                //Session.WriteLog(pk.ToStr());
#endif

                return true;
            }
        }
        #endregion

        #region WebSocket
        /// <summary>检查WebSocket</summary>
        /// <param name="pk"></param>
        /// <param name="remote"></param>
        /// <returns></returns>
        protected virtual Boolean CheckWebSocket(ref Packet pk, IPEndPoint remote)
        {
            if (!IsWebSocket)
            {
                var key = Request["Sec-WebSocket-Key"];
                if (key.IsNullOrEmpty()) return false;

                WriteLog("WebSocket Handshake {0}", key);

                // 发送握手
                HttpHelper.Handshake(key, Response);
                Send(null);

                IsWebSocket = true;
                DisconnectWhenEmptyData = false;
            }
            else
            {
                pk = HttpHelper.ParseWS(pk);

                return base.OnReceive(pk, remote);
            }

            return true;
        }
        #endregion
    }
}