using EST.MIT.Payment.Models;

namespace EST.MIT.Payment.Interfaces;

public interface IPaymentRequestService
{
    bool ValidatePaymentRequest(StrategicPaymentInstruction paymentRequest);
}
