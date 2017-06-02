using Ayatta.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    public abstract class BaseController : AbstractController
    {
        public BaseController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<BaseController> logger) : base(defaultStorage, defaultCache, logger)
        {
        }
    }
}