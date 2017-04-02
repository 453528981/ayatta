using Dapper;
using System.Linq;
using Ayatta.Domain;
using System.Collections.Generic;

namespace Ayatta.Storage
{

    public partial class DefaultStorage
    {
        #region 店铺优惠
        /// <summary>
        /// 创建店铺优惠活动
        /// </summary>
        /// <param name="o">店铺优惠活动</param>
        /// <returns></returns>
        public int PromotionNormalCreate(Promotion.Normal o)
        {
            if (o.LimitBy == Promotion.LimitBy.None)
            {
                o.LimitValue = 0;
            }
            return Try(nameof(PromotionNormalCreate), () =>
            {
                var cmd = SqlBuilder.Insert("Normal")
                .Column("Name", o.Name)
                .Column("Title", o.Title)
                .Column("StartedOn", o.StartedOn)
                .Column("StoppedOn", o.StoppedOn)
                .Column("Plateform", o.Plateform)
                .Column("MediaScope", o.MediaScope)
                .Column("Global", o.Global)
                .Column("Discount", o.Discount)
                .Column("LimitBy", o.LimitBy)
                .Column("LimitValue", o.LimitValue)
                .Column("WarmUp", o.WarmUp)
                .Column("Picture", o.Picture)
                .Column("ExternalUrl", o.ExternalUrl)
                .Column("Infinite", o.Infinite)
                .Column("ItemScope", o.ItemScope)
                .Column("FreightFree", o.FreightFree)
                .Column("FreightFreeExclude", o.FreightFreeExclude)
                .Column("SellerId", o.SellerId)
                .Column("Status", o.Status)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand(true);
                return PromotionConn.ExecuteScalar<int>(cmd);
            });
        }

        /// <summary>
        /// 更新店铺优惠活动
        /// </summary>
        /// <param name="o">店铺优惠活动</param>
        /// <returns></returns>
        public bool PromotionNormalUpdate(Promotion.Normal o)
        {
            if (o.LimitBy == Promotion.LimitBy.None)
            {
                o.LimitValue = 0;
            }
            return Try(nameof(PromotionNormalUpdate), () =>
            {
                var cmd = SqlBuilder.Update("Normal")
                    .Column("Name", o.Name)
                    .Column("Title", o.Title)
                    .Column("StartedOn", o.StartedOn)
                    .Column("StoppedOn", o.StoppedOn)
                    .Column("Plateform", o.Plateform)
                    .Column("MediaScope", o.MediaScope)
                    .Column("Global", o.Global)
                    .Column("Discount", o.Discount)
                    .Column("LimitBy", o.LimitBy)
                    .Column("LimitValue", o.LimitValue)
                    .Column("WarmUp", o.WarmUp)
                    .Column("Picture", o.Picture)
                    .Column("ExternalUrl", o.ExternalUrl)
                    .Column("Infinite", o.Infinite)
                    .Column("ItemScope", o.ItemScope)
                    .Column("FreightFree", o.FreightFree)
                    .Column("FreightFreeExclude", o.FreightFreeExclude)
                    .Column("SellerId", o.SellerId)
                    .Column("Status", o.Status)
                    .Column("ModifiedBy", o.ModifiedBy)
                    .Column("ModifiedOn", o.ModifiedOn)
                    .Where("Id=@id", new { o.Id })
                    .ToCommand();
                return PromotionConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// 设置店铺优惠活动为不可用
        /// </summary>
        /// <param name="id">店铺优惠活动Id</param>
        /// <param name="sellerId">卖家Id</param>
        /// <returns></returns>
        public bool PromotionNormalDisable(int id, int? sellerId = null)
        {
            return Try(nameof(PromotionNormalUpdate), () =>
            {
                var cmd = SqlBuilder.Update("Normal")
                     .Column("Status=0")
                    .Where("Id=@Id and Seller=@SellerId", new { id })
                    .Where(sellerId.HasValue && sellerId.Value > 0, "Seller=@SellerId", new { sellerId })
                    .ToCommand();
                return PromotionConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// 获取店铺优惠活动数量
        /// </summary>
        /// <param name="sellerId">卖家Id</param>
        /// <param name="global">是否为全店活动</param>
        /// <param name="status">活动状态</param>
        /// <returns></returns>
        public int PromotionNormalCount(int sellerId, bool? global = null, bool? status = null)
        {
            return Try(nameof(PromotionNormalCount), () =>
            {
                var cmd = SqlBuilder.Select("count(0)")
                        .From("Normal")
                        .Where("SellerId=@sellerId", new { sellerId })
                        .Where(global.HasValue, "Global=@global", new { global })
                        .Where(status.HasValue, "Status=@status", new { status })
                        .ToCommand();
                return PromotionConn.ExecuteScalar<int>(cmd);
            });
        }

        /// <summary>
        /// 获取店铺优惠活动
        /// </summary>
        /// <param name="id">店铺优惠活动Id</param>
        /// <param name="includeRules">是否同时获取店铺优惠规则</param>
        /// <returns></returns>
        public Promotion.Normal PromotionNormalGet(int id, bool includeRules = false)
        {
            return Try(nameof(PromotionNormalGet), () =>
            {
                if (includeRules)
                {
                    var sql = @"select * from Normal where id=@id;select * from NormalRule where ParentId=@id;";
                    var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                    using (var reader = StoreConn.QueryMultiple(cmd))
                    {
                        var o = reader.Read<Promotion.Normal>().FirstOrDefault();
                        if (o != null)
                        {
                            o.Rules = reader.Read<Promotion.Normal.Rule>().ToList();
                        }
                        return o;
                    }
                }
                else
                {
                    var cmd = SqlBuilder.Select("*")
                       .From("Normal")
                       .Where("Id=@id", new { id })
                       .ToCommand();
                    return PromotionConn.QueryFirst<Promotion.Normal>(cmd);
                }
            });
        }


        /// <summary>
        /// 获取店铺优惠活动
        /// </summary>
        /// <param name="index">当前页</param>
        /// <param name="size">分页大小</param>
        /// <param name="sellerId">卖家Id</param>
        /// <param name="status">状态 true为可用正在进行中和即将开始 false为不可用或已结束</param>
        /// <returns></returns>
        public IPagedList<Promotion.Normal> PromotionNormalPagedList(int index, int size, int? sellerId = null, bool? status = null)
        {
            return Try(nameof(PromotionNormalPagedList), () =>
            {
                var cmd = SqlBuilder.Select("*")
                    .From("Normal")
                    .Where(sellerId.HasValue, "SellerId=@sellerId", new { sellerId })
                    .Where(status.HasValue && status.Value, "(Status=1 and StopOn>now())")
                    .Where(status.HasValue && !status.Value, "(Status=0 or StopOn<now())")
                    .ToCommand(index, size);
                return PromotionConn.PagedList<Promotion.Normal>(index, size, cmd);
            });
        }

        /*
        /// <summary>
        /// 创建店铺优惠活动规则
        /// </summary>
        /// <param name="o">店铺优惠活动规则</param>
        /// <returns></returns>
        public bool PromotionDefaultRuleCreate(Promotion.Default.Rule o)
        {
            try
            {
                var cmd = SqlBuilder
                    .Insert("[Default.Rule]")
                    .Column("ParentId", o.ParentId)
                    .Column("Upon", o.Upon)
                    .Column("Value", o.Value)
                    .Column("SendGift", o.SendGift)
                    .Column("GiftJson", o.GiftData)
                    .Column("SendCoupon", o.SendCoupon)
                    .Column("CouponJson", o.CouponData)
                    .Column("SellerId", o.SellerId)
                    .Column("Status", o.Status)
                    .Column("CreatedBy", o.CreatedBy)
                    .Column("CreatedOn", o.CreatedOn)
                    .ToCommand();
                return PromotionConn.Execute(cmd) > 0;
            }
            catch (Exception e)
            {
                OnException(e);
            }
            return false;
        }

        /// <summary>
        /// 更新店铺优惠活动规则
        /// </summary>
        /// <param name="o">店铺优惠活动规则</param>
        /// <returns></returns>
        public bool PromotionDefaultRuleUpdate(Promotion.Default.Rule o)
        {
            try
            {
                var cmd = SqlBuilder
                    .Update("[Default.Rule]")
                    .Column("Upon", o.Upon)
                    .Column("Value", o.Value)
                    .Column("SendGift", o.SendGift)
                    .Column("GiftJson", o.GiftData)
                    .Column("SendCoupon", o.SendCoupon)
                    .Column("CouponJson", o.CouponData)
                    .Column("Status", o.Status)
                    .Where("Id=@Id", new { o.Id })
                    .ToCommand();
                return PromotionConn.Execute(cmd) > 0;
            }
            catch (Exception e)
            {
                OnException(e);
            }
            return false;
        }

        /// <summary>
        /// 获取店铺优惠活动规则
        /// </summary>
        /// <param name="id">店铺优惠活动规则Id</param>
        /// <param name="sellerId">卖家Id</param>
        /// <returns></returns>
        public Promotion.Default.Rule PromotionDefaultRuleGet(int id, int? sellerId = null)
        {
            try
            {
                var cmd = SqlBuilder
                    .Select("*")
                    .From("[Default.Rule]")
                    .Where("Id=@Id", new { id })
                    .Where(sellerId.HasValue, "SellerId=@sellerId", new { sellerId })
                    .ToCommand();
                return PromotionConn.Query<Promotion.Default.Rule>(cmd).FirstOrDefault();
            }
            catch (Exception e)
            {
                OnException(e);
            }
            return null;
        }

        /// <summary>
        /// 设置店铺优惠活动规则为不可用
        /// </summary>
        /// <param name="id">店铺优惠活动规则Id</param>
        /// <param name="sellerId">卖家Id</param>
        /// <returns></returns>
        public bool PromotionDefaultRuleDisable(int id, int? sellerId = null)
        {
            var sql = "update [Default.Rule] set [Status]=0 where Id=@Id";
            if (sellerId.HasValue)
            {
                sql = "update [Default.Rule] set [Status]=0 where Id=@Id and Seller=@SellerId";
            }
            var cmd = SqlBuilder.Raw(sql).Append(sellerId.HasValue, "", new { sellerId }).ToCommand();
            try
            {
                return PromotionConn.Execute(cmd) > 0;
            }
            catch (Exception e)
            {
                OnException(e);
            }
            return false;
        }
        */

        #endregion

        /// <summary>
        /// 获取搭配组合套餐
        /// </summary>
        /// <param name="id">搭配组合套餐Id</param>
        /// <param name="includeItems">是否包含套餐附属商品信息</param>
        /// <returns></returns>
        public Promotion.Package PromotionPackageGet(int id, bool includeItems = false)
        {
            if (includeItems)
            {
                const string sql = @"select * from Package where Id=@id;select * from PackageItem where ParentId=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return
                    StoreConn.Query<Promotion.Package, Promotion.Package.Item, int>(cmd, o => o.Id,
                        o => o.ParentId, (a, b) =>
                        {
                            a.Items = b.ToList();
                        }).FirstOrDefault();
            }
            else
            {
                var cmd = SqlBuilder.Select("*")
                        .From("Package")
                        .Where("Id=@id", new { id })
                        .ToCommand();

                return PromotionConn.Query<Promotion.Package>(cmd).FirstOrDefault();
            }
        }

        #region 获取卖家当前时间可用的促销

        /// <summary>
        /// 获取卖家当前时间可用的限购信息
        /// </summary>
        /// <param name="sellerId">卖家Id</param>
        /// <returns></returns>
        public IList<Promotion.LimitBuy> PromotionLimitBuyList(int sellerId)
        {

            const string sql = @"select * from LimitBuy where Status=1 and StartedOn<now() and StoppedOn>now() and SellerId=@sellerId;";
            var cmd = SqlBuilder.Raw(sql, new { sellerId }).ToCommand();
            return PromotionConn.Query<Promotion.LimitBuy>(cmd).ToList();

        }

        /// <summary>
        /// 获取卖家当前时间可用的特价
        /// </summary>
        /// <param name="sellerId">卖家Id</param>
        /// <returns></returns>
        public IList<Promotion.SpecialPrice> PromotionSpecialPriceList(int sellerId)
        {
            const string sql = @"select * from SpecialPrice where Status=1 and StartedOn<now() and StoppedOn>now() and SellerId=@sellerId;
            select a.* from SpecialPriceItem a inner join SpecialPrice b on a.ParentId=b.Id where a.Status=1 and b.Status=1 and b.StartedOn<now() and b.StoppedOn>now() and a.SellerId=@sellerId";
            var cmd = SqlBuilder.Raw(sql, new { sellerId }).ToCommand();
            return PromotionConn.Query<Promotion.SpecialPrice, Promotion.SpecialPrice.Item, int>(cmd, o => o.Id, o => o.ParentId, (a, b) => { a.Items = b.Where(x => x.ParentId == a.Id).ToList(); }).ToList();

        }

        /// <summary>
        /// 获取卖家当前时间可用的搭配组合套餐
        /// </summary>
        /// <param name="sellerId">卖家Id</param>
        /// <returns></returns>
        public IList<Promotion.Package> PromotionPackageList(int sellerId)
        {
            const string sql = @"select * from Package where Status=1 and StartedOn<now() and StoppedOn>now() and SellerId=@sellerId;
            select a.* from PackageItem a inner join Package b on a.ParentId=b.Id where a.Status=1 and b.Status=1 and b.StartedOn<now() and b.StoppedOn>now() and a.SellerId=@sellerId";
            var cmd = SqlBuilder.Raw(sql, new { sellerId }).ToCommand();
            return PromotionConn.Query<Promotion.Package, Promotion.Package.Item, int>(cmd, o => o.Id, o => o.ParentId, (a, b) => { a.Items = b.Where(x => x.ParentId == a.Id).ToList(); }).ToList();
        }

        /// <summary>
        /// 获取卖家当前时间可用的店铺优惠活动
        /// </summary>
        /// <param name="sellerId">卖家Id</param>
        /// <returns></returns>
        public IList<Promotion.Normal> PromotionNormalList(int sellerId)
        {
            const string sql = @"select * from Normal where Status=1 and StartedOn<now() and StoppedOn>now() and SellerId=@sellerId;
            select a.* from NormalRule a inner join Normal b on a.ParentId=b.Id where a.Status=1 and b.Status=1 and b.StartedOn<now() and b.StoppedOn>now() and a.SellerId=@sellerId";
            var cmd = SqlBuilder.Raw(sql, new { sellerId }).ToCommand();
            return PromotionConn.Query<Promotion.Normal, Promotion.Normal.Rule, int>(cmd, o => o.Id, o => o.ParentId, (a, b) => { a.Rules = b.Where(x => x.ParentId == a.Id).OrderBy(x => x.Upon).ToList(); }).ToList();
        }

        /// <summary>
        /// 获取卖家当前时间可用的购物车促销
        /// </summary>
        /// <param name="sellerId">卖家Id</param>
        /// <returns></returns>
        public IList<Promotion.Cart> PromotionCartList(int sellerId)
        {
            const string sql = @"select * from Cart where Status=1 and StartedOn<now() and StoppedOn>now() and SellerId=@sellerId;
            select a.* from CartRule a inner join Cart b on a.ParentId=b.Id where a.Status=1 and b.Status=1 and b.StartedOn<now() and b.StoppedOn>now() and a.SellerId=@sellerId";
            var cmd = SqlBuilder.Raw(sql, new { sellerId }).ToCommand();
            return PromotionConn.Query<Promotion.Cart, Promotion.Cart.Rule, int>(cmd, o => o.Id, o => o.ParentId, (a, b) => { a.Rules = b.Where(x => x.ParentId == a.Id).ToList(); }).ToList();
        }
        #endregion


    }
}