using System;
using MediatR;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Ayatta.Event.Handler
{
    public class PromotionChangedHandler : INotificationHandler<PromotionChangedEvent>
    {
        private readonly ILogger logger;
        public PromotionChangedHandler(ILogger<PromotionChangedHandler> logger)
        {
            this.logger = logger;
        }
        public void Handle(PromotionChangedEvent e)
        {
            logger.LogInformation("EventHandler " + e.DateTime);
        }
    }
}
