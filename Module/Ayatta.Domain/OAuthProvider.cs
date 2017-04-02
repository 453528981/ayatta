using System;
using ProtoBuf;

namespace Ayatta.Domain
{
    ///<summary>
    /// OAuthProvider
    ///</summary>
    [ProtoContract]
    public class OAuthProvider : IEntity<string>
    {

        ///<summary>
        /// Id
        ///</summary>
        [ProtoMember(1)]
        public string Id { get; set; }

        ///<summary>
        /// 名称
        ///</summary>
        [ProtoMember(2)]
        public string Name { get; set; }

        ///<summary>
        /// ClientId
        ///</summary>
        [ProtoMember(3)]
        public string ClientId { get; set; }

        ///<summary>
        /// ClientSecret
        ///</summary>
        [ProtoMember(4)]
        public string ClientSecret { get; set; }

        ///<summary>
        /// Scope
        ///</summary>
        [ProtoMember(5)]
        public string Scope { get; set; }

        ///<summary>
        /// CallbackEndpoint
        ///</summary>
        [ProtoMember(6)]
        public string CallbackEndpoint { get; set; }

        ///<summary>
        /// BaseUrl
        ///</summary>
        [ProtoMember(7)]
        public string BaseUrl { get; set; }

        ///<summary>
        /// AuthorizationEndpoint
        ///</summary>
        [ProtoMember(8)]
        public string AuthorizationEndpoint { get; set; }

        ///<summary>
        /// TokenEndpoint
        ///</summary>
        [ProtoMember(9)]
        public string TokenEndpoint { get; set; }

        ///<summary>
        /// UserEndpoint
        ///</summary>
        [ProtoMember(10)]
        public string UserEndpoint { get; set; }

        ///<summary>
        /// 排序优先级 从小到大
        ///</summary>
        [ProtoMember(11)]
        public int Priority { get; set; }

        ///<summary>
        /// 徽章 标记
        ///</summary>
        [ProtoMember(12)]
        public string Badge { get; set; }

        ///<summary>
        /// 扩展信息
        ///</summary>
        [ProtoMember(13)]
        public string Extra { get; set; }

        ///<summary>
        /// 状态 1可用 0不可用
        ///</summary>
        [ProtoMember(14)]
        public bool Status { get; set; }

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