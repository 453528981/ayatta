namespace Ayatta.Domain
{
    /// <summary>
    /// 订单类别
    /// </summary>
    public enum OrderType : byte
    {
        /// <summary>
        /// 网购订单
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 竞购订单
        /// </summary>
        Auction = 1,

        /// <summary>
        /// 竞购补差订单
        /// </summary>
        AuctionVariant = 2,

        /// <summary>
        /// 积分兑换订单
        /// </summary>
        IntegralExchange = 3

    }


    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderStatus : byte
    {
        /// <summary>
        /// 交易成功
        /// </summary>
        Finished = 0,

        /// <summary>
        /// 等待买家付款
        /// </summary>
        WaitBuyerPay = 1,

        /// <summary>
        /// 买家已付款等待卖家发货
        /// </summary>
        WaitSellerSend = 2,

        /// <summary>
        /// 卖家部分发货
        /// </summary>
        SellerSendPart = 3,

        /// <summary>
        /// 卖家已发货等待买家确认收货
        /// </summary>
        WaitBuyerConfirm = 4,

        /// <summary>
        /// 买家已签收并付完款 货到付款专用
        /// </summary>
        BuyerSigned = 5,

        /// <summary>
        /// 付款前 卖家或买家主动关闭交易
        /// </summary>
        Canceled = 7,

        /// <summary>
        /// 付款后退款成功交易自动关闭
        /// </summary>
        Closed = 8,

        /// <summary>
        /// 删除
        /// </summary>
        Deleted = 9
    }

    /// <summary>
    /// 订单兑换
    /// </summary>
    public enum OrderExchange : byte
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,

        /// <summary>
        /// 兑换为金币
        /// </summary>
        Coin = 1,

        /// <summary>
        /// 兑换为积分
        /// </summary>
        Point = 2,

    }

}

