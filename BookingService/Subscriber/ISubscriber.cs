using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingService.Subscriber
{
    public interface ISubscriber
    {
        void SubscribeJobMessage();
        Task SubscribeVerificationReservationId();
        void SubscribePayment();

    }
}
