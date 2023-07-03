using EST.MIT.Payment.Models;

namespace EST.MIT.Payment.Interfaces;

public interface IStrategicPaymentTransactionJsonGenerator
{
    string Generate(StrategicPaymentTransaction strategicPaymentTransaction);
    Task Send(StrategicPaymentTransaction strategicPayment);
}
