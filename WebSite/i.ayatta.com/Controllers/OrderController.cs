using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    [Authorize, Route("order")]
    public class OrderController : BaseController
    {
        public OrderController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<OrderController> logger) : base(defaultStorage, defaultCache, logger)
        {
        }

        [HttpGet("list")]
        public IActionResult OrderList()
        {
            var model = new OrderListModel();
            model.Orders = DefaultStorage.OrderPagedList();

            return View(model);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("detail/{id}")]
        public IActionResult OrderDetail(string id)
        {
            var model = new OrderDetailModel();
            model.Order = DefaultStorage.OrderGet(id, User.Id, false, true);

            return View(model);
        }

        #region ��������
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="id">������</param>
        /// <returns></returns>
        [HttpGet("memo/{id}")]
        public IActionResult OrderMemo(string id)
        {
            var identity = User;
            var model = DefaultStorage.OrderMemoGet(id, identity.Id, false);
            return PartialView(model);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="param">������</param>
        /// <param name="flag">Flag</param>
        /// <param name="memo">����</param>
        /// <returns></returns>
        [HttpPost("memo/{id}")]
        public IActionResult OrderMemo(string id, byte flag, string memo)
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

            result.Status = DefaultStorage.OrderMemoUpdate(id, User.Id, false, flag, memo);

            return Json(result);
        }
        #endregion

        #region ȡ������
        /// <summary>
        /// ȡ������
        /// </summary>
        /// <param name="id">������</param>
        /// <returns></returns>
        [HttpGet("cancel/{id}")]
        public IActionResult OrderCancel(string id)
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
        [HttpPost("cancel/{id}")]
        public IActionResult OrderCancel(string id, string reason)
        {
            var result = new Result();
            if (!User)
            {
                result.Error("δ��¼���¼��ʱ�������µ�¼��");
                return Json(result);
            }
            var status = DefaultStorage.OrderStatusGet(id, User.Id, true);

            if (status == OrderStatus.None || status == OrderStatus.Pending || status == OrderStatus.WaitBuyerPay)
            {
                result.Status = DefaultStorage.OrderCancel(id, User.Id, false, 2, reason);
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
        #endregion

        /// <summary>
        /// ����֧��
        /// </summary>
        /// <param name="id">������</param>
        /// <returns></returns>
        [HttpGet("pay/{id}")]
        public IActionResult OrderPay(string id)
        {
            var status = DefaultStorage.OrderStatusGet(id, User.Id, false);
            if (status != OrderStatus.WaitBuyerPay)
            {
                return Redirect("/order/detail/" + id);
            }

            return Redirect("http://localhost:10015/pay/order/" + id);//��ת��֧��
        }
    }

}


