using Ayatta;
using Ayatta.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;


namespace Ayatta.Web.Controllers
{
    public class MiscController : BaseController
    {
        public MiscController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<MiscController> logger) : base(defaultStorage, defaultCache, logger)
        {
        }



        [HttpPost("/misc/mobile-captcha-send/0")]
        public IActionResult SendMobileCaptcha(string mobile)
        {
            var result = new Result<string>();
            result.Status = true;
            return Json(result);
        }
    }
}