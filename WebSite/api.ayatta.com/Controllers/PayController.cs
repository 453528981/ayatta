using System;
using Ayatta.Api;
using Ayatta.Sms;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.Extension;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    public class PayController : BaseController
    {

        public PayController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<PayController> logger) : base(defaultStorage, defaultCache, logger)
        {

        }

        [HttpPost("pay-alipay")]
        public AlipayResponse Alipay([FromBody]AlipayRequest req)
        {
            var rep = new AlipayResponse();

            if (string.IsNullOrEmpty(req.PayId))
            {
                rep.Error("PayId����Ϊ��");
                return rep;
            }
            var p = DefaultStorage.PaymentGet(req.PayId);
            if (p == null)
            {
                rep.Error("PayIdֵ��Ч");
                return rep;
            }
            if (p.Status)
            {
                rep.Error("��֧����");
                return rep;
            }

            var op = GetOnlinePay(4);
            if (op == null)
            {
                rep.Error("��Ч");
                return rep;
            }
            rep.Code = 0;
            rep.Data = op.Pay(p);
            return rep;
        }
    }
}