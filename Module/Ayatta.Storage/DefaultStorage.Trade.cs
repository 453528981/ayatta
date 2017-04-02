using System;
using Dapper;
using System.Linq;
using Ayatta.Domain;

namespace Ayatta.Storage
{

    public partial class DefaultStorage
    {
        #region 订单

        ///<summary>
        /// 订单创建
        ///</summary>
        ///<param name="o">订单</param>
        ///<returns></returns>
        public bool OrderCreate(Order o)
        {
            return Try(nameof(OrderCreate), () =>
            {
                using (var conn = TradeConn)
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            var cmd = SqlBuilder.Insert("OrderInfo")
                                .Column("Id", o.Id)
                                .Column("Type", o.Type)
                                .Column("Quantity", o.Quantity)
                                .Column("SubTotal", o.SubTotal)
                                .Column("Freight", o.Freight)
                                .Column("Tax", o.Tax)
                                .Column("Discount", o.Discount)
                                .Column("Total", o.Total)
                                .Column("Paid", o.Paid)
                                .Column("PayId", o.PayId)
                                .Column("PaidOn", o.PaidOn)
                                .Column("PointUse", o.PointUse)
                                .Column("PointRealUse", o.PointRealUse)
                                .Column("PointReward", o.PointReward)
                                .Column("Coupon", o.Coupon)
                                .Column("CouponUse", o.CouponUse)
                                .Column("GiftCard", o.GiftCard)
                                .Column("GiftCardUse", o.GiftCardUse)
                                .Column("PromotionData", o.PromotionData)
                                .Column("Weight", o.Weight)
                                .Column("ETicket", o.ETicket)
                                .Column("IsVirtual", o.IsVirtual)
                                .Column("IsBonded", o.IsBonded)
                                .Column("IsOversea", o.IsOversea)
                                .Column("PaymentType", o.PaymentType)
                                .Column("PaymentData", o.PaymentData)
                                .Column("ShipmentType", o.ShipmentType)
                                .Column("ShipmentData", o.ShipmentData)                                
                                .Column("ExpiredOn", o.ExpiredOn)
                                .Column("ConsignedOn", o.ConsignedOn)
                                .Column("FinishedOn", o.FinishedOn)
                                .Column("InvoiceType", o.InvoiceType)
                                .Column("InvoiceTitle", o.InvoiceTitle)
                                .Column("InvoiceContent", o.InvoiceContent)
                                .Column("InvoiceStatus", o.InvoiceStatus)
                                .Column("LogisticsNo", o.LogisticsNo)
                                .Column("LogisticsType", o.LogisticsType)
                                .Column("LogisticsCode", o.LogisticsCode)
                                .Column("LogisticsName", o.LogisticsName)
                                .Column("Receiver", o.Receiver)
                                .Column("ReceiverPhone", o.ReceiverPhone)
                                .Column("ReceiverMobile", o.ReceiverMobile)
                                .Column("ReceiverRegionId", o.ReceiverRegionId)
                                .Column("ReceiverProvince", o.ReceiverProvince)
                                .Column("ReceiverCity", o.ReceiverCity)
                                .Column("ReceiverDistrict", o.ReceiverDistrict)
                                .Column("ReceiverStreet", o.ReceiverStreet)
                                .Column("ReceiverPostalCode", o.ReceiverPostalCode)
                                .Column("BuyerId", o.BuyerId)
                                .Column("BuyerName", o.BuyerName)
                                .Column("BuyerFlag", o.BuyerFlag)
                                .Column("BuyerMemo", o.BuyerMemo)
                                .Column("BuyerRated", o.BuyerRated)
                                .Column("BuyerMessage", o.BuyerMessage)
                                .Column("SellerId", o.SellerId)
                                .Column("SellerName", o.SellerName)
                                .Column("SellerFlag", o.SellerFlag)
                                .Column("SellerMemo", o.SellerMemo)
                                .Column("HasReturn", o.HasReturn)
                                .Column("HasRefund", o.HasRefund)
                                .Column("CancelId", o.CancelId)
                                .Column("CancelReason", o.CancelReason)
                                .Column("RelatedId", o.RelatedId)
                                .Column("MediaId", o.MediaId)
                                .Column("TraceCode", o.TraceCode)
                                .Column("IpAddress", o.IpAddress)
                                .Column("Extra", o.Extra)
                                .Column("Status", o.Status)
                                .Column("CreatedBy", o.CreatedBy)
                                .Column("CreatedOn", o.CreatedOn)
                                .Column("ModifiedBy", o.ModifiedBy)
                                .Column("ModifiedOn", o.ModifiedOn)

                                .ToCommand(false, tran);


                            var status = conn.Execute(cmd) > 0;

                            if (status)
                            {
                                foreach (var item in o.Items)
                                {
                                    cmd = SqlBuilder.Insert("OrderItem")
                                        .Column("Id", item.Id)
                                        .Column("OrderId", item.OrderId)
                                        .Column("SpuId", item.SpuId)
                                        .Column("ItemId", item.ItemId)
                                        .Column("SkuId", item.SkuId)
                                        .Column("CatgRId", item.CatgRId)
                                        .Column("CatgMId", item.CatgMId)
                                        .Column("CatgId", item.CatgId)
                                        .Column("PackageId", item.PackageId)
                                        .Column("PackageName", item.PackageName)
                                        .Column("Code", item.Code)
                                        .Column("Name", item.Name)
                                        .Column("Price", item.Price)
                                        .Column("PriceShow", item.PriceShow)
                                        .Column("Quantity", item.Quantity)
                                        .Column("Tax", item.Tax)
                                        .Column("Adjust", item.Adjust)
                                        .Column("Discount", item.Discount)
                                        .Column("Total", item.Total)
                                        .Column("TaxRate", item.TaxRate)
                                        .Column("Picture", item.Picture)
                                        .Column("PropText", item.PropText)
                                        .Column("IsGift", item.IsGift)
                                        .Column("IsVirtual", item.IsVirtual)
                                        .Column("IsService", item.IsService)
                                        .Column("PromotionData", item.PromotionData)
                                        .Column("ExpiredOn", item.ExpiredOn)
                                        .Column("ConsignedOn", item.ConsignedOn)
                                        .Column("FinishedOn", item.FinishedOn)
                                        .Column("LogisticsNo", item.LogisticsNo)
                                        .Column("LogisticsName", item.LogisticsName)
                                        .Column("RetrunId", item.RetrunId)
                                        .Column("RetrunStatus", item.RetrunStatus)
                                        .Column("RefundId", item.RefundId)
                                        .Column("RefundStatus", item.RefundStatus)
                                        .Column("BuyerId", item.BuyerId)
                                        .Column("BuyerName", item.BuyerName)
                                        .Column("BuyerRated", item.BuyerRated)
                                        .Column("SellerId", item.SellerId)
                                        .Column("SellerName", item.SellerName)
                                        .Column("Extra", item.Extra)
                                        .Column("Status", item.Status)
                                        .Column("CreatedOn", item.CreatedOn)
                                        .Column("ModifiedBy", item.ModifiedBy)
                                        .Column("ModifiedOn", item.ModifiedOn)
                                        .ToCommand(false, tran);

                                    status = conn.Execute(cmd) > 0;
                                    if (!status)
                                    {
                                        break;
                                    }
                                }
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
        /// 订单状态更新
        /// </summary>
        /// <param name="id">订单id</param>
        /// <param name="status">订单状态</param>
        /// <returns></returns>
        public bool OrderStatusUpdate(string id, OrderStatus status)
        {
            return Try(nameof(OrderStatusUpdate), () =>
            {
                var sql = @"update orderinfo set status=@statue where id=@id";

                return TradeConn.Execute(sql, new { id, status }) > 0;
            });
        }

        /// <summary>
        /// 订单已付金额及状态更新
        /// </summary>
        /// <param name="id">订单Id</param>
        /// <param name="amount">本次支金额</param>
        /// <param name="paidUp">是否已付清(付清后如果是在线支付订单则会更新订单状态为已付款待发货并更新订单有效期)</param>
        /// <returns></returns>
        public bool OrderPaid(string id, decimal amount, bool paidUp)
        {
            return Try(nameof(OrderPaid), () =>
            {
                var sql = @"update orderinfo set Paid=Paid+@amount,PaidOn=now() where id=@id";
                if (paidUp)
                {
                    sql = @"update orderinfo set Paid=Paid+@amount,PaidOn=now(),ExpiredOn=date_add(now(),interval 1 week),status=2 where id=@id and ";
                }
                return TradeConn.Execute(sql, new { id, amount }) > 0;
            });
        }

        /// <summary>
        /// 订单获取
        /// </summary>
        /// <param name="id">订单id</param>
        /// <param name="includeItems">是否包含明细</param>
        /// <returns></returns>
        public Order OrderGet(string id, bool includeItems = false)
        {
            return Try(nameof(OrderGet), () =>
            {
                if (includeItems)
                {
                    var sql = @"select * from orderinfo where id=@id;select * from orderitem where orderid=@id;";
                    var cmd = SqlBuilder.Raw(sql, new { id }).ToCommand();
                    using (var reader = TradeConn.QueryMultiple(cmd))
                    {
                        var o = reader.Read<Order>().FirstOrDefault();
                        if (o != null)
                        {
                            o.Items = reader.Read<OrderItem>().ToList();
                        }
                        return o;
                    }
                }
                else
                {
                    var cmd = SqlBuilder
                    .Select("*").From("orderinfo")
                    .Where("id=@id", new { id })
                    .ToCommand();
                    return TradeConn.QueryFirstOrDefault<Order>(cmd);
                }
            });
        }

        #endregion

        #region 订单Note
        ///<summary>
        /// 订单Note创建
        ///</summary>
        ///<param name="o">订单Note</param>
        ///<returns></returns>
        public int OrderNoteCreate(OrderNote o)
        {
            return Try(nameof(OrderNoteCreate), () =>
            {
                var cmd = SqlBuilder.Insert("OrderNote")
                .Column("Type", o.Type)
                .Column("OrderId", o.OrderId)
                .Column("Subject", o.Subject)
                .Column("Message", o.Message)
                .Column("CreatedBy", o.CreatedBy)
                .Column("CreatedOn", o.CreatedOn)
                .ToCommand(true);
                return TradeConn.ExecuteScalar<int>(cmd);
            });
        }

        #endregion
    }
}