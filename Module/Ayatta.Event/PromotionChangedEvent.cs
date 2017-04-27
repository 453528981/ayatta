using System;

namespace Ayatta.Event
{   
    public class PromotionChangedEvent : BaseEvent
    {
        public int Id { get; set; }

        public int SellerId { get; set; }
    }
}
