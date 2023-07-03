using EST.MIT.Payment.Interfaces;
using Azure.Storage.Blobs;
using Azure;
using Microsoft.Extensions.Logging;
using System.Text;

namespace EST.MIT.Payment.DataAccess;
public class PaymentAuditRepository : IPaymentAuditRepository
{
    private readonly ILogger<PaymentAuditRepository> _logger;
    private readonly BlobServiceClient _blobServiceClient;

    public PaymentAuditRepository(ILogger<PaymentAuditRepository> logger, BlobServiceClient blobServiceClient)
    {
        _logger = logger;
        _blobServiceClient = blobServiceClient;
    }

    public void CreatePaymentInstruction(string strategicPaymentInstructionJson)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("Payment");
            var blobClient = containerClient.GetBlobClient($"{Guid.NewGuid()}.json");
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(strategicPaymentInstructionJson));
            blobClient.Upload(stream, true);

            _logger.LogInformation($"Payment Payment logged within the audit store.");
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError("An error occured when attempting to log the Payment Payment within the audit store.");
            _logger.LogError(ex.ErrorCode);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occured when attempting to log the Payment Payment within the audit store.");
            _logger.LogError(ex.Message);
        }
    }

    public async Task<string> GetPaymentInstructionByInvoiceNumberAsync(string strategicPaymentInstructionInvoiceNumber)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("Payment");
            var blobClient = containerClient.GetBlobClient($"{strategicPaymentInstructionInvoiceNumber}.json");

            if (!await blobClient.ExistsAsync())
            {
                _logger.LogError($"No payment instruction found for invoice number: {strategicPaymentInstructionInvoiceNumber}");
                return String.Empty;
            }

            var stream = new MemoryStream();
            await blobClient.DownloadToAsync(stream);
            stream.Position = 0;

            using var reader = new StreamReader(stream);
            var paymentInstructionJson = await reader.ReadToEndAsync();

            _logger.LogInformation($"Payment instruction retrieved for invoice number: {strategicPaymentInstructionInvoiceNumber}");
            return paymentInstructionJson;

        }
        catch (RequestFailedException ex)
        {
            _logger.LogError($"An error occurred when attempting to retrieve the payment instruction for invoice number: {strategicPaymentInstructionInvoiceNumber}");
            _logger.LogError(ex.ErrorCode);
            return String.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred when attempting to retrieve the payment instruction for invoice number: {strategicPaymentInstructionInvoiceNumber}");
            _logger.LogError(ex.Message);
            return String.Empty;
        }
    }

    public async Task<bool> ArchivePaymentInstructionAsync(string strategicPaymentInstructionInvoiceNumber)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("Payment");
            var sourceBlobClient = containerClient.GetBlobClient($"{strategicPaymentInstructionInvoiceNumber}.json");
            var destBlobClient = containerClient.GetBlobClient($"archive/{strategicPaymentInstructionInvoiceNumber}");

            if (!await sourceBlobClient.ExistsAsync())
            {
                _logger.LogError($"No payment instruction found for invoice number: {strategicPaymentInstructionInvoiceNumber}");
                return false;
            }

            var copyOperation = await destBlobClient.StartCopyFromUriAsync(sourceBlobClient.Uri);
            await copyOperation.WaitForCompletionAsync();
            await sourceBlobClient.DeleteIfExistsAsync();
            _logger.LogInformation($"File {strategicPaymentInstructionInvoiceNumber} moved to archive folder.");

            return true;
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError($"An error occured when moving the file [{strategicPaymentInstructionInvoiceNumber}] to the archive folder.");
            _logger.LogError(ex.ErrorCode);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured when moving the file [{strategicPaymentInstructionInvoiceNumber}] to the archive folder.");
            _logger.LogError(ex.Message);
            return false;
        }
    }
}
