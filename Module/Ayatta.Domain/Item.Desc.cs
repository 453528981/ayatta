using System;
using ProtoBuf;

namespace Ayatta.Domain
{
    ///<summary>
    /// 商品描述
    ///</summary>
    [ProtoContract]
    public class ItemDesc : IEntity<int>
    {

        ///<summary>
        /// 商品Id
        ///</summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        ///<summary>
        /// 商品详情
        ///</summary>
        [ProtoMember(2)]
        public string Detail { get; set; }

        ///<summary>
        /// 使用指南
        ///</summary>
        [ProtoMember(3)]
        public string Manual { get; set; }

        ///<summary>
        /// 产品实拍
        ///</summary>
        [ProtoMember(4)]
        public string Photo { get; set; }

        ///<summary>
        /// 品牌故事
        ///</summary>
        [ProtoMember(5)]
        public string Story { get; set; }

        ///<summary>
        /// 使用须知
        ///</summary>
        [ProtoMember(6)]
        public string Notice { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        [ProtoMember(7)]
        public DateTime CreatedOn { get; set; }

        ///<summary>
        /// 最后一次编辑者
        ///</summary>
        [ProtoMember(8)]
        public string ModifiedBy { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        [ProtoMember(9)]
        public DateTime ModifiedOn { get; set; }
    }
}
