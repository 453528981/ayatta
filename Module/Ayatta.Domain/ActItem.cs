﻿using System;
using ProtoBuf;

namespace Ayatta.Domain
{
    ///<summary>
    /// 官方活动条目
    ///</summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class ActItem : IEntity<int>
    {
        #region Properties

        ///<summary>
        /// Id
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        /// 模块Id
        ///</summary>
        public int PlanId { get; set; }

        ///<summary>
        /// 分组Id
        ///</summary>
        public int GroupId { get; set; }

        ///<summary>
        /// 商品Id
        ///</summary>
        public int ItemId { get; set; }

        ///<summary>
        /// 商品SkuId
        ///</summary>
        public int SkuId { get; set; }

        ///<summary>
        /// 名称
        ///</summary>
        public string Name { get; set; }

        ///<summary>
        /// 标题
        ///</summary>
        public string Title { get; set; }

        ///<summary>
        /// 价格
        ///</summary>
        public decimal Price { get; set; }

        ///<summary>
        /// 链接
        ///</summary>
        public string Link { get; set; }

        ///<summary>
        /// 图标
        ///</summary>
        public string Icon { get; set; }

        ///<summary>
        /// 图片
        ///</summary>
        public string Picture { get; set; }

        ///<summary>
        /// 描述
        ///</summary>
        public string Summary { get; set; }

        ///<summary>
        /// 开始时间
        ///</summary>
        public DateTime StartedOn { get; set; }

        ///<summary>
        /// 结束时间
        ///</summary>
        public DateTime StoppedOn { get; set; }

        ///<summary>
        /// 排序优先级 从小到大
        ///</summary>
        public int Priority { get; set; }

        ///<summary>
        /// 徽章 标记
        ///</summary>
        public string Badge { get; set; }

        ///<summary>
        /// 扩展信息
        ///</summary>
        public string Extra { get; set; }

        ///<summary>
        /// 数据
        ///</summary>
        public string Data { get; set; }

        ///<summary>
        /// 卖家Id
        ///</summary>
        public int SellerId { get; set; }

        ///<summary>
        /// 卖家
        ///</summary>
        public string SellerName { get; set; }

        ///<summary>
        /// 状态 true可用 false不可用
        ///</summary>
        public bool Status { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        public DateTime CreatedOn { get; set; }

        ///<summary>
        /// 最后一次编辑者
        ///</summary>
        public string ModifiedBy { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        public DateTime ModifiedOn { get; set; }

        #endregion
    }


}
