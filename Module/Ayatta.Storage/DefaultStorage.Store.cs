using Dapper;
using System;
using System.Text;
using System.Linq;
//using Ayatta.Param;
using Ayatta.Domain;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ayatta.Storage
{
    public partial class DefaultStorage
    {
        #region 商品

        /// <summary>
        /// 商品创建
        /// </summary>
        /// <param name="o">商品</param>
        /// <returns></returns>
        public int ItemCreate(Item o)
        {
            return Try(nameof(ItemCreate), () =>
            {
                using (var conn = StoreConn)
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            var cmd = SqlBuilder.Insert("Item")
                            .Column("SpuId", o.SpuId)
                            .Column("CatgRId", o.CatgRId)
                            .Column("CatgId", o.CatgId)
                            .Column("BrandId", o.BrandId)
                            .Column("BrandName", o.BrandName)
                            .Column("Name", o.Name)
                            .Column("Title", o.Title)
                            .Column("Stock", o.Stock)
                            .Column("Price", o.Price)
                            .Column("AppPrice", o.AppPrice)
                            .Column("RetailPrice", o.RetailPrice)
                            .Column("Code", o.Code)
                            .Column("Barcode", o.Barcode)
                            .Column("Keyword", o.Keyword)
                            .Column("Summary", o.Summary)
                            .Column("Picture", o.Picture)
                            .Column("ItemImgStr", o.ItemImgStr)
                            .Column("PropImgStr", o.PropImgStr)
                            .Column("Width", o.Width)
                            .Column("Depth", o.Depth)
                            .Column("Height", o.Height)
                            .Column("Weight", o.Weight)
                            .Column("Country", o.Country)
                            .Column("Location", o.Location)
                            .Column("IsBonded", o.IsBonded)
                            .Column("IsOversea", o.IsOversea)
                            .Column("IsTiming", o.IsTiming)
                            .Column("IsVirtual", o.IsVirtual)
                            .Column("IsAutoFill", o.IsAutoFill)
                            .Column("SupportCod", o.SupportCod)
                            .Column("FreightFree", o.FreightFree)
                            .Column("FreightTplId", o.FreightTplId)
                            .Column("SubStock", o.SubStock)
                            .Column("Showcase", o.Showcase)
                            .Column("OnlineOn", o.OnlineOn)
                            .Column("OfflineOn", o.OfflineOn)
                            .Column("RewardRate", o.RewardRate)
                            .Column("HasInvoice", o.HasInvoice)
                            .Column("HasWarranty", o.HasWarranty)
                            .Column("HasGuarantee", o.HasGuarantee)
                            .Column("SaleCount", o.SaleCount)
                            .Column("CollectCount", o.CollectCount)
                            .Column("ConsultCount", o.ConsultCount)
                            .Column("CommentCount", o.CommentCount)
                            .Column("PropId", o.PropId)
                            .Column("PropStr", o.PropStr)
                            .Column("PropAlias", o.PropAlias)
                            .Column("InputId", o.InputId)
                            .Column("InputStr", o.InputStr)
                            .Column("CatgStr", o.CatgStr)
                            .Column("BrandStr", o.BrandStr)
                            .Column("SearchStr", o.SearchStr)
                            .Column("Meta", o.Meta)
                            .Column("Label", o.Label)
                            .Column("Related", o.Related)
                            .Column("Prority", o.Prority)
                            .Column("SellerId", o.SellerId)
                            .Column("Status", o.Status)
                            .Column("CreatedOn", o.CreatedOn)
                            .Column("ModifiedBy", o.ModifiedBy)
                            .Column("ModifiedOn", o.ModifiedOn)

                            .ToCommand(true, tran);

                            var status = true;
                            var id = conn.ExecuteScalar<int>(cmd);

                            if (id > 0)
                            {

                                cmd = SqlBuilder.Insert("ItemDesc")
                                .Column("Id", id)
                                .Column("Detail", o.Desc.Detail)
                                .Column("Manual", o.Desc.Manual)
                                .Column("Photo", o.Desc.Photo)
                                .Column("Story", o.Desc.Story)
                                .Column("Notice", o.Desc.Notice)
                                .Column("CreatedOn", o.Desc.CreatedOn)
                                .Column("ModifiedBy", o.Desc.ModifiedBy)
                                .Column("ModifiedOn", o.Desc.ModifiedOn)
                                .ToCommand(false, tran);
                                status = conn.Execute(cmd) > 0;
                                if (status)
                                {
                                    foreach (var sku in o.Skus)
                                    {
                                        cmd = SqlBuilder.Insert("Sku")
                                        .Column("SpuId", sku.SpuId)
                                        .Column("ItemId", id)
                                        .Column("CatgRId", sku.CatgRId)
                                        .Column("CatgId", sku.CatgId)
                                        .Column("BrandId", sku.BrandId)
                                        .Column("Stock", sku.Stock)
                                        .Column("Price", sku.Price)
                                        .Column("AppPrice", sku.AppPrice)
                                        .Column("RetailPrice", sku.RetailPrice)
                                        .Column("Code", sku.Code)
                                        .Column("Barcode", sku.Barcode)
                                        .Column("PropId", sku.PropId)
                                        .Column("PropStr", sku.PropStr)
                                        .Column("SaleCount", sku.SaleCount)
                                        .Column("SellerId", sku.SellerId)
                                        .Column("Status", sku.Status)
                                        .Column("CreatedOn", sku.CreatedOn)
                                        .Column("ModifiedBy", sku.ModifiedBy)
                                        .Column("ModifiedOn", sku.ModifiedOn)
                                        .ToCommand(false, tran);

                                        status = conn.Execute(cmd) > 0;
                                        if (!status)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            if (id > 0 && status)
                            {
                                tran.Commit();
                                return id;
                            }
                            tran.Rollback();
                            return 0;
                        }
                        catch (Exception e)
                        {
                            tran.Rollback();
                            throw e;
                        }
                    }
                }
            });
        }

        /// <summary>
        /// 商品更新
        /// </summary>
        /// <param name="o">商品</param>
        /// <param name="deletedIds">待删除的Sku Id集合</param>
        /// <returns></returns>
        public bool ItemUpdate(Item o, int[] deletedIds)
        {
            return Try(nameof(ItemUpdate), () =>
            {
                using (var conn = StoreConn)
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            var cmd = SqlBuilder.Update("Item")
                            .Column("SpuId", o.SpuId)
                            .Column("CatgRId", o.CatgRId)
                            .Column("CatgId", o.CatgId)
                            .Column("BrandId", o.BrandId)
                            .Column("BrandName", o.BrandName)
                            .Column("Name", o.Name)
                            .Column("Title", o.Title)
                            .Column("Stock", o.Stock)
                            .Column("Price", o.Price)
                            .Column("AppPrice", o.AppPrice)
                            .Column("RetailPrice", o.RetailPrice)
                            .Column("Code", o.Code)
                            .Column("Barcode", o.Barcode)
                            .Column("Keyword", o.Keyword)
                            .Column("Summary", o.Summary)
                            .Column("Picture", o.Picture)
                            .Column("ItemImgStr", o.ItemImgStr)
                            .Column("PropImgStr", o.PropImgStr)
                            .Column("Width", o.Width)
                            .Column("Depth", o.Depth)
                            .Column("Height", o.Height)
                            .Column("Weight", o.Weight)
                            .Column("Country", o.Country)
                            .Column("Location", o.Location)
                            .Column("IsBonded", o.IsBonded)
                            .Column("IsOversea", o.IsOversea)
                            .Column("IsTiming", o.IsTiming)
                            .Column("IsVirtual", o.IsVirtual)
                            .Column("IsAutoFill", o.IsAutoFill)
                            .Column("SupportCod", o.SupportCod)
                            .Column("FreightFree", o.FreightFree)
                            .Column("FreightTplId", o.FreightTplId)
                            .Column("SubStock", o.SubStock)
                            .Column("Showcase", o.Showcase)
                            .Column("OnlineOn", o.OnlineOn)
                            .Column("OfflineOn", o.OfflineOn)
                            .Column("RewardRate", o.RewardRate)
                            .Column("HasInvoice", o.HasInvoice)
                            .Column("HasWarranty", o.HasWarranty)
                            .Column("HasGuarantee", o.HasGuarantee)
                            //.Column("SaleCount",o.SaleCount)
                            //.Column("CollectCount",o.CollectCount)
                            //.Column("ConsultCount",o.ConsultCount)
                            //.Column("CommentCount",o.CommentCount)
                            .Column("PropId", o.PropId)
                            .Column("PropStr", o.PropStr)
                            .Column("PropAlias", o.PropAlias)
                            .Column("InputId", o.InputId)
                            .Column("InputStr", o.InputStr)
                            .Column("CatgStr", o.CatgStr)
                            .Column("BrandStr", o.BrandStr)
                            .Column("SearchStr", o.SearchStr)
                            .Column("Meta", o.Meta)
                            .Column("Label", o.Label)
                            .Column("Related", o.Related)
                            .Column("Prority", o.Prority)
                            .Column("SellerId", o.SellerId)
                            .Column("Status", o.Status)
                            .Column("ModifiedBy", o.ModifiedBy)
                            .Column("ModifiedOn", o.ModifiedOn)
                            .Where("Id=@id", new { o.Id })

                            .ToCommand(tran);

                            var status = conn.Execute(cmd) > 0;

                            if (status)
                            {
                                cmd = SqlBuilder.Update("ItemDesc")
                                .Column("Detail", o.Desc.Detail)
                                .Column("Manual", o.Desc.Manual)
                                .Column("Photo", o.Desc.Photo)
                                .Column("Story", o.Desc.Story)
                                .Column("Notice", o.Desc.Notice)
                                .Column("ModifiedBy", o.Desc.ModifiedBy)
                                .Column("ModifiedOn", o.Desc.ModifiedOn)
                                .Where("Id=@id", new { o.Id })
                                .ToCommand(tran);
                                status = conn.Execute(cmd) > 0;

                                if (status && o.Skus.Any())
                                {
                                    foreach (var sku in o.Skus.Where(x => x.Id == 0))
                                    {
                                        cmd = SqlBuilder.Insert("Sku")

                                        .Column("SpuId", sku.SpuId)
                                        .Column("ItemId", o.Id)
                                        .Column("CatgRId", sku.CatgRId)
                                        .Column("CatgId", sku.CatgId)
                                        .Column("BrandId", sku.BrandId)
                                        .Column("Stock", sku.Stock)
                                        .Column("Price", sku.Price)
                                        .Column("AppPrice", sku.AppPrice)
                                        .Column("RetailPrice", sku.RetailPrice)
                                        .Column("Code", sku.Code)
                                        .Column("Barcode", sku.Barcode)
                                        .Column("PropId", sku.PropId)
                                        .Column("PropStr", sku.PropStr)
                                        .Column("SaleCount", sku.SaleCount)
                                        .Column("Status", sku.Status)
                                        .Column("SellerId", o.SellerId)
                                        .Column("CreatedOn", sku.CreatedOn)
                                        .Column("ModifiedBy", sku.ModifiedBy)
                                        .Column("ModifiedOn", sku.ModifiedOn)
                                        .ToCommand(false, tran);

                                        status = conn.Execute(cmd) > 0;

                                        if (!status)
                                        {
                                            break;
                                        }
                                    }
                                    foreach (var sku in o.Skus.Where(x => x.Id > 0))
                                    {
                                        cmd = SqlBuilder.Update("Sku")
                                        .Column("SpuId", sku.SpuId)
                                        .Column("CatgRId", sku.CatgRId)
                                        .Column("CatgId", sku.CatgId)
                                        .Column("BrandId", sku.BrandId)
                                        .Column("Stock", sku.Stock)
                                        .Column("Price", sku.Price)
                                        .Column("AppPrice", sku.AppPrice)
                                        .Column("RetailPrice", sku.RetailPrice)
                                        .Column("Code", sku.Code)
                                        .Column("Barcode", sku.Barcode)
                                        .Column("PropId", sku.PropId)
                                        .Column("PropStr", sku.PropStr)
                                        .Column("Status", sku.Status)
                                        .Column("ModifiedBy", sku.ModifiedBy)
                                        .Column("ModifiedOn", sku.ModifiedOn)
                                        .Where("Id=@Id", new { sku.Id })
                                        .ToCommand(tran);

                                        conn.Execute(cmd);
                                    }
                                }
                                var sql = "update Sku set stock=0,status=2 where id in @deletedIds";
                                cmd = SqlBuilder.Raw(sql, new { deletedIds }).ToCommand(tran);
                                conn.Execute(cmd);
                            }
                            if (status)
                            {
                                tran.Commit();
                                return true;
                            }
                            tran.Rollback();
                            return false;
                        }
                        catch (Exception)
                        {
                            tran.Rollback();
                            throw;
                        }
                    }
                }
            });
        }

        /// <summary>
        /// 商品获取
        /// </summary>
        /// <param name="id">商品id</param>
        /// <param name="includeSkus">是否包含Sku</param>
        /// <returns></returns>
        public Item ItemGet(int id, bool includeSkus = false)
        {
            return Try(nameof(ItemGet), () =>
            {
                if (includeSkus)
                {
                    var sql = @"select * from item where id=@id;select * from sku where itemid=@id;";
                    var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                    using (var reader = StoreConn.QueryMultiple(cmd))
                    {
                        var o = reader.Read<Item>().FirstOrDefault();
                        if (o != null)
                        {
                            o.Skus = reader.Read<Sku>().ToList();
                        }
                        return o;
                    }
                }
                else
                {
                    var cmd = SqlBuilder
                    .Select("*").From("item")
                    .Where("id=@id", new { id })
                    .ToCommand();
                    return StoreConn.QueryFirstOrDefault<Item>(cmd);
                }
            });
        }

        public IPagedList<Item.Tiny> ItemTinyList(int page = 1, int size = 20, string keyword = null, int? catgId = null, int? brandId = null, int? sellerId = null, Prod.Status[] status = null, int[] include = null, int[] exclude = null)
        {
            if (size < 0)
            {
                size = 20;
            }
            if (size > 200)
            {
                size = 200;
            }
            var sb = SqlBuilder
            .Select("id,catgid,brandid,brandname,code,name,stock,price,retailprice,barcode,keyword,summary,picture")
            .From("item");

            if (!string.IsNullOrEmpty(keyword))
            {
                if (Regex.IsMatch(keyword, "^\\d+$"))
                {
                    sb.Where("Id=@id", new { id = keyword });
                }
                else
                {
                    sb.Where("Name LIKE @Name", new { name = keyword + "%" });
                }
            }
            if (catgId.HasValue && catgId.Value > 0)
            {
                sb.Where("CatgId=@CatgId", new { catgId });
            }
            if (brandId.HasValue && brandId.Value > 0)
            {
                sb.Where("BrandId=@brandId", new { brandId });
            }
            if (sellerId.HasValue)
            {
                sb.Where("SellerId=@sellerId", new { sellerId });
            }
            if (status != null)
            {
                if (status.Length == 1 && status[0] > 0)
                {
                    sb.Where("Status=@status", new { id = include[0] });
                }
                if (status.Length > 1)
                {
                    var str = string.Join(",", status);
                    sb.Where("Status in (" + str + ")");
                }
            }
            if (include != null)
            {
                if (include.Length == 1 && include[0] > 0)
                {
                    sb.Where("Id=@id", new { id = include[0] });
                }
                if (include.Length > 1)
                {
                    var str = string.Join(",", include);
                    sb.Where("Id in (" + str + ")");
                }
            }
            if (exclude != null)
            {
                if (exclude.Length == 1 && exclude[0] > 0)
                {
                    sb.Where("Id<>@id", new { id = exclude[0] });
                }
                if (exclude.Length > 1)
                {
                    var str = string.Join(",", include);
                    sb.Where("Id not in (" + str + ")");
                }
            }

            //sb.OrderBy("Id DESC");

            var cmd = sb.ToCommand(page, size);

            return StoreConn.PagedList<Item.Tiny>(page, size, cmd);
        }

        /*
        public IPagedList<Item> ItemSearch(ProdItemSearchParam param)
        {
            var sb = SqlBuilder
            .Select("*")
            .From("item");
            if (param.UserId.HasValue)
            {
                sb.Where("SellerId=@userid", new { param.UserId });
            }
            if (param.Id > 0)
            {
                sb.Where("Id=@id", new { param.Id });
            }
            if (!string.IsNullOrEmpty(param.Name))
            {
                sb.Where("Name LIKE @Name", new { name = param.Name + "%" });
            }
            if (!string.IsNullOrEmpty(param.Code))
            {
                sb.Where("Code=@code", new { param.Code });
            }
            if (param.CRId > 0)
            {
                sb.Where("CatgRId=@CatgRId", new { CatgRId = param.CRId });
            }
            if (param.PriceFrom > 0)
            {
                sb.Where("Price>=@PriceFrom", new { param.PriceFrom });
            }
            if (param.PriceEnd > 0)
            {
                sb.Where("Price<=@PriceEnd", new { param.PriceEnd });
            }
            if (param.SaleFrom > 0)
            {
                sb.Where("SaleCount>=@SaleFrom", new { param.SaleFrom });
            }
            if (param.SaleEnd > 0)
            {
                sb.Where("SaleCount>=@SaleEnd", new { param.SaleEnd });
            }
            if (param.OrderBy != null && param.OrderBy.Count > 0)
            {
                var tmp = new StringBuilder();
                foreach (var o in param.OrderBy)
                {
                    tmp.AppendFormat("[{0}] {1},", o.Key, o.Value ? "ASC" : "DESC");
                }
                sb.OrderBy(sb.ToString().TrimEnd(','));
            }
            else
            {
                sb.OrderBy("Id DESC");
            }
            var cmd = sb.ToCommand(param.PageIndex, param.PageSize);

            return StoreConn.PagedList<Item>(param.PageIndex, param.PageSize, cmd);

        }
        */
        #endregion

        #region 商品Sku
        /// <summary>
        /// 商品Sku获取
        /// </summary>
        /// <param name="id">商品Sku id</param>
        /// <returns></returns>
        public Sku SkuGet(int id)
        {
            return Try(nameof(SkuGet), () =>
            {
                var cmd = SqlBuilder.Select("*")
                .From("sku")
                .Where("id=@id", new { id })
                .ToCommand();
                return StoreConn.QueryFirstOrDefault<Sku>(cmd);
            });
        }

        /// <summary>
        /// 商品Sku获取
        /// </summary>
        /// <param name="itemId">商品id</param>
        /// <returns></returns>
        public IList<Sku> SkuList(int itemId)
        {
            return Try(nameof(SkuList), () =>
            {
                var cmd = SqlBuilder
                .Select("*").From("sku")
                .Where("ItemId=@ItemId", new { itemId })
                .ToCommand();
                return StoreConn.Query<Sku>(cmd).ToList();
            });
        }

        #endregion

        #region 商品描述
        /// <summary>
        ///商品描述获取
        /// </summary>
        /// <param name="id">商品Id</param>
        /// <returns></returns>
        public ItemDesc ItemDescGet(int id)
        {
            return Try(nameof(ItemDescGet), () =>
            {
                var cmd = SqlBuilder.Select("*")
                .From("itemdesc")
                .Where("id=@id", new { id })
                .ToCommand();
                return StoreConn.QueryFirstOrDefault<ItemDesc>(cmd);
            });
        }
        #endregion

        #region 商品评价

        ///<summary>
        /// 商品评价创建
        ///</summary>
        ///<param name="o">商品评价</param>
        ///<returns></returns>
        public bool ItemCommentCreate(ItemComment o)
        {
            return Try(nameof(ItemCommentCreate), () =>
            {
                var cmd = SqlBuilder.Insert("ItemComment")
                .Column("Id", o.Id)
                .Column("SkuId", o.SkuId)
                .Column("AvgScore", o.AvgScore)
                .Column("ImgCount", o.ImgCount)
                .Column("SumCount", o.SumCount)
                .Column("CountA", o.CountA)
                .Column("CountB", o.CountB)
                .Column("CountC", o.CountC)
                .Column("CountD", o.CountD)
                .Column("CountE", o.CountE)
                .Column("TagData", o.TagData)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand();

                return StoreConn.Execute(cmd) > 0;
            });
        }

        #endregion

        #region 商品咨询
        ///<summary>
        /// 商品咨询创建
        ///</summary>
        ///<param name="o">商品咨询</param>
        ///<returns></returns>
        public int ConsultationCreate(Consultation o)
        {
            return Try(nameof(ConsultationCreate), () =>
            {
                var cmd = SqlBuilder.Insert("Consultation")
                .Column("SkuId", o.SkuId)
                .Column("ItemId", o.ItemId)
                .Column("GroupId", o.GroupId)
                .Column("Question", o.Question)
                .Column("UserId", o.UserId)
                .Column("UserNickname", o.UserNickname)
                .Column("Reply", o.Reply)
                .Column("Replier", o.Replier)
                .Column("ReplierId", o.ReplierId)
                .Column("RepliedOn", o.RepliedOn)
                .Column("SellerId", o.SellerId)
                .Column("Status", o.Status)
                .Column("CreatedBy", o.CreatedBy)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand(true);
                return StoreConn.ExecuteScalar<int>(cmd);
            });
        }

        /// <summary>
        /// 商品咨询回复
        /// </summary>
        /// <param name="id">商品咨询id</param>
        /// <param name="reply">回复内容</param>
        /// <param name="replier">回复者</param>
        /// <param name="replierId">回复者id</param>
        /// <returns></returns>
        public bool ConsultationReply(int id, string reply, string replier, int replierId)
        {
            return Try(nameof(ConsultationReply), () =>
            {
                var cmd = SqlBuilder.Update("Consultation")
                .Column("Reply", reply)
                .Column("Replier", replier)
                .Column("ReplierId", replierId)
                .Column("RepliedOn", DateTime.Now)
                .Column("Status", 0)
               .Where("Id=@id", new { id })
               .ToCommand();
                return StoreConn.Execute(cmd) > 0;
            });
        }

        #endregion

        #region ItemMini
        public ItemMini ItemMiniGet(int id)
        {
            return Try(nameof(ItemMiniGet), () =>
            {
                var sql = @"select Id,SpuId,CatgRId,CatgId,BrandId,BrandName,Name,Title,Stock,Price,AppPrice,RetailPrice,Code,Barcode,Keyword,Summary,Picture,ItemImgStr,PropImgStr,Status,PropId,PropStr,InputId,InputStr,CatgStr,SellerId from item where id=@id;
                select Id,Stock,Price,AppPrice,RetailPrice,Code,Barcode,PropId,PropStr,Status from sku where itemid=@id;";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                using (var reader = StoreConn.QueryMultiple(cmd))
                {
                    var o = reader.Read<ItemMini>().FirstOrDefault();
                    if (o != null)
                    {
                        o.Skus = reader.Read<ItemMini.Sku>().ToList();
                    }
                    return o;
                }
            });
        }

        public bool ItemMiniUpdate(ItemMini o)
        {
            return Try(nameof(ItemMiniUpdate), () =>
            {
                using (var conn = StoreConn)
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            var cmd = SqlBuilder
                            .Update("Item")
                            .Column("Name", o.Name)
                            .Column("Title", o.Title)
                            .Column("Stock", o.Stock)
                            .Column("Price", o.Price)
                            .Column("AppPrice", o.AppPrice)
                            .Column("RetailPrice", o.RetailPrice)
                            .Column("Code", o.Code ?? string.Empty)
                            .Column("Barcode", o.Barcode ?? string.Empty)
                            .Where("Id=@id and SellerId=@sellerId", new { o.Id, o.SellerId })
                            .ToCommand(tran);

                            var v = conn.Execute(cmd) > 0;

                            if (v && o.Skus != null && o.Skus.Any())
                            {
                                foreach (var sku in o.Skus)
                                {
                                    cmd = SqlBuilder
                                    .Update("Sku")
                                    .Column("Stock", sku.Stock)
                                    .Column("Price", sku.Price)
                                    .Column("AppPrice", o.AppPrice)
                                    .Column("RetailPrice", o.RetailPrice)
                                    .Column("Code", sku.Code ?? string.Empty)
                                    .Column("Barcode", sku.Barcode ?? string.Empty)
                                    .Where("Id=@id and ItemId=@itemId and SellerId=@sellerId", new { sku.Id, itemId = o.Id, o.SellerId })
                                    .ToCommand(tran);

                                    v = conn.Execute(cmd) > 0;

                                    if (!v)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (v)
                            {
                                tran.Commit();
                                return true;
                            }
                            tran.Rollback();
                            return false;
                        }
                        catch (Exception)
                        {
                            tran.Rollback();
                            throw;
                        }
                    }
                }
            });
        }

        #endregion

        /*
        public Prod.Current ProdCurrentGet(int id, Platform Platform)
        {
            var o = new Prod.Current();
            var sql = @"select Id,Stock,Price,AppPrice,RetailPrice,SellerId from item where id=@id;
            select Id,Stock,Price,AppPrice,RetailPrice from sku where itemid=@id;";
            var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
            using (var reader = StoreConn.QueryMultiple(cmd))
            {
                o = reader.Read<Prod.Current>().FirstOrDefault();
                if (o != null)
                {
                    o.Skus = reader.Read<Prod.Current.Sku>().ToList();
                }
            }

            sql = @"
            select b.Id,b.Platform,a.ItemId,a.Global,a.Value,a.SkuJson
            from SpecialPriceItem a inner join SpecialPrice b
            on a.ParentId=b.Id
            where a.Status=1 and b.Status=1 
            and b.StartedOn<now() and b.StoppedOn>now()
            and b.SellerId=@sellerId
            ";
            var specials = PromotionConn.Query<Prod.Current.Special>(sql, new { o.SellerId }).ToList();

            return o;


        }
    */
    }
}