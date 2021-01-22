using OrderService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Publisher
{
    public interface IPublisher
    {
        Task<string> VerifyReservationId(VerificationReservationId verificationtion);
        Task<string> PayRoom(PaymentModel payment);
        Task PublishPayment(Payment message);
    }
}
