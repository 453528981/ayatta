using MediatR;
using System;

namespace Ayatta.Event
{
    public abstract class BaseEvent : INotification
    {
        public DateTime DateTime { get; set; } = DateTime.Now;
    }   
}
