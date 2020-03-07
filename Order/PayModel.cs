using System;
using System.Collections.Generic;
using System.Text;

namespace Order
{
    /// <summary>
    /// 支付模型
    /// </summary>
    public class PayModel
    {
        /// <summary>
        /// 支付记录ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal ActualMoney { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public PayTypeEnums PayType { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime PayTime { get; set; }
    }

    /// <summary>
    /// 支付方式
    /// </summary>
    public enum PayTypeEnums
    {
        Cash = 1,
        WeChat = 2,
        Zhifubao = 3
    }

}
