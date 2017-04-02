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

        /// <summary>
        /// �û���֧��ƽ̨֧����ɺ�֧��ƽ̨���ض��򵽸õ�ַ
        /// </summary>
        /// <param name="platformId">֧��ƽ̨Id</param>
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
                Logger.LogError(1, "{onlinePay} Callback ����ʧ�� ֧����({payId}) ֧��ƽ̨���׺�{payNo} {message}", onlinePay.Name, result.Data.PayId, result.Data.PayNo, result.Message);
            }
            return Redirect("/pay/result/" + result.Data.PayId);//��ת�����ҳ��ͳһ����
        }

        /// <summary>
        /// ֧��ƽ̨�첽֪ͨ
        /// </summary>
        /// <param name="platformId">֧��ƽ̨Id</param>
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
            var s = Request.Query;

            //var model = new ViewModel<Payment>();

            var order = DefaultStorage.OrderGet(id);

            //model.Data = payment;


            return View();
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
            var identity = User;
            if (!identity)
            {
                //δ��¼
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
                            //����û�ѡ���˾�������������Payment���������Ϣ
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
                            return Redirect(url);//��ת��������֧��ƽ̨����֧��
                        }
                        return Content("����֧����ʧ��");

                    }
                    return Content("��������(OnlinePay)");
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
        /// ֧��ƽ̨֪ͨ��֤�ɹ��󴥷�
        /// ���¸��������״̬��
        /// </summary>
        /// <param name="notification">֧��ƽ̨֪ͨ</param>
        /// <returns></returns>
        private bool Notified(Notification notification)
        {
            var now = DateTime.Now;

            var payment = DefaultStorage.PaymentGet(notification.PayId);
            if (payment.Status)
            {
                return true;//�����֧���ɹ� ֱ�ӷ���true  (֧��ƽ̨���ܻ���֪ͨ)
            }

            payment.No = notification.PayNo;//��ȡ֧��ƽ̨���׺�

            var status = DefaultStorage.PaymentStatusUpdate(payment.Id, payment.No, true);//����֧����״̬
            if (status)
            {
                //�첽����֧���ɹ��������߼�
                Task.Factory.StartNew(() =>
                {
                    if (payment.Type == 0) //������
                    {
                        var orderId = payment.RelatedId;//����Ϊ���������

                        //TODO ����һ��֧��������������

                        var order = DefaultStorage.OrderGet(orderId); //��ȡ����
                        if (order != null)
                        {
                            //����Ϊ����֧�� ״̬Ϊ������
                            if (order.PaymentType.IsOnlinePay() && order.Status == OrderStatus.WaitBuyerPay)
                            {
                                var amount = payment.Amount;
                                var paidUp = (payment.Amount + order.Paid) == order.Total;
                                //�Ƿ��Ѹ���(��������������֧�����������¶���״̬Ϊ�Ѹ�������������¶�����Ч��)
                                status = DefaultStorage.OrderPaid(orderId, amount, paidUp); //���¶����Ѹ���״̬

                                if (status) return;

                                Logger.LogWarning(2, "���¶���({orderId})�Ѹ���״̬ʧ��", orderId);

                                var note = new OrderNote();
                                note.Id = 0;
                                note.Type = 0;
                                note.OrderId = orderId;
                                note.Subject = "���¶����Ѹ���״̬ʧ��";
                                note.Message = payment.Id;
                                note.CreatedBy = "";
                                note.CreatedOn = now;

                                status = DefaultStorage.OrderNoteCreate(note) > 0;

                                if (!status)
                                {
                                    Logger.LogError(2, "����OrderNoteʧ�� ֧����({paymentId}) ����({orderId}", payment.Id, orderId);
                                }
                            }
                        }
                        else
                        {
                            Logger.LogWarning(1, "֧����({paymentId})δƥ�䵽����({orderId})", payment.Id, orderId);

                            var note = new PaymentNote();
                            note.Id = 0;
                            note.PayId = payment.Id;
                            note.PayNo = payment.No;
                            note.UserId = payment.UserId;
                            note.Subject = "֧����δƥ�䵽����";
                            note.Message = orderId;
                            note.RawData = "";
                            note.CreatedBy = "";
                            note.CreatedOn = now;

                            status = DefaultStorage.PaymentNoteCreate(note) > 0;

                            if (!status)
                            {
                                Logger.LogError(1, "����PaymentNoteʧ�� ֧����({paymentId}) ����({orderId})", payment.Id, orderId);
                            }
                        }
                    }
                });
            }
            else
            {
                Task.Factory.StartNew(() =>
                {
                    Logger.LogWarning(1, "����֧����({paymentId})״̬ʧ��", payment.Id);

                    var note = new PaymentNote();
                    note.Id = 0;
                    note.PayId = payment.Id;
                    note.PayNo = payment.No;
                    note.UserId = payment.UserId;
                    note.Subject = "����֧����״̬ʧ��";
                    note.Message = "";
                    note.RawData = "";
                    note.ForAdmin = true;
                    note.CreatedBy = "";
                    note.CreatedOn = now;

                    status = DefaultStorage.PaymentNoteCreate(note) > 0;

                    if (!status)
                    {
                        Logger.LogError(1, "����PaymentNoteʧ�� ֧����({paymentId})", payment.Id);
                    }
                });
            }
            return status;
        }
        #endregion
    }
}