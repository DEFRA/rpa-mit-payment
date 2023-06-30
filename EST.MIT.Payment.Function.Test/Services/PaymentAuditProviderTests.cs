using EST.MIT.Payment.Interfaces;
using EST.MIT.Payment.Services;
using Moq;

namespace EST.MIT.Payment.Function.Test.Services;

public class PaymentAuditProviderTests
{
    private readonly PaymentAuditProvider _paymentAuditProvider;
    private readonly Mock<IPaymentAuditRepository> _mockPaymentAuditRepository;

    public PaymentAuditProviderTests()
    {
        _mockPaymentAuditRepository = new Mock<IPaymentAuditRepository>();
        _paymentAuditProvider = new PaymentAuditProvider(_mockPaymentAuditRepository.Object);
    }

    [Fact]
    public void CreatePaymentInstruction_CallsRepositoryWithCorrectJson()
    {
        var strategicPaymentInstructionJson = "{ \"Property1\": \"Value1\", \"Property2\": \"Value2\" }";
        _paymentAuditProvider.CreatePaymentInstruction(strategicPaymentInstructionJson);

        _mockPaymentAuditRepository.Verify(repo =>
            repo.CreatePaymentInstruction(It.Is<string>(json => json == strategicPaymentInstructionJson)), Times.Once);
    }

    [Fact]
    public async void GetPaymentInstructionByInvoiceNumber_CallsRepositoryWithCorrectInvoiceNumber()
    {
        var strategicPaymentInstructionInvoiceNumber = "INVOICE123";

        _mockPaymentAuditRepository.Setup(repo =>
            repo.GetPaymentInstructionByInvoiceNumberAsync(It.IsAny<string>()))
            .ReturnsAsync("Your Expected JSON Response");

        var result = await _paymentAuditProvider.GetPaymentInstructionByInvoiceNumberAsync(strategicPaymentInstructionInvoiceNumber);

        _mockPaymentAuditRepository.Verify(repo =>
            repo.GetPaymentInstructionByInvoiceNumberAsync(It.Is<string>(invoiceNumber => invoiceNumber == strategicPaymentInstructionInvoiceNumber)), Times.Once);

        Assert.Equal("Your Expected JSON Response", result);
    }

    [Fact]
    public async void ArchivePaymentInstructionAsync_CallsRepositoryWithCorrectInvoiceNumber()
    {
        var strategicPaymentInstructionInvoiceNumber = "INVOICE123";

        _mockPaymentAuditRepository.Setup(repo =>
            repo.ArchivePaymentInstructionAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        var result = await _paymentAuditProvider.ArchivePaymentInstructionAsync(strategicPaymentInstructionInvoiceNumber);

        _mockPaymentAuditRepository.Verify(repo =>
            repo.ArchivePaymentInstructionAsync(It.Is<string>(invoiceNumber => invoiceNumber == strategicPaymentInstructionInvoiceNumber)), Times.Once);

        Assert.True(result);
    }

}
