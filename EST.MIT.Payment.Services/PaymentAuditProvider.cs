using EST.MIT.Payment.Interfaces;

namespace EST.MIT.Payment.Services;
public class PaymentAuditProvider : IPaymentAuditProvider
{
    private readonly IPaymentAuditRepository _paymentAuditRepository;

    public PaymentAuditProvider(IPaymentAuditRepository paymentAuditRepository)
    {
        _paymentAuditRepository = paymentAuditRepository;
    }

    public void CreatePaymentInstruction(string strategicPaymentInstructionJson)
    {
        _paymentAuditRepository.CreatePaymentInstruction(strategicPaymentInstructionJson);
    }

    public async Task<string> GetPaymentInstructionByInvoiceNumberAsync(string strategicPaymentInstructionInvoiceNumber)
    {
        return await _paymentAuditRepository.GetPaymentInstructionByInvoiceNumberAsync(strategicPaymentInstructionInvoiceNumber);
    }

    public async Task<bool> ArchivePaymentInstructionAsync(string strategicPaymentInstructionInvoiceNumber)
    {
        return await _paymentAuditRepository.ArchivePaymentInstructionAsync(strategicPaymentInstructionInvoiceNumber);
    }
}
