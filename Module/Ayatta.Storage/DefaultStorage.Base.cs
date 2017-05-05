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
            .Column("GroupId", o.GroupId)
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
            .Column("GroupId", o.GroupId)
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

        #region Region
        ///<summary>
        /// RegionCreate
        ///</summary>
        ///<param name="o">Region</param>
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
        /// RegionUpdate
        ///</summary>
        ///<param name="o">Region</param>
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
        /// RegionGet
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
        /// RegionList
        ///</summary>
        ///<returns></returns>
        public IList<Region> RegionList()
        {
            var sql = @"select * from Region";
            var cmd = SqlBuilder.Raw(sql).ToCommand();
            return BaseConn.Query<Region>(cmd).ToList();
        }

        /// <summary>
        /// RegionPagedList
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

        #region Bank
        ///<summary>
        /// BankCreate
        ///</summary>
        ///<param name="o">Bank</param>
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
        /// BankUpdate
        ///</summary>
        ///<param name="o">Bank</param>
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
        /// BankGet
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

        #region PaymentBank
        ///<summary>
        /// PaymentBankCreate
        ///</summary>
        ///<param name="o">PaymentBank</param>
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
        /// PaymentBankUpdate
        ///</summary>
        ///<param name="o">PaymentBank</param>
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
        /// PaymentBankGet
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
        /// 获取支付平台下的银行
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

    }
}