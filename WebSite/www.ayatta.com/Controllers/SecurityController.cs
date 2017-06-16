using Ayatta.Nsq;
using Ayatta.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    public class SecurityController : BaseController
    {
        private readonly INsqService nsqService;
        public SecurityController(INsqService nsqService, DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<SecurityController> logger) : base(defaultStorage, defaultCache, logger)
        {
            this.nsqService = nsqService;
        }

        /// <summary>
        /// 第三方帐号绑定
        /// </summary>            
        /// <returns></returns>
        [HttpGet("/security/test")]
        public IActionResult Test()
        {
           var id= nsqService.Publish("xxx", new TestMessage() { Name = "test" });
            return Content(id);
        }


        /// <summary>
        /// 第三方帐号绑定
        /// </summary>            
        /// <returns></returns>
        [HttpGet("/security/bind")]
        public IActionResult Bind()
        {
            return View();
        }

        /// <summary>
        /// 第三方帐号绑定
        /// </summary>            
        /// <returns></returns>
        [HttpPost("/security/bind")]
        public IActionResult Bind(string uid, string captcha)
        {
            return View();
        }
    }
}