using System;
using ProtoBuf;

namespace Ayatta.Domain
{
    ///<summary>
    /// Help
    /// created on 2016-07-14 20:06:59
    ///</summary>
    [ProtoContract]
    public class Help : IEntity<int>
    {
        ///<summary>
        /// Id
        ///</summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        ///<summary>
        /// 分组Id
        ///</summary>
        [ProtoMember(2)]
        public int GroupId { get; set; }

        ///<summary>
        /// 标题
        ///</summary>
        [ProtoMember(3)]
        public string Title { get; set; }

        ///<summary>
        /// 导航URL
        ///</summary>
        [ProtoMember(4)]
        public string NavUrl { get; set; }

        ///<summary>
        /// 内容
        ///</summary>
        [ProtoMember(5)]
        public string Content { get; set; }

        ///<summary>
        /// 排序优先级 从小到大
        ///</summary>
        [ProtoMember(6)]
        public int Priority { get; set; }

        ///<summary>
        /// 扩展信息
        ///</summary>
        [ProtoMember(7)]
        public string Extra { get; set; }

        ///<summary>
        /// 状态 1可用 0不可用
        ///</summary>
        [ProtoMember(8)]
        public bool Status { get; set; }

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
}