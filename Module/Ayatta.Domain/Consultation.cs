using System;
using ProtoBuf;

namespace Ayatta.Domain
{
    ///<summary>
    /// 商品咨询
    ///</summary>
    [ProtoContract]
    public class Consultation : IEntity<int>
    {

        ///<summary>
        /// Id
        ///</summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        ///<summary>
        /// 商品SkuId
        ///</summary>
        [ProtoMember(2)]
        public int SkuId { get; set; }

        ///<summary>
        /// 商品Id
        ///</summary>
        [ProtoMember(3)]
        public int ItemId { get; set; }

        ///<summary>
        /// 分组 0商品咨询 1库存配送 2支付问题 3发票保修
        ///</summary>
        [ProtoMember(4)]
        public byte GroupId { get; set; }

        ///<summary>
        /// 咨询
        ///</summary>
        [ProtoMember(5)]
        public string Question { get; set; }

        ///<summary>
        /// 用户Id
        ///</summary>
        [ProtoMember(6)]
        public int UserId { get; set; }

        ///<summary>
        /// 用户昵称
        ///</summary>
        [ProtoMember(7)]
        public string UserNickname { get; set; }

        ///<summary>
        /// 回复
        ///</summary>
        [ProtoMember(8)]
        public string Reply { get; set; }

        ///<summary>
        /// 回复者
        ///</summary>
        [ProtoMember(9)]
        public string Replier { get; set; }

        ///<summary>
        /// 回复者Id
        ///</summary>
        [ProtoMember(10)]
        public int ReplierId { get; set; }

        ///<summary>
        /// 回复时间
        ///</summary>
        [ProtoMember(11)]
        public DateTime RepliedOn { get; set; }

        ///<summary>
        /// 商家Id
        ///</summary>
        [ProtoMember(12)]
        public int SellerId { get; set; }

        ///<summary>
        /// 状态 0为已回复 1为待处理 2审核未通过无效
        ///</summary>
        [ProtoMember(13)]
        public byte Status { get; set; }

        ///<summary>
        /// pc wap iphone android
        ///</summary>
        [ProtoMember(14)]
        public string CreatedBy { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        [ProtoMember(15)]
        public DateTime CreatedOn { get; set; }

        ///<summary>
        /// 最后一次编辑者
        ///</summary>
        [ProtoMember(16)]
        public string ModifiedBy { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        [ProtoMember(17)]
        public DateTime ModifiedOn { get; set; }

    }
}
