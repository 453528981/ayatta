using System;
using ProtoBuf;

namespace Ayatta.Domain
{
    ///<summary>
    /// OrderNote
    ///</summary>
    [ProtoContract]
    public class OrderNote : IEntity<int>
    {

        ///<summary>
        /// Id
        ///</summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        ///<summary>
        /// 类型
        ///</summary>
        [ProtoMember(2)]
        public int Type { get; set; }

        ///<summary>
        /// 订单Id
        ///</summary>
        [ProtoMember(3)]
        public string OrderId { get; set; }

        ///<summary>
        /// 主题
        ///</summary>
        [ProtoMember(4)]
        public string Subject { get; set; }

        ///<summary>
        /// 消息
        ///</summary>
        [ProtoMember(5)]
        public string Message { get; set; }

        ///<summary>
        /// 创建者 UserName
        ///</summary>
        [ProtoMember(6)]
        public string CreatedBy { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        [ProtoMember(7)]
        public DateTime CreatedOn { get; set; }

    }

}