using System;
using System.Collections.Generic;
using System.Text;

namespace Order
{
    public class UserModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户积分
        /// </summary>
        public long UserIntegral { get; set; }
    }
}