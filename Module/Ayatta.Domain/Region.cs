using System;
using ProtoBuf;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ayatta.Domain
{

    ///<summary>
    /// Region
    ///</summary>
    [ProtoContract]
    public class Region : IEntity<string>
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
        /// 上级Id 最上级为国家编码
        ///</summary>
        [ProtoMember(3)]
        public string ParentId { get; set; }

        ///<summary>
        /// 邮政编码
        ///</summary>
        [ProtoMember(4)]
        public string PostalCode { get; set; }

        ///<summary>
        /// 分组Id 可能会把某些省归入华北 华南这种地理大区
        ///</summary>
        [ProtoMember(5)]
        public int GroupId { get; set; }

        ///<summary>
        /// 排序优先级 从小到大
        ///</summary>
        [ProtoMember(6)]
        public int Priority { get; set; }

        ///<summary>
        /// 徽章 标记
        ///</summary>
        [ProtoMember(7)]
        public string Badge { get; set; }

        ///<summary>
        /// 扩展信息
        ///</summary>
        [ProtoMember(8)]
        public string Extra { get; set; }

        ///<summary>
        /// 状态 1可用 0不可用
        ///</summary>
        [ProtoMember(9)]
        public bool Status { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        [ProtoMember(10)]
        public DateTime CreatedOn { get; set; }

        ///<summary>
        /// 最后一次编辑者
        ///</summary>
        [ProtoMember(11)]
        public string ModifiedBy { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        [ProtoMember(12)]
        public DateTime ModifiedOn { get; set; }
    }

    /// <summary>
    /// 中国
    /// </summary>
    [ProtoContract]
    public class China
    {
        [ProtoMember(1)]
        public IList<Province> Provinces { get; set; }

        [ProtoContract]
        public class Province
        {
            /// <summary>
            /// Id
            /// </summary>
            [ProtoMember(1)]
            public string Id { get; set; }

            /// <summary>
            /// 名称
            /// </summary>
            [ProtoMember(2)]
            public string Name { get; set; }

            /// <summary>
            /// 邮政编码
            /// </summary>

            [ProtoMember(3)]
            public string PostalCode { get; set; }

            /// <summary>
            /// 市
            /// </summary>
            [ProtoMember(4)]
            public IList<City> Cities { get; set; }


            [ProtoContract]
            public class City
            {
                /// <summary>
                /// Id
                /// </summary>
                [ProtoMember(1)]
                public string Id { get; set; }

                /// <summary>
                /// 名称
                /// </summary>
                [ProtoMember(2)]
                public string Name { get; set; }

                /// <summary>
                /// 邮政编码
                /// </summary>

                [ProtoMember(3)]
                public string PostalCode { get; set; }

                /// <summary>
                /// 区
                /// </summary>

                [ProtoMember(4)]
                public IList<District> Districts { get; set; }


                [ProtoContract]
                public class District
                {
                    /// <summary>
                    /// Id
                    /// </summary>
                    [ProtoMember(1)]
                    public string Id { get; set; }

                    /// <summary>
                    /// 名称
                    /// </summary>
                    [ProtoMember(2)]
                    public string Name { get; set; }

                    /// <summary>
                    /// 邮政编码
                    /// </summary>

                    [ProtoMember(3)]
                    public string PostalCode { get; set; }


                }
            }


        }

    }

}