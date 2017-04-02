using System;
using ProtoBuf;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace Ayatta.Domain
{
    ///<summary>
    /// User
    ///</summary>
    [ProtoContract]
    public class User : IEntity<int>
    {
        ///<summary>
        /// Id
        ///</summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        ///<summary>
        /// Guid
        ///</summary>
        [ProtoMember(2)]
        public string Guid { get; set; }

        ///<summary>
        /// 登录帐号 用户名/绑定的邮箱/绑定的手机号
        ///</summary>
        [ProtoMember(3)]
        public string Name { get; set; }

        ///<summary>
        /// 绑定的邮箱
        ///</summary>
        [ProtoMember(4)]
        public string Email { get; set; }

        ///<summary>
        /// 绑定的手机号
        ///</summary>
        [ProtoMember(5)]
        public string Mobile { get; set; }

        ///<summary>
        /// 用户昵称
        ///</summary>
        [ProtoMember(6)]
        public string Nickname { get; set; }

        ///<summary>
        /// 用户登录密码
        ///</summary>
        [JsonIgnore]
        //[ProtoMember(7)]
        public string Password { get; set; }

        ///<summary>
        /// 用户角色
        ///</summary>
        [ProtoMember(8)]
        public UserRole Role { get; set; }

        ///<summary>
        /// 用户级别
        ///</summary>
        [ProtoMember(9)]
        public UserGrade Grade { get; set; }

        ///<summary>
        /// 用户限制
        ///</summary>
        [ProtoMember(10)]
        public UserLimitation Limitation { get; set; }

        ///<summary>
        /// 商家许可
        ///</summary>
        [ProtoMember(11)]
        public UserPermission Permission { get; set; }

        ///<summary>
        /// Avatar
        ///</summary>
        [ProtoMember(12)]
        public string Avatar { get; set; }

        ///<summary>
        /// 0正常 1未通过手机 邮箱验证 2被系统隔离 无法下单 3被系统禁用 帐号异常或违规  255被系统删除 无法进行任何操作
        ///</summary>
        [ProtoMember(13)]
        public UserStatus Status { get; set; }

        ///<summary>
        /// 通过真实身份验证时间
        ///</summary>
        [ProtoMember(14)]
        public DateTime? AuthedOn { get; set; }

        ///<summary>
        /// 通过qq sina 等注册
        ///</summary>
        [ProtoMember(15)]
        public string CreatedBy { get; set; }

        ///<summary>
        /// 创建时间
        ///</summary>
        [ProtoMember(16)]
        public DateTime CreatedOn { get; set; }

        ///<summary>
        /// 最后一次编辑者
        ///</summary>
        [ProtoMember(17)]
        public string ModifiedBy { get; set; }

        ///<summary>
        /// 最后一次编辑时间
        ///</summary>
        [ProtoMember(18)]
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// Profile
        /// </summary>
        /// <returns></returns>
        [ProtoMember(100)]
        public virtual UserProfile Profile { get; set; }


        #region

        /// <summary>
        /// 性别字典
        /// </summary>
        public static IDictionary<Gender, string> UserGenderDic
        {
            get
            {
                var dic = new Dictionary<Gender, string>();
                dic.Add(Gender.Secrect, "保密");
                dic.Add(Gender.Male, "男");
                dic.Add(Gender.Female, "女");
                return dic;
            }
        }

        /// <summary>
        /// 婚姻状态字典
        /// </summary>
        public static IDictionary<Marital, string> MaritalStatusDic
        {
            get
            {
                var dic = new Dictionary<Marital, string>();
                dic.Add(Marital.Secrect, "保密");
                dic.Add(Marital.Single, "未婚");
                dic.Add(Marital.Married, "已婚");
                return dic;
            }
        }
        #endregion


    }
}