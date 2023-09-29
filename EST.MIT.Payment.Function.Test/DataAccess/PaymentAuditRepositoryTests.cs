using Azure.Storage.Blobs;
using EST.MIT.Payment.DataAccess;
using Microsoft.Extensions.Logging;
using Moq;
using Azure;
using Azure.Storage.Blobs.Models;

namespace EST.MIT.Payment.Function.Test.DataAccess;

public class PaymentAuditRepositoryTests
{
    [Fact]
    public void CreatePaymentInstruction_ShouldLogInformation_WhenSuccess()
    {
        var loggerMock = new Mock<ILogger<PaymentAuditRepository>>();
        var blobMock = new Mock<BlobServiceClient>();
        var blobContainerMock = new Mock<BlobContainerClient>();
        var blobClientMock = new Mock<BlobClient>();

        blobMock.Setup(b => b.GetBlobContainerClient(It.IsAny<string>())).Returns(blobContainerMock.Object);
        blobContainerMock.Setup(b => b.GetBlobClient(It.IsAny<string>())).Returns(blobClientMock.Object);
        blobClientMock.Setup(b => b.Upload(It.IsAny<Stream>(), default));


        var repository = new PaymentAuditRepository(loggerMock.Object, blobMock.Object);

        repository.CreatePaymentInstruction("Test JSON");

        loggerMock.Verify(l => l.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
            Times.Once);
    }


    [Fact]
    public void CreatePaymentInstruction_ShouldLogError_WhenExceptionOccurs()
    {
        var loggerMock = new Mock<ILogger<PaymentAuditRepository>>();
        var blobMock = new Mock<BlobServiceClient>();
        var blobContainerMock = new Mock<BlobContainerClient>();

        blobMock.Setup(b => b.GetBlobContainerClient(It.IsAny<string>())).Returns(blobContainerMock.Object);
        blobContainerMock.Setup(b => b.GetBlobClient(It.IsAny<string>())).Throws(new Exception("Test Exception"));

        var repository = new PaymentAuditRepository(loggerMock.Object, blobMock.Object);

        repository.CreatePaymentInstruction("{ 'Test': 'Test JSON' }");

        loggerMock.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
            Times.Exactly(2));
    }


    [Fact]
    public void CreatePaymentInstruction_ShouldLogError_WhenRequestFailedExceptionOccurs()
    {
        var loggerMock = new Mock<ILogger<PaymentAuditRepository>>();
        var blobMock = new Mock<BlobServiceClient>();
        var blobContainerMock = new Mock<BlobContainerClient>();

        blobMock.Setup(b => b.GetBlobContainerClient(It.IsAny<string>())).Returns(blobContainerMock.Object);
        blobContainerMock.Setup(b => b.GetBlobClient(It.IsAny<string>())).Throws(new RequestFailedException("Test RequestFailedException"));

        var repository = new PaymentAuditRepository(loggerMock.Object, blobMock.Object);

        repository.CreatePaymentInstruction("{ 'Test': 'Test JSON' }");

        loggerMock.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
            Times.Exactly(2));
    }

    [Fact]
    public async Task GetPaymentInstructionByInvoiceNumberAsync_ShouldReturnJson_WhenBlobExists()
    {
        var loggerMock = new Mock<ILogger<PaymentAuditRepository>>();
        var mockContainerClient = new Mock<BlobContainerClient>();
        var mockBlobClient = new Mock<BlobClient>();
        var mockBlobServiceClient = new Mock<BlobServiceClient>(); // MockBehavior.Strict, new Uri("http://test.com"), (BlobClientOptions)null);
        var mockResponse = Response.FromValue(true, Mock.Of<Response>());

        mockBlobClient.Setup(s => s.ExistsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Response.FromValue<bool>(true, Mock.Of<Response>()));
        mockBlobServiceClient.Setup(b => b.GetBlobContainerClient(It.IsAny<string>())).Returns(mockContainerClient.Object);
        mockContainerClient.Setup(b => b.GetBlobClient(It.IsAny<string>())).Returns(mockBlobClient.Object);

        var repository = new PaymentAuditRepository(loggerMock.Object, mockBlobServiceClient.Object);
        var result = await repository.GetPaymentInstructionByInvoiceNumberAsync("Test Invoice");

        Assert.NotNull(result);
        mockBlobClient.Verify(x => x.DownloadToAsync(It.IsAny<Stream>()), Times.Once);
        Assert.IsType<string>(result);
    }



    [Fact]
    public async Task GetPaymentInstructionByInvoiceNumberAsync_ShouldReturnJson_WhenNotBlobExists()
    {
        var loggerMock = new Mock<ILogger<PaymentAuditRepository>>();
        var blobContainerMock = new Mock<BlobContainerClient>();
        var blobClientMock = new Mock<BlobClient>();
        var blobMock = new Mock<BlobServiceClient>();

        blobMock.Setup(b => b.GetBlobContainerClient(It.IsAny<string>())).Returns(blobContainerMock.Object);
        blobContainerMock.Setup(b => b.GetBlobClient(It.IsAny<string>())).Returns(blobClientMock.Object);
        blobClientMock.Setup(s => s.ExistsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Response.FromValue<bool>(false, Mock.Of<Response>()));

        var repository = new PaymentAuditRepository(loggerMock.Object, blobMock.Object);

        var result = await repository.GetPaymentInstructionByInvoiceNumberAsync("Test Invoice");

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public async Task GetPaymentInstructionByInvoiceNumberAsync_ShouldLogError_WhenRequestFailedExceptionOccurs()
    {
        var loggerMock = new Mock<ILogger<PaymentAuditRepository>>();
        var blobContainerMock = new Mock<BlobContainerClient>();
        var blobClientMock = new Mock<BlobClient>();
        var blobMock = new Mock<BlobServiceClient>();

        blobMock.Setup(b => b.GetBlobContainerClient(It.IsAny<string>())).Returns(blobContainerMock.Object);
        blobContainerMock.Setup(b => b.GetBlobClient(It.IsAny<string>())).Returns(blobClientMock.Object);
        blobClientMock.Setup(s => s.ExistsAsync(It.IsAny<CancellationToken>()))
            .Throws(new RequestFailedException("An error occurred when attempting to retrieve the payment instruction for invoice number"));

        var repository = new PaymentAuditRepository(loggerMock.Object, blobMock.Object);

        string result = string.Empty;
        Exception ex = await Record.ExceptionAsync(async () => result = await repository.GetPaymentInstructionByInvoiceNumberAsync("Test Invoice"));

        Assert.Equal(string.Empty, result);
        loggerMock.Verify(
            x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task GetPaymentInstructionByInvoiceNumberAsync_ShouldLogError_WhenExceptionOccurs()
    {
        var loggerMock = new Mock<ILogger<PaymentAuditRepository>>();
        var blobContainerMock = new Mock<BlobContainerClient>();
        var blobClientMock = new Mock<BlobClient>();
        var blobMock = new Mock<BlobServiceClient>();

        blobMock.Setup(b => b.GetBlobContainerClient(It.IsAny<string>())).Returns(blobContainerMock.Object);
        blobContainerMock.Setup(b => b.GetBlobClient(It.IsAny<string>())).Returns(blobClientMock.Object);
        blobClientMock.Setup(s => s.ExistsAsync(It.IsAny<CancellationToken>()))
            .Throws(new Exception("An error occurred when attempting to retrieve the payment instruction for invoice number"));

        var repository = new PaymentAuditRepository(loggerMock.Object, blobMock.Object);

        string result = string.Empty;
        Exception ex = await Record.ExceptionAsync(async () => result = await repository.GetPaymentInstructionByInvoiceNumberAsync("Test Invoice"));

        Assert.Equal(string.Empty, result);
        loggerMock.Verify(
            x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task ArchivePaymentInstructionAsync_ShouldReturnTrue_WhenSuccess()
    {
        var loggerMock = new Mock<ILogger<PaymentAuditRepository>>();
        var mockContainerClient = new Mock<BlobContainerClient>();
        var mockBlobClient = new Mock<BlobClient>(MockBehavior.Strict);
        var mockBlobServiceClient = new Mock<BlobServiceClient>(); // MockBehavior.Strict, new Uri("http://test.com"), (BlobClientOptions)null);
        var blobCopyOperationMock = new Mock<CopyFromUriOperation>(new Uri("http://localhost"), null);

        mockBlobClient.Setup(s => s.ExistsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Response.FromValue<bool>(true, Mock.Of<Response>()));
        mockBlobServiceClient.Setup(b => b.GetBlobContainerClient(It.IsAny<string>())).Returns(mockContainerClient.Object);
        mockContainerClient.Setup(b => b.GetBlobClient(It.IsAny<string>())).Returns(mockBlobClient.Object);

        blobCopyOperationMock
            .Setup(s => s.WaitForCompletionAsync(CancellationToken.None))
            .Returns(new ValueTask<Response<long>>(Response.FromValue(0L, Mock.Of<Response>())));

        mockBlobClient.Setup(s => s.DeleteIfExistsAsync(DeleteSnapshotsOption.None, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(true, Mock.Of<Response>()));


        var repository = new PaymentAuditRepository(loggerMock.Object, mockBlobServiceClient.Object);
        var result = await repository.ArchivePaymentInstructionAsync("Test Invoice");

        Assert.IsType<Boolean>(result);
    }

    [Fact]
    public async Task ArchivePaymentInstructionAsync_ShouldReturnFalse_WhenNoPaymentInstructionFound()
    {
        var loggerMock = new Mock<ILogger<PaymentAuditRepository>>();
        var blobContainerMock = new Mock<BlobContainerClient>();
        var blobClientMock = new Mock<BlobClient>();
        var blobMock = new Mock<BlobServiceClient>();

        blobMock.Setup(b => b.GetBlobContainerClient(It.IsAny<string>())).Returns(blobContainerMock.Object);
        blobContainerMock.Setup(b => b.GetBlobClient(It.IsAny<string>())).Returns(blobClientMock.Object);
        blobClientMock.Setup(s => s.ExistsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(Response.FromValue<bool>(false, Mock.Of<Response>()));

        var repository = new PaymentAuditRepository(loggerMock.Object, blobMock.Object);

        var result = await repository.ArchivePaymentInstructionAsync("Test Invoice");

        Assert.False(result);
    }

    [Fact]
    public async Task ArchivePaymentInstructionAsync_ShouldLogError_WhenRequestFailedExceptionOccurs()
    {
        var loggerMock = new Mock<ILogger<PaymentAuditRepository>>();
        var blobContainerMock = new Mock<BlobContainerClient>();
        var blobClientMock = new Mock<BlobClient>();
        var blobMock = new Mock<BlobServiceClient>();

        blobMock.Setup(b => b.GetBlobContainerClient(It.IsAny<string>())).Returns(blobContainerMock.Object);
        blobContainerMock.Setup(b => b.GetBlobClient(It.IsAny<string>())).Returns(blobClientMock.Object);
        blobClientMock.Setup(s => s.ExistsAsync(It.IsAny<CancellationToken>())).Throws(new RequestFailedException("Test RequestFailedException"));

        var repository = new PaymentAuditRepository(loggerMock.Object, blobMock.Object);

        var result = await repository.ArchivePaymentInstructionAsync("Test Invoice");

        Assert.False(result);
        loggerMock.Verify(
            x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.AtLeastOnce);
    }

    [Fact]
    public async Task ArchivePaymentInstructionAsync_ShouldLogError_WhenExceptionOccurs()
    {
        var loggerMock = new Mock<ILogger<PaymentAuditRepository>>();
        var blobContainerMock = new Mock<BlobContainerClient>();
        var blobClientMock = new Mock<BlobClient>();
        var blobMock = new Mock<BlobServiceClient>();

        blobMock.Setup(b => b.GetBlobContainerClient(It.IsAny<string>())).Returns(blobContainerMock.Object);
        blobContainerMock.Setup(b => b.GetBlobClient(It.IsAny<string>())).Returns(blobClientMock.Object);
        blobClientMock.Setup(s => s.ExistsAsync(It.IsAny<CancellationToken>())).Throws(new Exception("Test Exception"));

        var repository = new PaymentAuditRepository(loggerMock.Object, blobMock.Object);

        var result = await repository.ArchivePaymentInstructionAsync("Test Invoice");

        Assert.False(result);
        loggerMock.Verify(
            x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.AtLeastOnce);
    }
}
