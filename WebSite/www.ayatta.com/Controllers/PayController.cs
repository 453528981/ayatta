using System;
using System.Linq;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.OnlinePay;
using Ayatta.Web.Extensions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    [Route("pay")]
    public class PayController : BaseController
    {
        public PayController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<PayController> logger) : base(defaultStorage, defaultCache, logger)
        {
        }
        /*
        [HttpGet("{id}")]
        public IActionResult Pay(string id)
        {
            var payment = DefaultStorage.PaymentGet(id);
            return Json(payment);
        }
        */
        [HttpGet("for/{id}")]
        public IActionResult Pay(string id, int platformId = 2)
        {
            var payment = DefaultStorage.PaymentGet(id);
            var onlinePay = GetOnlinePay(platformId);
            if (onlinePay != null)
            {
                payment.Amount = 0.01m;//测试
                var url = onlinePay.Pay(payment);
                if (platformId == 3)
                {
                    //RenderQRCode(url);
                }
                return Redirect(url);
            }
            return View();
        }

        /// <summary>
        /// 用户在支付平台支付完成后支付平台会重定向到该地址
        /// </summary>
        /// <param name="platformId">支付平台Id</param>
        /// <returns></returns>
        [HttpGet("/callback/{platformId}")]
        public IActionResult Callback(int platformId = 2)
        {
            var onlinePay = GetOnlinePay(platformId);
            if (onlinePay == null) return StatusCode(404);
            var param = new PaymentParam(Request.Query);
            var result = onlinePay.HandleNotify(param);
            if (!result)
            {
                Logger.LogError(1, "{onlinePay} Callback 处理失败 支付单({payId}) 支付平台交易号{payNo} {message}", onlinePay.Name, result.Data.PayId, result.Data.PayNo, result.Message);
            }
            return Redirect("/pay/result/" + result.Data.PayId);//跳转到结果页面统一处理
        }

        /// <summary>
        /// 支付平台异步通知
        /// </summary>
        /// <param name="platformId">支付平台Id</param>
        /// <returns></returns>
        [Route("/notify/{platformId}")]
        public IActionResult Notify(int platformId = 2)
        {
            var onlinePay = GetOnlinePay(platformId);
            if (onlinePay == null) return StatusCode(404);
            var param = new PaymentParam(Request.Query);
            var result = onlinePay.HandleNotify(param);
            if (!result)
            {
                Logger.LogError(1, "{onlinePay} Notify 处理失败 支付单({payId}) 支付平台交易号{payNo} {message}", onlinePay.Name, result.Data.PayId, result.Data.PayNo, result.Message);
            }
            return Content(result.Data.Output);
        }

        /// <summary>
        /// 支付结果
        /// </summary>
        /// <param name="id">支付单号</param>
        /// <returns></returns>
        [HttpGet("result/{id}")]
        public ActionResult Result(string id)
        {
            return View();
        }

        #region 订单支付
        /// <summary>
        /// 待支付订单
        /// </summary>
        /// <param name="id">订单Id</param>
        /// <returns></returns>
        [HttpGet("order/{id}")]
        public IActionResult Order(string id)
        {
            var s = Request.Query;

            //var model = new ViewModel<Payment>();

            var order = DefaultStorage.OrderGet(id);

            //model.Data = payment;


            return View();
        }

        /// <summary>
        /// 待支付订单
        /// </summary>
        /// <param name="id">订单Id</param>
        /// <param name="platformId">支付平台Id</param>
        /// <param name="bankId">支付银行Id</param>
        /// <returns></returns>
        [HttpPost("order/{id}")]
        public IActionResult Order(string id, int platformId, int bankId = 0)
        {
            var identity = User;
            if (!identity)
            {
                //未登录
            }

            var now = DateTime.Now;
            var userId = identity.Id;

            var order = DefaultStorage.OrderGet(id);

            if (order != null)
            {
                if (order.PaymentType.IsOnlinePay() && order.Status == OrderStatus.WaitBuyerPay)
                {
                    var onlinePay = GetOnlinePay(platformId);
                    if (onlinePay != null)
                    {
                        var payment = new Payment();

                        payment.Id = Payment.NewId();

                        payment.No = string.Empty;
                        payment.Type = 0;
                        payment.UserId = userId;
                        payment.Method = 1;
                        payment.Amount = 0.01m;// order.Unpaid;
                        payment.Subject = "test";
                        payment.Message = "";
                        payment.RawData = "";
                        payment.BankId = 0;
                        payment.BankCode = string.Empty;
                        payment.BankName = string.Empty;
                        payment.BankCard = 0;
                        payment.PlatformId = platformId;
                        payment.CardNo = string.Empty;
                        payment.CardPwd = string.Empty;
                        payment.CardAmount = 0;
                        payment.RelatedId = order.Id;
                        payment.IpAddress = "106.2.161.2";
                        payment.Extra = "";
                        payment.Status = false;
                        payment.CreatedBy = "";
                        payment.CreatedOn = now;
                        payment.ModifiedBy = "";
                        payment.ModifiedOn = now;

                        var paymentBanks = DefaultStorage.PaymentBankList(platformId);
                        var paymentBank = paymentBanks.FirstOrDefault(o => o.Id == bankId);
                        if (paymentBank != null)
                        {
                            //如果用户选择了具体的银行则填充Payment相关银行信息
                            payment.BankId = paymentBank.Id;
                            payment.BankCode = paymentBank.Code;
                            payment.BankName = paymentBank.Bank.Name;
                        }
                        var status = DefaultStorage.PaymentCreate(payment);
                        if (status)
                        {
                            var url = onlinePay.Pay(payment);
                            if (platformId == 3)
                            {
                                //RenderQRCode(url);
                            }
                            return Redirect(url);//跳转到第三方支付平台进行支付
                        }
                        return Content("创建支付单失败");

                    }
                    return Content("参数错误(OnlinePay)");
                }
                return Redirect("http://my.ayatta.com/order/" + order.Id);
            }
            return NotFound();
        }


        #endregion

        #region protected
        protected IOnlinePay GetOnlinePay(int platformId)
        {
            var platforms = DefaultStorage.PaymentPlatformList();
            var platform = platforms.FirstOrDefault(x => x.Id == platformId);

            if (platform == null) return null;

            var onlinePay = OnlinePayFactory.Create(platform);
            if (onlinePay != null)
            {
                onlinePay.Notified += Notified;
            }
            return onlinePay;
        }

        /// <summary>
        /// 支付平台通知验证成功后触发
        /// 更新付款单及订单状态等
        /// </summary>
        /// <param name="notification">支付平台通知</param>
        /// <returns></returns>
        private bool Notified(Notification notification)
        {
            var now = DateTime.Now;

            var payment = DefaultStorage.PaymentGet(notification.PayId);
            if (payment.Status)
            {
                return true;//付款单已支付成功 直接返回true  (支付平台可能会多次通知)
            }

            payment.No = notification.PayNo;//获取支付平台交易号

            var status = DefaultStorage.PaymentStatusUpdate(payment.Id, payment.No, true);//更新支付单状态
            if (status)
            {
                //异步处理支付成功后的相关逻辑
                Task.Factory.StartNew(() =>
                {
                    if (payment.Type == 0) //处理订单
                    {
                        var orderId = payment.RelatedId;//可能为多个订单号

                        //TODO 处理一次支付多个订单的情况

                        var order = DefaultStorage.OrderGet(orderId); //获取订单
                        if (order != null)
                        {
                            //订单为在线支付 状态为待付款
                            if (order.PaymentType.IsOnlinePay() && order.Status == OrderStatus.WaitBuyerPay)
                            {
                                var amount = payment.Amount;
                                var paidUp = (payment.Amount + order.Paid) == order.Total;
                                //是否已付清(付清后如果是在线支付订单则会更新订单状态为已付款待发货并更新订单有效期)
                                status = DefaultStorage.OrderPaid(orderId, amount, paidUp); //更新订单已付金额及状态

                                if (status) return;

                                Logger.LogWarning(2, "更新订单({orderId})已付金额及状态失败", orderId);

                                var note = new OrderNote();
                                note.Id = 0;
                                note.Type = 0;
                                note.OrderId = orderId;
                                note.Subject = "更新订单已付金额及状态失败";
                                note.Message = payment.Id;
                                note.CreatedBy = "";
                                note.CreatedOn = now;

                                status = DefaultStorage.OrderNoteCreate(note) > 0;

                                if (!status)
                                {
                                    Logger.LogError(2, "生成OrderNote失败 支付单({paymentId}) 订单({orderId}", payment.Id, orderId);
                                }
                            }
                        }
                        else
                        {
                            Logger.LogWarning(1, "支付单({paymentId})未匹配到订单({orderId})", payment.Id, orderId);

                            var note = new PaymentNote();
                            note.Id = 0;
                            note.PayId = payment.Id;
                            note.PayNo = payment.No;
                            note.UserId = payment.UserId;
                            note.Subject = "支付单未匹配到订单";
                            note.Message = orderId;
                            note.RawData = "";
                            note.CreatedBy = "";
                            note.CreatedOn = now;

                            status = DefaultStorage.PaymentNoteCreate(note) > 0;

                            if (!status)
                            {
                                Logger.LogError(1, "生成PaymentNote失败 支付单({paymentId}) 订单({orderId})", payment.Id, orderId);
                            }
                        }
                    }
                });
            }
            else
            {
                Task.Factory.StartNew(() =>
                {
                    Logger.LogWarning(1, "更新支付单({paymentId})状态失败", payment.Id);

                    var note = new PaymentNote();
                    note.Id = 0;
                    note.PayId = payment.Id;
                    note.PayNo = payment.No;
                    note.UserId = payment.UserId;
                    note.Subject = "更新支付单状态失败";
                    note.Message = "";
                    note.RawData = "";
                    note.ForAdmin = true;
                    note.CreatedBy = "";
                    note.CreatedOn = now;

                    status = DefaultStorage.PaymentNoteCreate(note) > 0;

                    if (!status)
                    {
                        Logger.LogError(1, "生成PaymentNote失败 支付单({paymentId})", payment.Id);
                    }
                });
            }
            return status;
        }
        #endregion
    }
}