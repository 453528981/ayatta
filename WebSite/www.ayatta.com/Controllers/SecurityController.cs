using Ayatta.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;


namespace Ayatta.Web.Controllers
{
    public class SecurityController : BaseController
    {
        public SecurityController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<SecurityController> logger) : base(defaultStorage, defaultCache, logger)
        {
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