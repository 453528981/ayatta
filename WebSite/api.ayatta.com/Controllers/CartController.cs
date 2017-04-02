using Ayatta.Api;
using Ayatta.Cart;
using System.Linq;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.OnlinePay;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    /// <summary>
    /// 购物车
    /// </summary>
    public class CartController : BaseController
    {
        private readonly CartManager cartManager;

        private readonly Plateform plateform = Plateform.App;
        public CartController(CartManager cartManager, DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<CartController> logger) : base(defaultStorage, defaultCache, logger)
        {
            this.cartManager = cartManager;
        }

        #region 获取
        /// <summary>
        /// 购物车获取
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost("cart-get")]
        public CartGetResponse CartGet([FromBody]CartGetRequest req)
        {

            var cart = cartManager.GetCart(req.Guid, plateform);

            var rep = new CartGetResponse();

            var temp = cart.GetData();
            if (temp)
            {
                rep.Data = temp.Data;
            }

            return rep;
        }
        #endregion

        #region 操作
        /// <summary>
        /// 购物车商品操作
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost("cart-opt")]
        public CartOptResponse CartOpt([FromBody]CartOptRequest req)
        {
            var cart = cartManager.GetCart(req.Guid, plateform);

            var rep = new CartOptResponse();

            var temp = cart.ProductOpt(req.Opt, req.ItemId, req.SkuId, req.Quantity);
            if (temp)
            {
                rep.Data = temp.Data;
            }
            else
            {
                rep.Error(temp.Message);
            }

            return rep;
        }
        #endregion

        #region 选择
        /// <summary>
        /// 购物车商品选择
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost("cart-select")]
        public CartSelectResponse CartSelect([FromBody]CartSelectRequest req)
        {
            var cart = cartManager.GetCart(req.Guid, plateform);

            var rep = new CartSelectResponse();

            var temp = cart.Select(req.Select, req.Param, req.Selected);
            if (temp)
            {
                rep.Data = temp.Data;
            }
            else
            {
                rep.Error(temp.Message);
            }
            return rep;
        }

        #endregion

        #region 清除
        /// <summary>
        /// 购物车商品清除
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost("cart-clean")]
        public CartCleanResponse CartClean([FromBody]CartCleanRequest req)
        {
            var cart = cartManager.GetCart(req.Guid, plateform);

            var rep = new CartCleanResponse();
            if (req.All)
            {
                var status = cart.Empty();
                if (!status)
                {
                    rep.Error("清空购物车失败");
                }
                return rep;
            }
            var temp = cart.Clean(req.Skus, req.Items, req.Packages);
            if (temp)
            {
                rep.Data = temp.Data;
            }
            else
            {
                rep.Error(temp.Message);
            }
            return rep;
        }

        #endregion

        #region 确认
        /// <summary>
        /// 购物车确认
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost("cart-confirm")]
        public CartConfirmResponse CartConfirm([FromBody]CartConfirmRequest req)
        {
            var cart = cartManager.GetCart(req.Guid, plateform);

            var rep = new CartConfirmResponse();

            //var temp = cart.Confirm(req.Skus, req.Items, req.Packages, req.AddressId);
            //if (temp)
            //{
            //    rep.Data = temp.Data;
            //}
            //else
            //{
            //    rep.Error(temp.Message);
            //}

            return rep;
        }

        #endregion

        #region 提交
        /// <summary>
        /// 购物车提交
        /// </summary>
        /// <param name="req">请求参数</param>
        /// <returns></returns>
        [HttpPost("cart-submit")]
        public CartSubmitResponse CartSubmit([FromBody]CartSubmitRequest req)
        {
            var cart = cartManager.GetCart(req.Guid, plateform);

            var rep = new CartSubmitResponse();

            var param = new SubmitParam();
            param.Skus = req.Skus;
            param.Items = req.Items;
            param.Packages = req.Packages;
            param.UserAddressId = req.UserAddressId;

            var temp = cart.Submit();
            if (temp)
            {
                var payId = temp.Data;
                var platformId = 2;
                /*
                var onlinePay = GetOnlinePay(platformId);
                if (onlinePay != null)
                {
                    var payment = DefaultStorage.PaymentGet(payId);
                    //rep.Data = onlinePay.Pay(payment);
                }
                */
            }
            else
            {
                rep.Error("提交订单失败");
                return rep;
            }          

            return rep;
        }

        #endregion

        [HttpGet("test")]
        public string Test()
        {
            var onlinePay = GetOnlinePay(2);

            if (onlinePay != null)
            {
                var payment = DefaultStorage.PaymentGet("1607261810160001");
                return onlinePay.Pay(payment);
            }
            return "";
        }


        protected IOnlinePay GetOnlinePay(int platformId)
        {
            var platforms = DefaultStorage.PaymentPlatformList();
            var platform = platforms.FirstOrDefault(x => x.Id == platformId);

            if (platform == null) return null;
            var key = "MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBAK8sNAUi6eFbAD1oqM3IIV9OKcbZchkr2jTZx5SZMW5i5Bp6HYCI3+HpRF2bKGk3XkXNnKr+uzcEFkG/xgtPP2vXf4QZxodBU9ceg5pRuY0qgZ6DUP/vE4kai9clJjWn0WOn6bNDOomnGTkWt2rGHGdzMFN9vuyEMzkB2M0Cwai1AgMBAAECgYAp3Nh5ucRG9OZzxoVA2GkRS660NNcNqOs24izOGGY1yTBWG4TdaNiINqT98cyQiIjhCag9PS8kkLd48wmzPjcbewke2GNYPTm7sDDXWrD7UiJ3LJXf1JFEasZq7BAXSxm8Tr9/e49lRbTj17PsbV8zadS/xrYevc9YiH2MJSxI6QJBAOaZ29VfSJzz2vaz7e9oQnAgUJHRyjH8A2Low6BD0cu7+xk2tXlIZ51sedIkiui+tHqgPerrB3XCn/ytfnYVXvsCQQDCd3WSdx8/8yz/GlA8pwoD3z1q3CR4hZNQ4HlgBoYROMGyXifPyHi3GICwOkflzmdGfpnp8yfo6NxdkC36V8gPAkAnb9iwvQLmFK410r+2WdZC5sPgrEgwFDFgEP6jwfV3KkbfIQQYIdHWkl6jGazH8RVcg1sTee5krUw/IkymVTFRAkA+AdtSbXtgZ8jEOv60qEqQO4GY7kMOzwDPPBRXoxzipudUWaN2JKUhNMXr61l7lFnn53xqVac3I/EIQG34sj7PAkEAmWb51gOvzRsf0tDBf8lerYxEuiGbLnEAv8HfR85z61FwGSoxC1CCuscqU9DJ6lSjQn+qpU6/oFxKIDuV24+b8w==";
            // var key = "MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBAK8sNAUi6eFbAD1oqM3IIV9OKcbZchkr2jTZx5SZMW5i5Bp6HYCI3+HpRF2bKGk3XkXNnKr+uzcEFkG/xgtPP2vXf4QZxodBU9ceg5pRuY0qgZ6DUP/vE4kai9clJjWn0WOn6bNDOomnGTkWt2rGHGdzMFN9vuyEMzkB2M0Cwai1AgMBAAECgYAp3Nh5ucRG9OZzxoVA2GkRS660NNcNqOs24izOGGY1yTBWG4TdaNiINqT98cyQiIjhCag9PS8kkLd48wmzPjcbewke2GNYPTm7sDDXWrD7UiJ3LJXf1JFEasZq7BAXSxm8Tr9/e49lRbTj17PsbV8zadS/xrYevc9YiH2MJSxI6QJBAOaZ29VfSJzz2vaz7e9oQnAgUJHRyjH8A2Low6BD0cu7+xk2tXlIZ51sedIkiui+tHqgPerrB3XCn/ytfnYVXvsCQQDCd3WSdx8/8yz/GlA8pwoD3z1q3CR4hZNQ4HlgBoYROMGyXifPyHi3GICwOkflzmdGfpnp8yfo6NxdkC36V8gPAkAnb9iwvQLmFK410r+2WdZC5sPgrEgwFDFgEP6jwfV3KkbfIQQYIdHWkl6jGazH8RVcg1sTee5krUw/IkymVTFRAkA+AdtSbXtgZ8jEOv60qEqQO4GY7kMOzwDPPBRXoxzipudUWaN2JKUhNMXr61l7lFnn53xqVac3I/EIQG34sj7PAkEAmWb51gOvzRsf0tDBf8lerYxEuiGbLnEAv8HfR85z61FwGSoxC1CCuscqU9DJ6lSjQn+qpU6/oFxKIDuV24+b8w==";
            //var pubkey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCQ1AmVDaf5ZlcAk3p1fnHR8f5NfdzchAKeweZR kR/YxKAS6sT99h0Petjw1ASjmI6dpxdax4b/2GFwVldLqpkPXanWHTItx8JQXsXyqnt7eaaoBVR ZwwXpt80M9Ar/ffZF2ONfF/HAqS5pg/5NXRNckWabsGUpWdz/TZ2sufDdQIDAQAB";

            platform.PrivateKey = key;
            return OnlinePayFactory.Create(platform);
        }

    }
}