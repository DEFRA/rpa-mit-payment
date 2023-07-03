using EST.MIT.Payment.Models;

namespace EST.MIT.Payment.Interfaces;

public interface IPaymentRequesService
{
    bool ValidatePaymentRequest(StrategicPaymentInstruction paymentRequest);
}
