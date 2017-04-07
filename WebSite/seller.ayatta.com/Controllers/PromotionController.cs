using System;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq;

namespace Ayatta.Web.Controllers
{
    [Route("promotion")]
    public class PromotionController : BaseController
    {
        public PromotionController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<PromotionController> logger) : base(defaultStorage, defaultCache, logger)
        {
        }

        [HttpGet("activity-list")]
        public ActionResult ActityList(int page = 1, int size = 30, bool global = false, string name = null, DateTime? startedOn = null, DateTime? stoppedOn = null, byte? status = null)
        {
            var now = DateTime.Now;
            var model = new PromotionModel.ActityList();
            model.Name = name;
            model.Global = global;
            model.StartedOn = startedOn.ToString("yyyy-MM-dd HH:mm:ss", string.Empty);
            model.StoppedOn = stoppedOn.ToString("yyyy-MM-dd HH:mm:ss", string.Empty);

            model.Promotions = DefaultStorage.PromotionActivityPagedList(page, size, User.Id);
            return View(model);
        }


        [HttpGet("activity-detail/{id}")]
        public ActionResult ActityDetail(int id = 0, bool global = false)
        {
            var now = DateTime.Now;

            var model = new PromotionModel.ActityDetail();

            model.Global = global;
            model.Promotion = new Promotion.Activity();
            model.Promotion.StartedOn = now.AddDays(2);
            model.Promotion.StoppedOn = now.AddDays(20);

            if (id > 0)
            {
                var promotion = DefaultStorage.PromotionActivityGet(id, true);
                model.Promotion = promotion;
                model.Global = promotion.Global;
            }

            return View(model);
        }


        [HttpPost("activity-detail/{id}")]
        public ActionResult Default(int id, Promotion.Activity model, Promotion.Activity.Rule[] rules)
        {

            var now = DateTime.Now;

            var result = new Result();
            if (id < 0)
            {
                result.Message = "参数不正确";
                return Json(result);
            }

            if (rules == null || rules.Length < 1)
            {
                result.Message = "请设置优惠规则";
                return Json(result);
            }

            var promotion = DefaultStorage.PromotionActivityGet(id);
            if (promotion == null)
            {
                result.Message = "数据不存在";
                return Json(result);

            }
            rules = rules.Where(x => x.Threshold > 0).OrderByDescending(x => x.Threshold).ToArray();

            for (var i = 0; i < rules.Length; i++)
            {
                var urle = rules[i];

                if (i > 0)
                {
                    var pre = rules[i - 1];
                    if (model.Type == 0 && urle.Discount > 0 && urle.Discount < pre.Discount)//保证当前层级优惠比上一级大
                    {
                        result.Message = "优惠规则不合法";
                        return Json(result);
                    }

                    if (urle.SendGift && string.IsNullOrEmpty(urle.GiftData))
                    {
                        if (string.IsNullOrEmpty(urle.GiftData))
                        {
                            result.Message = "赠品规则不合法";
                            return Json(result);
                        }
                    }
                    if (urle.SendCoupon)
                    {
                        if (string.IsNullOrEmpty(urle.CouponData))
                        {
                            result.Message = "优惠券规则不合法";
                            return Json(result);
                        }
                    }

                }
                //如果当前层级包邮 之后的层级都必须包邮 且不包邮地区都一样
                //if (urle.)
                //{
                //    for (var j = i; j < len; j++)
                //    {
                //        rules[j].FreightFreeSelected = true;
                //        rules[j].FreightFreeExclude = urle.FreightFreeExclude;
                //    }
                //}
            }
            model.SellerId = User.Id;
            model.SellerName = User.Name;
            model.Status = true;
            model.CreatedOn = now;
            model.ModifiedBy = "";
            model.ModifiedOn = now;
            return Json(result);
        }

        /*
       [Route("packages")]
       public ActionResult Packages(int page = 1, int size = 30)
       {
           var model = new PromotionModel.Packages();
           var identity = User.Identity.Identity();
           model.SelectedMenu = "promotion.packages";
           model.Promotions = DataCore.PromotionPackagePagedList(page, size);
           return View("packages", model);
       }
       */

    }
}