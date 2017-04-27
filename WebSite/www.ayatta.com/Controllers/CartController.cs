using Ayatta;
using Ayatta.Cart;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    [Route("cart")]
    public class CartController : BaseController
    {
        private readonly Cart.Cart cart;

        public CartController(CartManager cartManager, DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<CartController> logger) : base(defaultStorage, defaultCache, logger)
        {
            var guid = "adfafadf";
            var platform = Platform.Pc;
            cart = cartManager.GetCart(guid, platform);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new CartModel.Index();
            //model.CartData = cart.GetData().Data;
            return View(model);
        }

        /// <summary>
        /// 获取购物车数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("data")]
        public IActionResult Data()
        {
            var data = cart.GetData();
            return Json(data);
        }

        /// <summary>
        /// 商品操作
        /// </summary>
        /// <param name="operate">操作方式</param>        
        /// <param name="itemId">ItemId</param>
        /// <param name="skuId">SkuId</param>
        /// <param name="quantity">数量</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [Route("operate")]
        //[HttpGet,HttpPost]
        public IActionResult Operate(Operate operate, int itemId, int skuId = 0, int quantity = 1, string callback = null)
        {
            var data = cart.ProductOpt(operate, itemId, skuId, quantity);
            if (!string.IsNullOrEmpty(callback))
            {
                return Jsonp(data, callback);
            }
            return Json(data);
        }

        /// <summary>
        /// 清除指定商品
        /// </summary>
        /// <param name="skus">待清除sku</param>
        /// <param name="items">待清除item</param>
        /// <param name="packages">待清除package</param>
        /// <returns></returns>
        [HttpPost("clean")]
        public IActionResult Clean(int[] skus = null, int[] items = null, int[] packages = null)
        {
            var data = cart.Clean(skus, items, packages);
            return Json(data);
        }

        /// <summary>
        /// 清空购物车
        /// </summary>
        /// <returns></returns>
        [HttpPost("clean")]
        public IActionResult Empty()
        {
            var data = cart.Empty();
            return Json(data);
        }


        /// <summary>
        /// 选择
        /// </summary>
        /// <param name="select">选择类型</param>
        /// <param name="param">选择参数值</param>
        /// <param name="selected">是否选中</param>
        /// <returns></returns>
        [HttpPost("select")]
        public IActionResult Select(Select select = Cart.Select.All, int param = 0, bool selected = true)
        {
            var status = cart.Select(select, param, selected);
            return Json(status);
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <returns></returns>

        [HttpGet("confirm/{oversea}")]
        public IActionResult Confirm(bool oversea = false)
        {
            var userId = User.Id;

            var model = new CartModel.Confirm();
            var temp = cart.GetData(oversea);
            if (temp)
            {
                temp.Data.Oversea = oversea;
                model.CartData = temp.Data;
            }
            model.Addresses = DefaultStorage.UserAddressList(userId);

            return View(model);
        }


        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="userAddressId">用户收货地址Id</param>
        /// <returns></returns>
        [HttpPost("confirm")]
        public IActionResult Confirm(int userAddressId = 0)
        {
            if (userAddressId < 1)
            {
                return Content("100");
            }
            if (!User)
            {
                return Content("200");
            }

            var userId = User.Id;

            var userAddress = DefaultStorage.UserAddressGet(userAddressId, User.Id);

            var model = new CartModel.Confirm();

            var temp = cart.SetAddress(userAddress);
            if (!temp)
            {
                return Content("500");
            }
            model.CartData = temp.Data;
            model.Addresses = DefaultStorage.UserAddressList(userId);

            return PartialView(model);
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        [HttpPost("submit")]
        public IActionResult Submit()
        {
            var status = cart.Submit();
            if (status)
            {
                //if (data.PaymentType == PaymentType.Weixin)
                //{
                //    //跳转到收银台进行支付
                //    var url = "/pay/" + status.Data;
                //    return Redirect(url);
                //}
            }
            return Json(status);
        }

    }
}