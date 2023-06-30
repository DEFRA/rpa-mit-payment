namespace EST.MIT.Payment.Interfaces;

public interface IPaymentAuditProvider
{
    void CreatePaymentInstruction(string strategicPaymentInstructionJson);
    Task<string> GetPaymentInstructionByInvoiceNumberAsync(string strategicPaymentInstructionInvoiceNumber);
    Task<Boolean> ArchivePaymentInstructionAsync(string strategicPaymentInstructionInvoiceNumber);
}
