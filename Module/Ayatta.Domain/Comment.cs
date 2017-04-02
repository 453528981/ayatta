using System;
using ProtoBuf;
using System.Collections.Generic;


namespace Ayatta.Domain
{
    ///<summary>
    /// 商品评价详情
    ///</summary>
    [ProtoContract]
    public class Comment : IEntity<int>
    {
        #region

        ///<summary>
        /// Id
        ///</summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        ///<summary>
        /// 评分 1-5
        ///</summary>
        [ProtoMember(2)]
        public byte Score { get; set; }

        ///<summary>
        /// 内容
        ///</summary>
        [ProtoMember(3)]
        public string Content { get; set; }

        ///<summary>
        /// 买家印象标签
        ///</summary>
        [ProtoMember(4)]
        public string TagData { get; set; }

        ///<summary>
        /// 是否推荐
        ///</summary>
        [ProtoMember(5)]
        public bool Recommend { get; set; }

        ///<summary>
        /// 该评价被赞成总数
        ///</summary>
        [ProtoMember(6)]
        public int AgreedCount { get; set; }

        ///<summary>
        /// 该评价被回复总数
        ///</summary>
        [ProtoMember(7)]
        public int RepliedCount { get; set; }

        ///<summary>
        /// 奖励积分
        ///</summary>
        [ProtoMember(8)]
        public int PointReward { get; set; }

        ///<summary>
        /// 商品Id
        ///</summary>
        [ProtoMember(9)]
        public int ItemId { get; set; }

        ///<summary>
        /// 商品名称
        ///</summary>
        [ProtoMember(10)]
        public string ItemName { get; set; }

        ///<summary>
        /// 商品SkuId
        ///</summary>
        [ProtoMember(11)]
        public int SkuId { get; set; }

        ///<summary>
        /// 商品销售属性
        ///</summary>
        [ProtoMember(12)]
        public string SkuProp { get; set; }

        ///<summary>
        /// 用户Id
        ///</summary>
        [ProtoMember(13)]
        public int UserId { get; set; }

        ///<summary>
        /// 用户昵称
        ///</summary>
        [ProtoMember(14)]
        public string UserNickname { get; set; }

        ///<summary>
        /// 晒图 多个以","分隔
        ///</summary>
        [ProtoMember(15)]
        public string ImageData { get; set; }

        ///<summary>
        /// 赞成该评价的用户Id 多个以","分隔
        ///</summary>
        [ProtoMember(16)]
        public string AgreedData { get; set; }

        ///<summary>
        /// 商家Id
        ///</summary>
        [ProtoMember(17)]
        public int SellerId { get; set; }

        ///<summary>
        /// 订单Id
        ///</summary>
        [ProtoMember(18)]
        public string OrderId { get; set; }

        ///<summary>
        /// 状态 0为审核通过 1为审核未通过 2为删除
        ///</summary>
        [ProtoMember(19)]
        public byte Status { get; set; }

        ///<summary>
        /// pc wap iphone android
        ///</summary>
        [ProtoMember(20)]
        public string CreatedBy { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        [ProtoMember(21)]
        public DateTime CreatedOn { get; set; }

        ///<summary>
        /// 最后一次编辑者
        ///</summary>
        [ProtoMember(22)]
        public string ModifiedBy { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        [ProtoMember(23)]
        public DateTime ModifiedOn { get; set; }


        public virtual IList<Reply> Replies { get; set; }
        #endregion

        #region
        ///<summary>
        /// 商品评论回复
        /// created on 2016-09-01 20:09:51
        ///</summary>
        [ProtoContract]
        public class Reply : IEntity<int>
        {

            ///<summary>
            /// Id
            ///</summary>
            [ProtoMember(1)]
            public int Id { get; set; }

            ///<summary>
            /// 父级Id
            ///</summary>
            [ProtoMember(2)]
            public int ParentId { get; set; }

            ///<summary>
            /// 评价详情Id
            ///</summary>
            [ProtoMember(3)]
            public int CommentId { get; set; }

            ///<summary>
            /// 内容
            ///</summary>
            [ProtoMember(4)]
            public string Content { get; set; }

            ///<summary>
            /// 用户Id
            ///</summary>
            [ProtoMember(5)]
            public int UserId { get; set; }

            ///<summary>
            /// 用户昵称
            ///</summary>
            [ProtoMember(6)]
            public string UserNickname { get; set; }

            ///<summary>
            /// 状态 0为审核通过 1为审核未通过 2为删除
            ///</summary>
            [ProtoMember(7)]
            public byte Status { get; set; }

            ///<summary>
            /// pc wap iphone android
            ///</summary>
            [ProtoMember(8)]
            public string CreatedBy { get; set; }

            ///<summary>
            /// 创建时间
            ///</summary>
            [ProtoMember(9)]
            public DateTime CreatedOn { get; set; }

            ///<summary>
            /// 最后一次编辑者
            ///</summary>
            [ProtoMember(10)]
            public string ModifiedBy { get; set; }

            ///<summary>
            /// 最后一次编辑时间
            ///</summary>
            [ProtoMember(11)]
            public DateTime ModifiedOn { get; set; }

        }

        #endregion
    }

}
