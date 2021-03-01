namespace Entities.Contracts
{
    // payment service that is able to take 
    //      - total for the amount to be charged and 
    //      - card which is debit/credit card that contains all the needed information to be charged
    public interface IPaymentService
    {
        bool Charge(double total, ICard card);
    }
}
