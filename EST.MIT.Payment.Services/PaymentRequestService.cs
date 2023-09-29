using EST.MIT.Payment.Interfaces;
using EST.MIT.Payment.Models;

namespace EST.MIT.Payment.Services;

public class PaymentRequestService : IPaymentRequestService
{
    public bool ValidatePaymentRequest(StrategicPaymentInstruction? paymentRequest)
    {
        // Ensure paymentRequest is not null
        if (paymentRequest == null)
        {
            return false;
        }

        // Ensure sbi, value, schemeId, and frn are integers
        if (paymentRequest.Sbi <= 0 || paymentRequest.Value <= 0 || paymentRequest.SchemeId <= 0 || paymentRequest.Frn <= 0)
        {
            return false;
        }

        // Ensure marketingYear is in the past
        if (paymentRequest.MarketingYear >= DateTime.Now.Year)
        {
            return false;
        }

        // Ensure dueDate is a valid date in the future
        if (paymentRequest.DueDate <= DateTime.Now)
        {
            return false;
        }

        // Ensure there's at least one invoice line
        if (paymentRequest.PaymentDetails == null || paymentRequest.PaymentDetails?.Count < 1)
        {
            return false;
        }

        // Validate each payment detail
        foreach (var detail in paymentRequest.PaymentDetails!)
        {
            // Ensure strings have a value
            if (string.IsNullOrEmpty(detail.StandardCode) ||
                string.IsNullOrEmpty(detail.Description) ||
                string.IsNullOrEmpty(detail.SchemeCode) ||
                string.IsNullOrEmpty(detail.FundCode))
            {
                return false;
            }

            // Ensure value is a positive integer
            if (detail.Value <= 0)
            {
                return false;
            }
        }

        return true;
    }
}
