using Entities.Contracts;

namespace Repository.Moq
{
    public class PaymentServiceMoq : IPaymentService
    {
        public bool Charge(double total, ICard card)
        {
            throw new System.NotImplementedException();
        }
    }
}
