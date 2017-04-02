using System;
using ProtoBuf;

namespace Ayatta.Domain
{

    ///<summary>
    /// 商品评价
    ///</summary>
    [ProtoContract]
    public class ItemComment : IEntity<int>
    {
        ///<summary>
        /// 商品Id
        ///</summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        ///<summary>
        /// 商品SkuId
        ///</summary>
        [ProtoMember(2)]
        public int SkuId { get; set; }

        ///<summary>
        /// 平均得分
        ///</summary>
        [ProtoMember(3)]
        public int AvgScore { get; set; }

        ///<summary>
        /// 有晒图的评论总数
        ///</summary>
        [ProtoMember(4)]
        public int ImgCount { get; set; }

        ///<summary>
        /// 评论总数
        ///</summary>
        [ProtoMember(5)]
        public int SumCount { get; set; }

        ///<summary>
        /// 1分得票数
        ///</summary>
        [ProtoMember(6)]
        public int CountA { get; set; }

        ///<summary>
        /// 2分得票数
        ///</summary>
        [ProtoMember(7)]
        public int CountB { get; set; }

        ///<summary>
        /// 3分得票数
        ///</summary>
        [ProtoMember(8)]
        public int CountC { get; set; }

        ///<summary>
        /// 4分得票数
        ///</summary>
        [ProtoMember(9)]
        public int CountD { get; set; }

        ///<summary>
        /// 5分得票数
        ///</summary>
        [ProtoMember(10)]
        public int CountE { get; set; }

        ///<summary>
        /// 买家印象标签
        ///</summary>
        [ProtoMember(11)]
        public string TagData { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        [ProtoMember(12)]
        public DateTime CreatedOn { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        [ProtoMember(13)]
        public DateTime ModifiedOn { get; set; }

    }
}
