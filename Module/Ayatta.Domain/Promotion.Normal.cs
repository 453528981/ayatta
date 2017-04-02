using System;
using ProtoBuf;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ayatta.Domain
{
    /// <summary>
    /// 促销 http://bbs.taobao.com/catalog/thread/16543510-264265243.htm
    /// </summary>
    public partial class Promotion
    {

        ///<summary>
        /// 店铺优惠 http://bbs.taobao.com/catalog/thread/16543510-264269834.htm
        /// created on 2016-05-22 00:50:36
        ///</summary>
        [ProtoContract]
        public class Normal : IEntity<int>
        {
            #region
            ///<summary>
            /// Id
            ///</summary>
            [ProtoMember(1)]
            public int Id { get; set; }

            ///<summary>
            /// 活动名称
            ///</summary>
            [ProtoMember(2)]
            public string Name { get; set; }

            ///<summary>
            /// 活动标题
            ///</summary>
            [ProtoMember(3)]
            public string Title { get; set; }

            ///<summary>
            /// 开始时间
            ///</summary>
            [ProtoMember(4)]
            public DateTime StartedOn { get; set; }

            ///<summary>
            /// 结束时间
            ///</summary>
            [ProtoMember(5)]
            public DateTime StoppedOn { get; set; }

            ///<summary>
            /// 活动适用平台 0为None 1为适用平于pc 2为适用平于wap 4为适用平于app
            ///</summary>
            [ProtoMember(6)]
            public Plateform Plateform { get; set; }

            ///<summary>
            /// 限定媒体Id 空为无限定 如需限定部分媒体 使用","分隔
            ///</summary>
            [ProtoMember(7)]
            public string MediaScope { get; set; }

            ///<summary>
            /// 适用于(全场店铺)所有商品
            ///</summary>
            [ProtoMember(8)]
            public bool Global { get; set; }

            ///<summary>
            /// true为满件折 false为满元减
            ///</summary>
            [ProtoMember(9)]
            public bool Discount { get; set; }

            ///<summary>
            /// 用户参与活动限制
            ///</summary>
            [ProtoMember(10)]
            public LimitBy LimitBy { get; set; }

            ///<summary>
            /// 用户参与活动限制值 Limit为true时有效
            ///</summary>
            [ProtoMember(11)]
            public int LimitValue { get; set; }

            ///<summary>
            /// 提前预热天数
            ///</summary>
            [ProtoMember(12)]
            public int WarmUp { get; set; }

            ///<summary>
            /// 标准版 活动图片
            ///</summary>
            [ProtoMember(13)]
            public string Picture { get; set; }

            ///<summary>
            /// 豪华版 专辑地址
            ///</summary>
            [ProtoMember(14)]
            public string ExternalUrl { get; set; }

            ///<summary>
            /// 上不封顶(当规则为满元减且只有一级时 该值可为true)
            ///</summary>
            [ProtoMember(15)]
            public bool Infinite { get; set; }

            ///<summary>
            /// Global==false时为包含的商品多个以,分隔 Global==true时为排除的商品多个以,分隔
            ///</summary>
            [ProtoMember(16)]
            public string ItemScope { get; set; }

            ///<summary>
            /// 是否免运费
            ///</summary>
            [ProtoMember(17)]
            public bool FreightFree { get; set; }

            ///<summary>
            /// 免运费排除在外的地区多个以,分隔
            ///</summary>
            [ProtoMember(18)]
            public string FreightFreeExclude { get; set; } = "";

            ///<summary>
            /// 卖家Id
            ///</summary>
            [ProtoMember(19)]
            public int SellerId { get; set; }

            ///<summary>
            /// 状态 1为可用 0为不可用
            ///</summary>
            [ProtoMember(20)]
            public bool Status { get; set; }

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

            /// <summary>
            /// 规则
            /// </summary>
            [ProtoMember(24)]
            public IList<Rule> Rules { get; set; }

            #endregion

            public Normal()
            {
                Rules = new List<Rule>();
            }

            #region Readonly

            /// <summary>
            /// 活动限定媒体
            /// </summary>
            public virtual IList<int> Medias
            {
                get
                {
                    if (!string.IsNullOrEmpty(MediaScope))
                    {
                        return MediaScope.Split(',').Select(x => int.Parse(x)).ToArray();                       
                    }
                    return new List<int>(0);
                }
            }

            /// <summary>
            /// 活动限定商品
            /// </summary>
            public virtual IList<int> Items
            {
                get
                {
                    if (!string.IsNullOrEmpty(ItemScope))
                    {
                        return ItemScope.Split(',').Select(x => int.Parse(x)).ToArray();
                    }
                    return new List<int>(0);
                }
            }

            public virtual string TimeText
            {
                get
                {
                    var span = StoppedOn.Subtract(StartedOn);
                    return $"活动持续：{span.Days}天 {span.Hours}小时 {span.Minutes}分 {span.Seconds}秒";
                }
            }

            public virtual string StatusText
            {
                get
                {
                    if (!Status) return "已暂停";
                    if (DateTime.Now >= StartedOn && DateTime.Now < StoppedOn)
                    {
                        return "进行中";
                    }
                    return DateTime.Now > StoppedOn ? "已结束" : "未开始";
                }
            }

            public static IDictionary<int, string> WarmUpDic
            {
                get
                {
                    var dic = new Dictionary<int, string>();
                    dic.Add(0, "无");
                    dic.Add(1, "提前1天");
                    dic.Add(2, "提前2天");
                    dic.Add(3, "提前3天");
                    dic.Add(4, "提前4天");
                    dic.Add(5, "提前5天");
                    return dic;
                }
            }

            #endregion

            #region Method
            /// <summary>
            /// 匹配规则
            /// </summary>
            /// <param name="amount">订单商品总金额小计</param>
            /// <param name="quantity">详单商品总数</param>
            /// <returns></returns>
            public Rule MatchRule(decimal amount, int quantity)
            {
                var list = Rules.OrderByDescending(x => x.Upon);
                return Discount ? list.FirstOrDefault(o => quantity >= o.Value) : list.FirstOrDefault(o => amount >= o.Value);
            }

            /// <summary>
            /// 判断活动在指定平台是否有效            
            /// </summary>
            /// <param name="plateform">适用平台</param>
            /// <returns></returns>
            public bool IsValid(Plateform plateform)
            {
                var now = DateTime.Now;
                var available = ((Plateform & plateform) == plateform);//检查当前促销是否适用于给定平台
                return Status && StartedOn < now && now < StoppedOn && available && Rules.Any(x => x.Status);
            }

            #endregion

            ///<summary>
            /// NormalRule
            /// created on 2016-05-22 00:56:10
            ///</summary>
            [ProtoContract]
            public class Rule : IEntity<int>
            {
                ///<summary>
                /// Id
                ///</summary>
                [ProtoMember(1)]
                public int Id { get; set; }

                ///<summary>
                /// 店铺活动Id
                ///</summary>
                [ProtoMember(2)]
                public int ParentId { get; set; }

                ///<summary>
                /// 临界值
                ///</summary>
                [ProtoMember(3)]
                public decimal Upon { get; set; }

                ///<summary>
                /// 满减/满折值
                ///</summary>
                [ProtoMember(4)]
                public decimal Value { get; set; }

                ///<summary>
                /// 送赠品
                ///</summary>
                [ProtoMember(5)]
                public bool SendGift { get; set; }

                ///<summary>
                /// 赠品信息 Json格式
                ///</summary>
                [JsonIgnore]
                [ProtoMember(6)]
                public string GiftJson { get; set; }

                ///<summary>
                /// 送店铺优惠券
                ///</summary>
                [ProtoMember(7)]
                public bool SendCoupon { get; set; }

                ///<summary>
                /// 优惠券信息 Json格式
                ///</summary>
                [JsonIgnore]
                [ProtoMember(8)]
                public string CouponJson { get; set; }

                ///<summary>
                /// 卖家Id
                ///</summary>
                [ProtoMember(9)]
                public int SellerId { get; set; }

                ///<summary>
                /// 状态 1为可用 0为不可用
                ///</summary>
                [ProtoMember(10)]
                public bool Status { get; set; }

                ///<summary>
                /// 创建时间
                ///</summary>
                [ProtoMember(11)]
                public DateTime CreatedOn { get; set; }

                ///<summary>
                /// 最后一次编辑者
                ///</summary>
                [ProtoMember(12)]
                public string ModifiedBy { get; set; }

                ///<summary>
                /// 最后一次编辑时间
                ///</summary>
                [ProtoMember(13)]
                public DateTime ModifiedOn { get; set; }


                /// <summary>
                /// 优惠券
                /// </summary>
                public virtual IList<Coupon> Coupons
                {
                    get
                    {
                        if (!SendCoupon || string.IsNullOrEmpty(CouponJson)) return null;

                        return JsonConvert.DeserializeObject<List<Coupon>>(CouponJson);

                    }
                }

                /// <summary>
                /// 赠品
                /// </summary>
                public virtual IList<Gift> Gifts
                {
                    get
                    {
                        if (!SendGift || string.IsNullOrEmpty(GiftJson)) return null;

                        return JsonConvert.DeserializeObject<List<Gift>>(GiftJson);
                    }
                }
            }

            #region 赠品
            /// <summary>
            /// 赠品
            /// </summary>
            public class Gift
            {
                /// <summary>
                /// SkuId
                /// </summary>
                public int SkuId { get; set; }

                /// <summary>
                /// 商品Id
                /// </summary>
                public int ItemId { get; set; }

                /// <summary>
                /// 商品名称
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 商品数量
                /// </summary>
                public int Quantity { get; set; }

                /// <summary>
                /// 销售属性
                /// </summary>
                public string PropText { get; set; }

            }
            #endregion

            #region 店铺优惠券
            /// <summary>
            /// 店铺优惠券 http://bbs.taobao.com/catalog/thread/16543510-264269834.htm
            /// </summary>
            public class Coupon
            {
                /// <summary>
                /// 面额 3 5 10 20 50 100 200
                /// </summary>
                public int Value { get; set; }

                /// <summary>
                /// 数量
                /// </summary>
                public int Count { get; set; }

                /// <summary>
                /// 适用范围 pc wap app 通用
                /// </summary>
                public Plateform Plateform { get; set; }

                /// <summary>
                /// 生效时间
                /// </summary>
                public DateTime StartedOn { get; set; }

                /// <summary>
                /// 失效时间 失效时间不应早于生效时间及活动结束时间
                /// </summary>
                public DateTime StoppedOn { get; set; }

                /// <summary>
                /// 使用条件 不限/订单满x元
                /// </summary>
                public bool Limit { get; set; }

                /// <summary>
                /// 使用条件值 订单满x元
                /// </summary>
                public decimal LimitVal { get; set; }

            }
            #endregion
        }
    }
}

