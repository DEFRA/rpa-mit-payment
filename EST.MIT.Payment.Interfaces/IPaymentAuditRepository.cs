namespace EST.MIT.Payment.Interfaces;

public interface IPaymentAuditRepository
{
    void CreatePaymentInstruction(string strategicPaymentInstructionJson);
    Task<string> GetPaymentInstructionByInvoiceNumberAsync(string strategicPaymentInstructionInvoiceNumber);
    Task<Boolean> ArchivePaymentInstructionAsync(string strategicPaymentInstructionInvoiceNumber);
}
