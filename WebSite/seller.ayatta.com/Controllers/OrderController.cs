using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    public class OrderController : BaseController
    {
        public OrderController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<OrderController> logger) : base(defaultStorage, defaultCache, logger)
        {
        }

        [HttpGet("/order-list")]
        public IActionResult OrderList()
        {
            var model = new OrderListModel();
            model.Orders = DefaultStorage.OrderPagedList();

            return View(model);
        }

        [HttpGet("/order-detail/{id}")]
        public IActionResult OrderDetail(string id)
        {
            var model = new OrderDetailModel();
            model.Order = DefaultStorage.OrderGet(id, User.Id, true, true);

            return View(model);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="id">������</param>
        /// <returns></returns>
        [HttpGet("/order-memo/{id}")]
        public ActionResult OrderMemo(string id)
        {
            var identity = User;
            var model = DefaultStorage.OrderMemoGet(id, identity.Id, true);
            return PartialView(model);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="param">������</param>
        /// <param name="flag">Flag</param>
        /// <param name="memo">����</param>
        /// <returns></returns>
        [HttpPost("/order-memo/{id}")]
        public ActionResult OrderMemo(string id, byte flag, string memo)
        {
            var result = new Result();
            if (!User)
            {
                result.Error("δ��¼���¼��ʱ�������µ�¼��");
                return Json(result);
            }
            if (memo.Length > 100)
            {
                result.Message = "�������ܳ���100���֣�";
                return Json(result);
            }

            result.Status = DefaultStorage.OrderMemoUpdate(id, User.Id, true, flag, memo);

            return Json(result);
        }

        /// <summary>
        /// ȡ������
        /// </summary>
        /// <param name="id">������</param>
        /// <returns></returns>
        [HttpGet("/order-cancel/{id}")]
        public ActionResult OrderCancel(string id)
        {
            var model = new Result<OrderStatus>();

            var status = DefaultStorage.OrderStatusGet(id, User.Id, true);

            model.Data = status;
            model.Status = true;

            return PartialView(model);
        }

        /// <summary>
        /// ȡ������
        /// </summary>
        /// <param name="id">������</param>
        /// <param name="reason">ȡ��ԭ��</param>
        /// <returns></returns>
        [HttpPost("/order-cancel/{id}")]
        public ActionResult OrderCancel(string id, string reason)
        {
            var result = new Result();
            if (!User)
            {
                result.Error("δ��¼���¼��ʱ�������µ�¼��");
                return Json(result);
            }
            var status = DefaultStorage.OrderStatusGet(id, User.Id, true);

            if (status == OrderStatus.Pending || status == OrderStatus.WaitBuyerPay)
            {
                result.Status = DefaultStorage.OrderCancel(id, User.Id,true, 3, reason);
                if (result.Status)
                {
                    result.Status = true;
                }
                else
                {
                    result.Message = "ȡ������ʧ�ܣ�";
                }
            }
            else
            {
                result.Message = "�ö�����ǰ״̬����ȡ����";
            }
            return Json(result);
        }
    }

}


