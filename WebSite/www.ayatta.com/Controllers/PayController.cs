using System;
using System.Linq;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.OnlinePay;
using Ayatta.Web.Models;
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
        [HttpGet("for/{id}")]
        public IActionResult Pay(string id)
        {
            var payment = DefaultStorage.PaymentGet(id);
            return Json(payment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">֧����Id</param>
        /// <param name="platformId">֧��ƽ̨Id</param>
        /// <param name="bankId">֧������Id</param>
        /// <returns></returns>
        [HttpPost("for/{id}")]
        public IActionResult Pay(string id, int platformId = 2, int bankId = 0)
        {
            var payment = DefaultStorage.PaymentGet(id);
            var onlinePay = GetOnlinePay(platformId);
            if (onlinePay != null)
            {
                payment.Amount = 0.01m;//����
                var url = onlinePay.Pay(payment);
                if (platformId == 3)
                {
                    //RenderQRCode(url);
                }
                return Redirect(url);
            }
            return View();
        }
        */

        /// <summary>
        /// �û���֧��ƽ̨֧����ɺ�֧��ƽ̨���ض��򵽸õ�ַ
        /// </summary>
        /// <param name="platformId">֧��ƽ̨Id</param>
        /// <returns></returns>
        [HttpGet("callback/{platformId}")]
        public IActionResult Callback(int platformId = 2)
        {
            var onlinePay = GetOnlinePay(platformId);
            if (onlinePay == null) return StatusCode(404);
            var param = new PaymentParam(Request.Query);
            var result = onlinePay.HandleNotify(param);
            if (!result)
            {
                Logger.LogError(1, "{onlinePay} Callback ����ʧ�� ֧����({payId}) ֧��ƽ̨���׺�{payNo} {message}", onlinePay.Name, result.Data.PayId, result.Data.PayNo, result.Message);
            }
            return Redirect("/pay/result/" + result.Data.PayId);//��ת�����ҳ��ͳһ����
        }

        /// <summary>
        /// ֧��ƽ̨�첽֪ͨ
        /// </summary>
        /// <param name="platformId">֧��ƽ̨Id</param>
        /// <returns></returns>
        [Route("notify/{platformId}")]
        public IActionResult Notify(int platformId = 2)
        {
            var onlinePay = GetOnlinePay(platformId);
            if (onlinePay == null) return StatusCode(404);
            var param = new PaymentParam(Request.Query);
            var result = onlinePay.HandleNotify(param);
            if (!result)
            {
                Logger.LogError(1, "{onlinePay} Notify ����ʧ�� ֧����({payId}) ֧��ƽ̨���׺�{payNo} {message}", onlinePay.Name, result.Data.PayId, result.Data.PayNo, result.Message);
            }
            return Content(result.Data.Output);
        }

        /// <summary>
        /// ֧�����
        /// </summary>
        /// <param name="id">֧������</param>
        /// <returns></returns>
        [HttpGet("result/{id}")]
        public ActionResult Result(string id)
        {
            return View();
        }

        #region ����֧��
        /// <summary>
        /// ��֧������
        /// </summary>
        /// <param name="id">����Id</param>
        /// <returns></returns>
        [HttpGet("order/{id}")]
        public IActionResult Order(string id)
        {
            var identity = User;
            if (!identity)
            {
                return Content("��¼");
            }

            var status = DefaultStorage.OrderStatusGet(id, identity.Id, false);
            if (status != OrderStatus.WaitBuyerPay)
            {
                return Redirect("http://localhost:39272/order/detail/" + id);
            }
            var model = new PayOrder();
            model.Account = new Account();
            var order = DefaultStorage.OrderGet(id, identity.Id, false, true);

            if (order.PaymentType.IsOnlinePay() && order.Status == OrderStatus.WaitBuyerPay && order.Paid < order.Total)
            {
                var platforms = PaymentPlatformList();
                //var tempEBanks = PaymentEBankList();
                //var eBanks = new List<PaymentEBank>();

                //foreach (var platform in platforms)
                //{
                //    var p = platform;
                //    var x = eBanks.Select(o => o.BankId);
                //    var temp = tempEBanks.Where(o => o.PlatformId == p.Id && o.Status && !x.Contains(o.BankId));
                //    eBanks.AddRange(temp);
                //}


                model.Order = order;
                model.Platforms = platforms;
                //model.Banks = eBanks;
                return View(model);
            }
            return Redirect("http://localhost:39272/order/detail/" + id);
        }

        /// <summary>
        /// ��֧������
        /// </summary>
        /// <param name="id">����Id</param>
        /// <param name="platformId">֧��ƽ̨Id</param>
        /// <param name="bankId">֧������Id</param>
        /// <returns></returns>
        [HttpPost("order/{id}")]
        public IActionResult Order(string id, int platformId, int bankId = 0)
        {
            //�ɴ������ύ֧�����
            var identity = User;
            if (!identity)
            {
                //δ��¼
            }
            //�ٴμ�鶩��״̬
            var status = DefaultStorage.OrderStatusGet(id, identity.Id, false);
            if (status != OrderStatus.WaitBuyerPay)
            {
                return Redirect("http://localhost:39272/order/detail/" + id);
            }

            var order = DefaultStorage.OrderMiniGet(id, identity.Id);

            if (order.PaymentType.IsOnlinePay() && order.Status == OrderStatus.WaitBuyerPay && order.Paid < order.Total)
            {
                var onlinePay = GetOnlinePay(platformId);
                if (onlinePay != null)
                {
                    var ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    var payment = order.ToPayment(platformId, ipAddress);//֧���� ΢����ip
                    if (bankId > 0)
                    {
                        var paymentBanks = DefaultStorage.PaymentBankList(platformId);
                        var paymentBank = paymentBanks.FirstOrDefault(o => o.Id == bankId);
                        if (paymentBank != null)
                        {
                            //����û�ѡ���˾�������������Payment���������Ϣ
                            payment.BankId = paymentBank.Id;
                            payment.BankCode = paymentBank.Code;
                            payment.BankName = paymentBank.Bank.Name;
                        }
                    }

                    var created = DefaultStorage.PaymentCreate(payment);
                    if (created)
                    {
                        var url = onlinePay.Pay(payment);
                        if (string.IsNullOrEmpty(url))
                        {
                            return Content("����֧��ƽ̨��Ϣʧ��");
                        }
                        if (platformId == 3)
                        {
                            //RenderQRCode(url);
                        }
                        return Redirect(url);//��ת��������֧��ƽ̨����֧��
                    }
                    return Content("����֧����Ϣʧ��");
                }
                return Content("��������(OnlinePay)");
            }

            return Redirect("http://i.ayatta.com/order/detail/" + order.Id);
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
        /// ֧��ƽ̨֪ͨ��֤�ɹ��󴥷�
        /// ���¸��������״̬��
        /// </summary>
        /// <param name="notification">֧��ƽ̨֪ͨ</param>
        /// <returns></returns>
        private bool Notified(Notification notification)
        {
            try
            {
                var now = DateTime.Now;
                var payment = DefaultStorage.PaymentGet(notification.PayId);
                if (payment == null)
                {
                    Task.Factory.StartNew(() =>
                    {
                        Logger.LogWarning(1, "֧����({0})δ�ҵ�", notification.PayId);

                        var note = new PaymentNote();
                        note.PayId = notification.PayId;
                        note.PayNo = notification.PayNo;
                        note.UserId = 0;
                        note.Subject = "֧����δ�ҵ�";
                        note.Message = string.Empty;
                        note.RawData = notification.Input;
                        note.Extra = string.Empty;
                        note.CreatedBy = "sys";
                        note.CreatedOn = now;

                        PaymentNoteCreate(note);
                    });
                    return false;
                }
                if (payment.Status)
                {
                    return true;//�����֧���ɹ� ֱ�ӷ���true  (֧��ƽ̨���ܻ���֪ͨ)
                }

                var status = DefaultStorage.PaymentStatusUpdate(notification.PayId, notification.PayNo, true);//����֧����״̬

                if (status)
                {
                    //�첽����֧���ɹ��������߼�
                    Task.Factory.StartNew(() => Async(payment));
                }
                else
                {
                    Task.Factory.StartNew(() =>
                    {
                        Logger.LogWarning(1, "����֧����({0})״̬ʧ��", notification.PayId);

                        var note = new PaymentNote();
                        note.PayId = notification.PayId;
                        note.PayNo = notification.PayNo;
                        note.UserId = payment.UserId;
                        note.Subject = "����֧����״̬ʧ��";
                        note.Message = string.Empty;
                        note.RawData = notification.Input;
                        note.Extra = string.Empty;
                        note.CreatedBy = "sys";
                        note.CreatedOn = now;

                        PaymentNoteCreate(note);
                    });
                }

                return status;
            }
            catch (Exception e)
            {
                Logger.LogError(1, e, "����֧��ƽ̨֪ͨ�����쳣");

                return false;
            }
        }

        private void PaymentNoteCreate(PaymentNote note)
        {
            var status = DefaultStorage.PaymentNoteCreate(note) > 0;

            if (!status)
            {
                Logger.LogError(1, "����PaymentNoteʧ�� ֧����({0})", note.PayId);
            }
        }

        /// <summary>
        /// ������ ֧�ֶ��֧��/�ϲ�֧��
        /// </summary>
        /// <param name="payment"></param>
        private void Async(Payment payment)
        {
            var now = DateTime.Now;

            if (payment.Status && payment.Type == 0)
            {
                var fail = 0m;//����ʧ��
                var remain = payment.Amount;//������ϲ�֧���ܽ��

                var oids = payment.RelatedId.Split(',');//����Ϊ���������
                foreach (var oid in oids)
                {
                    if (remain <= 0)
                    {
                        break;
                    }
                    var order = DefaultStorage.OrderMiniGet(oid, payment.UserId); //��ȡ����
                    if (order != null)
                    {
                        //����Ϊ����֧�� ״̬Ϊ������
                        if (order.PaymentType.IsOnlinePay() && order.Status == OrderStatus.WaitBuyerPay)
                        {
                            var status = false;//���¶������
                            var amount = order.Unpaid;//��֧�����
                            if (remain - amount >= 0)
                            {
                                //����(��������������֧�����������¶���״̬Ϊ�Ѹ�������������¶�����Ч��)
                                status = DefaultStorage.OrderPaid(oid, amount, true); //���¶����Ѹ���״̬
                                if (status)
                                {
                                    remain -= amount;
                                }
                                else
                                {
                                    fail += amount;
                                }
                            }
                            else
                            {
                                //ʣ����� ����֧��
                                status = DefaultStorage.OrderPaid(oid, remain, false);
                                if (status)
                                {
                                    remain = 0;
                                }
                                else
                                {
                                    fail += remain;
                                }
                            }

                            if (!status)
                            {
                                Logger.LogWarning(2, "���¶���({orderId})�Ѹ���״̬ʧ��", oid);

                                var note = new OrderNote();
                                note.Type = 1;
                                note.UserId = payment.UserId;
                                note.OrderId = oid;
                                note.Subject = "���¶����Ѹ����/״̬ʧ��";
                                note.Message = string.Format("֧����({0})���¶���({1})�Ѹ����/״̬ʧ��", payment.Id, oid);
                                note.Extra = (remain - amount >= 0 ? amount : remain).ToString("F2");
                                note.CreatedBy = "sys";
                                note.CreatedOn = now;

                                if (DefaultStorage.OrderNoteCreate(note) == 0)
                                {
                                    Logger.LogError(2, "����OrderNoteʧ�� ֧����({paymentId}) ����({orderId}", payment.Id, oid);
                                }

                            }

                        }
                    }
                    else
                    {
                        Logger.LogWarning(1, "֧����({paymentId})δƥ�䵽����({orderId})", payment.Id, oid);

                        var note = new PaymentNote();
                        note.PayId = payment.Id;
                        note.PayNo = payment.No;
                        note.UserId = payment.UserId;
                        note.Subject = "֧����δƥ�䵽����";
                        note.Message = string.Format("֧����({0})δƥ�䵽����({1})", payment.Id, oid);
                        note.RawData = string.Empty;
                        note.Extra = oid;
                        note.CreatedBy = "sys";
                        note.CreatedOn = now;

                        if (DefaultStorage.PaymentNoteCreate(note) == 0)
                        {
                            Logger.LogError(1, "����PaymentNoteʧ�� ֧����({paymentId}) ����({orderId})", payment.Id, oid);
                        }
                    }
                }

                if (remain - fail > 0)
                {
                    //�û���֧��ƽ̨֧���ɹ� ��δ�ܼ�ʱ�յ�֪ͨ��ɹ����� ����û����֧�� ����û���֧���Ľ�� �˵��û�Ǯ��


                }
            }
        }

        #endregion
    }
}