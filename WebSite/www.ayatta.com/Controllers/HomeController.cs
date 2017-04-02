using System;
using Ayatta;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Ayatta.Web.Models;

namespace Ayatta.Web.Controllers
{

    public class HomeController : BaseController
    {
        public HomeController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<HomeController> logger) : base(defaultStorage, defaultCache, logger)
        {

        }

        [HttpGet("/")]
        public IActionResult Index()
        {
           
            return View();
        }

    }
}