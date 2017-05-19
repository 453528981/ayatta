using Dapper;
using System.Linq;
using Ayatta.Domain;
using System.Collections.Generic;

namespace Ayatta.Storage
{
    public partial class DefaultStorage
    {
        public Catg CatgGet(int id)
        {
            var sql = @"select * from catg where id=@id;
                select * from catgprop where catgid=@id;
                select * from catgpropvalue where catgid=@id;
                ";
            using (BaseConn)
            using (var multi = BaseConn.QueryMultiple(sql, new { id }, commandTimeout: 60))
            {
                var catg = multi.Read<Catg>().FirstOrDefault();
                var props = multi.Read<Catg.Prop>().ToList();
                var propValues = multi.Read<Catg.Prop.Value>().ToList();

                BaseConn.Close();

                if (catg != null)
                {
                    foreach (var prop in props)
                    {
                        prop.Values = propValues.Where(x => x.PropId == prop.Id).ToList();
                    }
                    catg.Props = props;
                }
                return catg;
            }
        }

        public Catg.Mini CatgMiniGet(int id)
        {
            var sql = @"select Id,Name,IsParent,ParentId,Priority from catg where id=@id;
                select Id,ParentPid,ParentVid,Name,Must,Multi,IsKeyProp,IsSaleProp,IsEnumProp,IsItemProp,IsColorProp,IsInputProp  from catgprop where catgid=@id;
                select Id,PropId,Name from catgpropvalue where catgid=@id;
                ";
            using (BaseConn)
            using (var multi = BaseConn.QueryMultiple(sql, new { id }, commandTimeout: 60))
            {
                var catg = multi.Read<Catg.Mini>().FirstOrDefault();
                var props = multi.Read<Catg.Mini.Prop>().ToList();
                var propValues = multi.Read<Catg.Mini.Prop.Value>().ToList();

                BaseConn.Close();

                if (catg != null)
                {
                    foreach (var prop in props)
                    {
                        prop.Values = propValues.Where(x => x.PropId == prop.Id).ToList();
                    }
                    catg.Props = props;
                }
                return catg;
            }
        }

        public IList<Catg.Tiny> CatgTinyList()
        {
            var sql = @"select id,name,parentid from catg where status=0";

            return BaseConn.Query<Catg.Tiny>(sql).ToList();
        }
        public IList<Catg> CatgList()
        {
            var sql = @"select * from catg order by Priority";
            var cmd = SqlBuilder.Raw(sql).ToCommand();
            return BaseConn.Query<Catg>(cmd).ToList();
        }

        public IList<Catg> CatgChildren(int catgId)
        {
            var sql = @"select * from catg where parentId=@catgId order by Priority";
            var cmd = SqlBuilder.Raw(sql, new { catgId }).ToCommand();
            return BaseConn.Query<Catg>(cmd).ToList();
        }

        public IList<Catg.Prop> CatgPropList(int catgId)
        {
            var sql = @"select * from catgprop where catgid=@catgId order by IsKeyProp desc,IsSaleProp desc,Priority";
            var cmd = SqlBuilder.Raw(sql, new { catgId }).ToCommand();
            return BaseConn.Query<Catg.Prop>(cmd).ToList();
        }
        public IList<Catg.Prop.Value> CatgPropValueList(int catgId, int propId)
        {
            var sql = @"select * from catgpropvalue where catgid=@catgId and propid=@propId order by Priority";
            var cmd = SqlBuilder.Raw(sql, new { catgId, propId }).ToCommand();
            return BaseConn.Query<Catg.Prop.Value>(cmd).ToList();
        }

        #region OAuthProvider
        ///<summary>
        /// OAuthProviderCreate
        ///</summary>
        ///<param name="o">OAuthProvider</param>
        ///<returns></returns>
        public bool OAuthProviderCreate(OAuthProvider o)
        {
            var cmd = SqlBuilder.Insert("OAuthProvider")
                .Column("Id", o.Id)
                .Column("Name", o.Name)
                .Column("ClientId", o.ClientId)
                .Column("ClientSecret", o.ClientSecret)
                .Column("Scope", o.Scope)
                .Column("CallbackEndpoint", o.CallbackEndpoint)
                .Column("BaseUrl", o.BaseUrl)
                .Column("AuthorizationEndpoint", o.AuthorizationEndpoint)
                .Column("TokenEndpoint", o.TokenEndpoint)
                .Column("UserEndpoint", o.UserEndpoint)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge)
                .Column("Extra", o.Extra)
                .Column("Status", o.Status)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand();
            return BaseConn.Execute(cmd) > 0;
        }

        ///<summary>
        /// OAuthProviderUpdate
        ///</summary>
        ///<param name="o">OAuthProvider</param>
        ///<returns></returns>
        public bool OAuthProviderUpdate(OAuthProvider o)
        {
            var cmd = SqlBuilder.Update("OAuthProvider")
            .Column("Name", o.Name)
            .Column("ClientId", o.ClientId)
            .Column("ClientSecret", o.ClientSecret)
            .Column("Scope", o.Scope)
            .Column("CallbackEndpoint", o.CallbackEndpoint)
            .Column("BaseUrl", o.BaseUrl)
            .Column("AuthorizationEndpoint", o.AuthorizationEndpoint)
            .Column("TokenEndpoint", o.TokenEndpoint)
            .Column("UserEndpoint", o.UserEndpoint)
            .Column("Priority", o.Priority)
            .Column("Badge", o.Badge)
            .Column("Extra", o.Extra)
            .Column("Status", o.Status)
            .Column("ModifiedBy", o.ModifiedBy)
            .Column("ModifiedOn", o.ModifiedOn)
            .Where("Id=@id", new { o.Id })
            .ToCommand();
            return BaseConn.Execute(cmd) > 0;
        }

        ///<summary>
        /// OAuthProviderGet
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public OAuthProvider OAuthProviderGet(string id)
        {
            var sql = @"select * from OAuthProvider where id=@id";
            var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
            return BaseConn.QueryFirstOrDefault<OAuthProvider>(cmd);
        }

        /**
        ///<summary>
        /// OAuthProviderGet
        /// created on 2016-07-14 17:00:13
        ///</summary>
        ///<param name="name">name</param>
        ///<returns></returns>
        public OAuthProvider OAuthProviderGet(string name)
        {
            var sql = @"select * from OAuthProvider where name=@name";
            var cmd = SqlBuilder.Raw(sql, new { name }).ToCommand();
            return BaseConn.QueryFirstOrDefault<OAuthProvider>(cmd);
        }
    */
        ///<summary>
        /// OAuthProviderList
        ///</summary>
        ///<returns></returns>
        public IList<OAuthProvider> OAuthProviderList()
        {

            var sql = @"select * from OAuthProvider";
            var cmd = SqlBuilder.Raw(sql).ToCommand();
            return BaseConn.Query<OAuthProvider>(cmd).ToList();
        }

        #endregion

        #region Help
        ///<summary>
        /// HelpCreate
        ///</summary>
        ///<param name="o">Help</param>
        ///<returns></returns>
        public int HelpCreate(Help o)
        {
            var cmd = SqlBuilder.Insert("Help")
            .Column("ParentId", o.ParentId)
            .Column("Title", o.Title)
            .Column("NavUrl", o.NavUrl)
            .Column("Content", o.Content)
            .Column("Priority", o.Priority)
            .Column("Extra", o.Extra)
            .Column("Status", o.Status)
            .Column("CreatedOn", o.CreatedOn)
            .Column("ModifiedBy", o.ModifiedBy)
            .Column("ModifiedOn", o.ModifiedOn)
            .ToCommand(true);
            return BaseConn.ExecuteScalar<int>(cmd);
        }

        ///<summary>
        /// HelpUpdate
        ///</summary>
        ///<param name="o">Help</param>
        ///<returns></returns>
        public bool HelpUpdate(Help o)
        {
            var cmd = SqlBuilder.Update("Help")
            .Column("ParentId", o.ParentId)
            .Column("Title", o.Title)
            .Column("NavUrl", o.NavUrl)
            .Column("Content", o.Content)
            .Column("Priority", o.Priority)
            .Column("Extra", o.Extra)
            .Column("Status", o.Status)
            .Column("ModifiedBy", o.ModifiedBy)
            .Column("ModifiedOn", o.ModifiedOn)
            .Where("Id=@id", new { o.Id })
            .ToCommand();
            return BaseConn.Execute(cmd) > 0;
        }

        ///<summary>
        /// HelpGet
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public Help HelpGet(int id)
        {
            var sql = @"select * from Help where id=@id";
            var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
            return BaseConn.QueryFirstOrDefault<Help>(cmd);
        }

        #endregion

        #region 国家
        ///<summary>
        /// 国家创建
        ///</summary>
        ///<param name="o">国家</param>
        ///<returns></returns>
        public bool CountryCreate(Country o)
        {
            return Try(nameof(CountryCreate), () =>
            {
                var cmd = SqlBuilder.Insert("Country")
                .Column("Id", o.Id)
                .Column("Code", o.Code)
                .Column("Name", o.Name)
                .Column("Flag", o.Flag)
                .Column("EnName", o.EnName)
                .Column("Extra", o.Extra)
                .Column("Status", o.Status)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 国家更新
        ///</summary>
        ///<param name="o">国家</param>
        ///<returns></returns>
        public bool CountryUpdate(Country o)
        {
            return Try(nameof(CountryCreate), () =>
            {
                var cmd = SqlBuilder.Update("Country")
                .Column("Code", o.Code)
                .Column("Name", o.Name)
                .Column("Flag", o.Flag)
                .Column("EnName", o.EnName)
                .Column("Extra", o.Extra)
                .Column("Status", o.Status)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .Where("Id=@id", new { o.Id })
                .ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 国家获取
        ///</summary>
        ///<param name="id">三位数字代码</param>
        ///<returns></returns>
        public Country CountryGet(int id)
        {
            return Try(nameof(CountryGet), () =>
            {
                var sql = @"select * from Country where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.QueryFirstOrDefault<Country>(cmd);
            });
        }

        ///<summary>
        /// 国家获取
        ///</summary>
        ///<param name="code">三位字母代码</param>
        ///<returns></returns>
        public Country CountryGet(string code)
        {
            return Try(nameof(CountryGet), () =>
            {
                var sql = @"select * from Country where code=@code";
                var cmd = SqlBuilder.Raw(sql, new { code }).ToCommand();
                return BaseConn.QueryFirstOrDefault<Country>(cmd);
            });
        }

        ///<summary>
        /// 国家是否存在
        ///</summary>
        ///<param name="id">三位数字代码</param>
        ///<returns></returns>
        public bool CountryExist(int id)
        {
            return Try(nameof(CountryExist), () =>
            {
                var sql = @"select 1 from Country where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.ExecuteScalar<bool>(cmd);
            });
        }

        ///<summary>
        /// 国家是否存在
        ///</summary>
        ///<param name="code">三位字母代码</param>
        ///<returns></returns>
        public bool CountryExist(string code)
        {
            return Try(nameof(CountryExist), () =>
            {
                var sql = @"select 1 from Country where code=@code";
                var cmd = SqlBuilder.Raw(sql, new { code }).ToCommand();
                return BaseConn.ExecuteScalar<bool>(cmd);
            });
        }

        #endregion

        #region 中国行政区(省市区)
        ///<summary>
        /// 行政区创建
        ///</summary>
        ///<param name="o">行政区</param>
        ///<returns></returns>
        public bool RegionCreate(Region o)
        {
            var cmd = SqlBuilder.Insert("Region")
             .Column("Id", o.Id)
            .Column("ParentId", o.ParentId)
            .Column("Name", o.Name)
            .Column("PostalCode", o.PostalCode)
            .Column("GroupId", o.GroupId)
            .Column("Priority", o.Priority)
            .Column("Badge", o.Badge)
            .Column("Extra", o.Extra)
            .Column("Status", o.Status)
            .Column("CreatedOn", o.CreatedOn)
            .Column("ModifiedBy", o.ModifiedBy)
            .Column("ModifiedOn", o.ModifiedOn)
            .ToCommand(true);
            return BaseConn.Execute(cmd) > 0;
        }

        ///<summary>
        /// 行政区更新
        ///</summary>
        ///<param name="o">行政区</param>
        ///<returns></returns>
        public bool RegionUpdate(Region o)
        {
            var cmd = SqlBuilder.Update("Region")
            .Column("ParentId", o.ParentId)
            .Column("Name", o.Name)
            .Column("PostalCode", o.PostalCode)
            .Column("GroupId", o.GroupId)
            .Column("Priority", o.Priority)
            .Column("Badge", o.Badge)
            .Column("Extra", o.Extra)
            .Column("Status", o.Status)
            .Column("ModifiedBy", o.ModifiedBy)
            .Column("ModifiedOn", o.ModifiedOn)
            .Where("Id=@id and ParentId=@parentId", new { o.Id, o.ParentId })
            .ToCommand();
            return BaseConn.Execute(cmd) > 0;
        }

        ///<summary>
        /// 行政区获取
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public Region RegionGet(string id)
        {
            var sql = @"select * from Region where id=@id";
            var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
            return BaseConn.QueryFirstOrDefault<Region>(cmd);
        }

        ///<summary>
        /// 行政区列表
        ///</summary>
        ///<returns></returns>
        public IList<Region> RegionList()
        {
            var sql = @"select * from Region";
            var cmd = SqlBuilder.Raw(sql).ToCommand();
            return BaseConn.Query<Region>(cmd).ToList();
        }

        /// <summary>
        /// 行政区列表
        /// </summary>
        /// <param name="parentId">父id</param>
        /// <returns></returns>
        public IList<Region> RegionList(string parentId)
        {
            var sql = @"select * from Region where ParentId=@parentId";
            var cmd = SqlBuilder.Raw(sql, new { parentId }).ToCommand();
            return BaseConn.Query<Region>(cmd).ToList();
        }

        /// <summary>
        /// 行政区列表分页
        /// </summary>
        /// <param name="page">分页</param>
        /// <param name="size">分页大小</param>
        /// <param name="parentId">parentId</param>
        /// <returns></returns>
        public IPagedList<Region> RegionPagedList(int page = 1, int size = 50, string parentId = null)
        {
            var cmd = SqlBuilder.Select("*").From("Region")
                .Where(!string.IsNullOrEmpty(parentId), "parentId=@parentId", new { parentId })
                .ToCommand(page, size);

            return BaseConn.PagedList<Region>(page, size, cmd);
        }

        #endregion

        #region 银行
        ///<summary>
        /// 银行创建
        ///</summary>
        ///<param name="o">银行</param>
        ///<returns></returns>
        public int BankCreate(Bank o)
        {
            var cmd = SqlBuilder.Insert("Bank")
            .Column("Name", o.Name)
            .Column("IconSrc", o.IconSrc)
            .Column("Description", o.Description)
            .Column("Priority", o.Priority)
            .Column("Badge", o.Badge)
            .Column("Extra", o.Extra)
            .Column("Status", o.Status)
            .Column("CreatedOn", o.CreatedOn)
            .Column("ModifiedBy", o.ModifiedBy)
            .Column("ModifiedOn", o.ModifiedOn)
            .ToCommand(true);
            return BaseConn.ExecuteScalar<int>(cmd);
        }

        ///<summary>
        /// 银行更新
        ///</summary>
        ///<param name="o">银行</param>
        ///<returns></returns>
        public bool BankUpdate(Bank o)
        {
            var cmd = SqlBuilder.Update("Bank")
            .Column("Name", o.Name)
            .Column("IconSrc", o.IconSrc)
            .Column("Description", o.Description)
            .Column("Priority", o.Priority)
            .Column("Badge", o.Badge)
            .Column("Extra", o.Extra)
            .Column("Status", o.Status)
            .Column("ModifiedBy", o.ModifiedBy)
            .Column("ModifiedOn", o.ModifiedOn)
            .Where("Id=@id", new { o.Id })
            .ToCommand();
            return BaseConn.Execute(cmd) > 0;
        }

        ///<summary>
        /// 银行获取
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public Bank BankGet(int id)
        {
            var sql = @"select * from Bank where id=@id";
            var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
            return BaseConn.QueryFirstOrDefault<Bank>(cmd);
        }


        #endregion

        #region 支付平台
        ///<summary>
        /// 创建支付平台
        ///</summary>
        ///<param name="o">PaymentPlatform</param>
        ///<returns></returns>
        public int PaymentPlatformCreate(PaymentPlatform o)
        {
            var cmd = SqlBuilder.Insert("PaymentPlatform")
            .Column("Name", o.Name)
            .Column("IconSrc", o.IconSrc)
            .Column("MerchantId", o.MerchantId)
            .Column("PrivateKey", o.PrivateKey)
            .Column("PublicKey", o.PublicKey)
            .Column("GatewayUrl", o.GatewayUrl)
            .Column("CallbackUrl", o.CallbackUrl)
            .Column("NotifyUrl", o.NotifyUrl)
            .Column("QueryUrl", o.QueryUrl)
            .Column("RefundUrl", o.RefundUrl)
            .Column("Description", o.Description)
            .Column("Priority", o.Priority)
            .Column("Badge", o.Badge)
            .Column("Extra", o.Extra)
            .Column("Status", o.Status)
            .Column("CreatedOn", o.CreatedOn)
            .Column("ModifiedBy", o.ModifiedBy)
            .Column("ModifiedOn", o.ModifiedOn)
            .ToCommand(true);
            return BaseConn.ExecuteScalar<int>(cmd);
        }

        ///<summary>
        /// 更新支付平台
        ///</summary>
        ///<param name="o">PaymentPlatform</param>
        ///<returns></returns>
        public bool PaymentPlatformUpdate(PaymentPlatform o)
        {
            var cmd = SqlBuilder.Update("PaymentPlatform")
            .Column("Name", o.Name)
            .Column("IconSrc", o.IconSrc)
            .Column("MerchantId", o.MerchantId)
            .Column("PrivateKey", o.PrivateKey)
            .Column("PublicKey", o.PublicKey)
            .Column("GatewayUrl", o.GatewayUrl)
            .Column("CallbackUrl", o.CallbackUrl)
            .Column("NotifyUrl", o.NotifyUrl)
            .Column("QueryUrl", o.QueryUrl)
            .Column("RefundUrl", o.RefundUrl)
            .Column("Description", o.Description)
            .Column("Priority", o.Priority)
            .Column("Badge", o.Badge)
            .Column("Extra", o.Extra)
            .Column("Status", o.Status)
            .Column("ModifiedBy", o.ModifiedBy)
            .Column("ModifiedOn", o.ModifiedOn)
            .Where("Id=@id", new { o.Id })
            .ToCommand();
            return BaseConn.Execute(cmd) > 0;
        }

        ///<summary>
        /// 获取支付平台
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public PaymentPlatform PaymentPlatformGet(int id)
        {
            var sql = @"select * from PaymentPlatform where id=@id";
            var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
            return BaseConn.QueryFirstOrDefault<PaymentPlatform>(cmd);
        }

        ///<summary>
        /// 获取支付平台
        ///</summary>
        ///<returns></returns>
        public IList<PaymentPlatform> PaymentPlatformList()
        {
            var sql = @"select * from PaymentPlatform";
            var cmd = SqlBuilder.Raw(sql).ToCommand();
            return BaseConn.Query<PaymentPlatform>(cmd).ToList();
        }

        #endregion

        #region 支付平台银行
        ///<summary>
        /// 支付平台银行创建
        ///</summary>
        ///<param name="o">支付平台银行</param>
        ///<returns></returns>
        public int PaymentBankCreate(PaymentBank o)
        {

            return Try(nameof(PaymentBankCreate), () =>
            {
                var cmd = SqlBuilder.Insert("PaymentBank")
                            .Column("BankId", o.BankId)
                            .Column("PlatformId", o.PlatformId)
                            .Column("Code", o.Code)
                            .Column("Description", o.Description)
                            .Column("Priority", o.Priority)
                            .Column("Badge", o.Badge)
                            .Column("Extra", o.Extra)
                            .Column("Status", o.Status)
                            .Column("CreatedOn", o.CreatedOn)
                            .Column("ModifiedBy", o.ModifiedBy)
                            .Column("ModifiedOn", o.ModifiedOn)
                            .ToCommand(true);
                return BaseConn.ExecuteScalar<int>(cmd);
            });
        }

        ///<summary>
        /// 支付平台银行更新
        ///</summary>
        ///<param name="o">支付平台银行</param>
        ///<returns></returns>
        public bool PaymentBankUpdate(PaymentBank o)
        {
            var cmd = SqlBuilder.Update("PaymentBank")
            .Column("BankId", o.BankId)
            .Column("PlatformId", o.PlatformId)
            .Column("Code", o.Code)
            .Column("Description", o.Description)
            .Column("Priority", o.Priority)
            .Column("Badge", o.Badge)
            .Column("Extra", o.Extra)
            .Column("Status", o.Status)
            .Column("ModifiedBy", o.ModifiedBy)
            .Column("ModifiedOn", o.ModifiedOn)
            .Where("Id=@id", new { o.Id })
            .ToCommand();
            return BaseConn.Execute(cmd) > 0;
        }

        ///<summary>
        /// 支付平台银行获取
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public PaymentBank PaymentBankGet(int id)
        {
            var sql = @"select * from PaymentBank where id=@id";
            var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
            return BaseConn.QueryFirstOrDefault<PaymentBank>(cmd);
        }

        /// <summary>
        /// 支付平台下的银行列表
        /// </summary>
        /// <param name="platformId">支付平台id</param>
        /// <returns></returns>
        public IList<PaymentBank> PaymentBankList(int platformId)
        {
            var sql = @"select * from PaymentBank a inner join PaymentPlatform b on a.PlatformId=b.Id inner join PaymentBank c on a.BankId=c.Id where a.PlatformId=@platformId";
            var cmd = SqlBuilder.Raw(sql).ToCommand();
            return BaseConn.Query<PaymentBank, PaymentPlatform, Bank>(sql, new { platformId }).ToList();
        }

        #endregion

        #region 幻灯片

        ///<summary>
        /// 幻灯片创建
        ///</summary>
        ///<param name="o">幻灯片</param>
        ///<returns></returns>
        public bool SlideCreate(Slide o)
        {
            return Try(nameof(SlideCreate), () =>
            {
                var cmd = SqlBuilder.Insert("Slide")
                .Column("Id", o.Id)
                .Column("Name", o.Name)
                .Column("Title", o.Title)
                .Column("Width", o.Width)
                .Column("Height", o.Height)
                .Column("Thumb", o.Thumb)
                .Column("ThumbW", o.ThumbW)
                .Column("ThumbH", o.ThumbH)
                .Column("Description", o.Description)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge)
                .Column("Extra", o.Extra)
                .Column("Status", o.Status)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand();

                return BaseConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 幻灯片更新
        ///</summary>
        ///<param name="o">Slide</param>
        ///<returns></returns>
        public bool SlideUpdate(Slide o)
        {
            return Try(nameof(SlideUpdate), () =>
            {
                var cmd = SqlBuilder.Update("Slide")
                .Column("Name", o.Name)
                .Column("Title", o.Title)
                .Column("Width", o.Width)
                .Column("Height", o.Height)
                .Column("Thumb", o.Thumb)
                .Column("ThumbW", o.ThumbW)
                .Column("ThumbH", o.ThumbH)
                .Column("Description", o.Description)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge)
                .Column("Extra", o.Extra)
                .Column("Status", o.Status)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .Where("Id=@id", new { o.Id })
                .ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        /// <summary>
        /// 幻灯片获取
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="includeItems">是否包含条目</param>
        /// <param name="current">是否筛选当前时间生效的条目</param>
        /// <returns></returns>
        public Slide SlideGet(string id, bool includeItems, bool current = false)
        {
            return Try(nameof(ItemGet), () =>
            {
                if (includeItems)
                {
                    var where = current ? " and StartedOn<=now() and StoppedOn>=now()" : string.Empty;
                    var sql = @"select * from Slide where id=@id;select * from SlideItem where SlideId=@id " + where + ";";
                    var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                    using (var reader = StoreConn.QueryMultiple(cmd))
                    {
                        var o = reader.Read<Slide>().FirstOrDefault();
                        if (o != null)
                        {
                            o.Items = reader.Read<SlideItem>().ToList();
                        }
                        return o;
                    }
                }
                else
                {
                    var sql = @"select * from Slide where id=@id";
                    var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                    return BaseConn.QueryFirstOrDefault<Slide>(cmd);
                }
            });
        }

        /// <summary>
        /// 幻灯片删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool SlideDelete(string id)
        {
            return Try(nameof(SlideItemGet), () =>
            {
                var sql = @"delete * from SlideItem where SlideId=@id;delete * from Slide where id=@id;";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 幻灯片条目创建
        ///</summary>
        ///<param name="o">幻灯片条目</param>
        ///<returns></returns>
        public int SlideItemCreate(SlideItem o)
        {
            return Try(nameof(SlideItemCreate), () =>
            {
                var cmd = SqlBuilder.Insert("SlideItem")
                .Column("SlideId", o.SlideId)
                .Column("GroupId", o.GroupId)
                .Column("Name", o.Name)
                .Column("Title", o.Title)
                .Column("NavUrl", o.NavUrl)
                .Column("ImageSrc", o.ImageSrc)
                .Column("ThumbSrc", o.ThumbSrc)
                .Column("Description", o.Description)
                .Column("StartedOn", o.StartedOn)
                .Column("StoppedOn", o.StoppedOn)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge)
                .Column("Extra", o.Extra)
                .Column("Status", o.Status)
                .Column("CreatedOn", o.CreatedOn)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .ToCommand(true);
                return BaseConn.ExecuteScalar<int>(cmd);
            });
        }

        ///<summary>
        /// 幻灯片条目更新
        ///</summary>
        ///<param name="o">幻灯片条目</param>
        ///<returns></returns>
        public bool SlideItemUpdate(SlideItem o)
        {
            return Try(nameof(SlideItemUpdate), () =>
            {
                var cmd = SqlBuilder.Update("SlideItem")
                .Column("SlideId", o.SlideId)
                .Column("GroupId", o.GroupId)
                .Column("Name", o.Name)
                .Column("Title", o.Title)
                .Column("NavUrl", o.NavUrl)
                .Column("ImageSrc", o.ImageSrc)
                .Column("ThumbSrc", o.ThumbSrc)
                .Column("Description", o.Description)
                .Column("StartedOn", o.StartedOn)
                .Column("StoppedOn", o.StoppedOn)
                .Column("Priority", o.Priority)
                .Column("Badge", o.Badge)
                .Column("Extra", o.Extra)
                .Column("Status", o.Status)
                .Column("ModifiedBy", o.ModifiedBy)
                .Column("ModifiedOn", o.ModifiedOn)
                .Where("Id=@id", new { o.Id })
                .ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }

        ///<summary>
        /// 幻灯片条目获取
        ///</summary>
        ///<param name="id">id</param>
        ///<returns></returns>
        public SlideItem SlideItemGet(int id)
        {
            return Try(nameof(SlideItemGet), () =>
            {
                var sql = @"select * from SlideItem where id=@id";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.QueryFirstOrDefault<SlideItem>(cmd);
            });
        }

        /// <summary>
        /// 幻灯片条目删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool SlideItemDelete(string id)
        {
            return Try(nameof(SlideItemDelete), () =>
            {
                var sql = @"delete * from SlideItem where id=@id;";
                var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                return BaseConn.Execute(cmd) > 0;
            });
        }
        #endregion

    }
}