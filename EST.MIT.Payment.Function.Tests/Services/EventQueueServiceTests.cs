using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using EST.MIT.Payment.Services;
using Moq;

namespace EST.MIT.Payment.Function.Tests.Services
{
    public class EventQueueServiceTests
    {
        private readonly Mock<QueueClient> _mockqueueClient;

        public EventQueueServiceTests()
        {
            _mockqueueClient = new Mock<QueueClient>();
        }

        [Fact]
        public async Task CreateMessage_ValidArguments_CallsSendMessageAsync()
        {
            var eventQueueService = new EventQueueService(_mockqueueClient.Object);
            const string status = "new";
            const string action = "create";
            const string message = "payment request created successfully";
            const string data = "{\n\"invoices\":[\n{\n\"id\":\"string\",\n\"invoiceType\":\"string\",\n\"accountType\":\"string\",\n\"organisation\":\"string\",\n\"schemeType\": \"string\",\n\"paymentRequests\":[\n{\n\"paymentRequestId\": \"string\",\n\"frn\": 0,\n\"sourceSystem\":\"string\",\n\"marketingYear\": 0,\n\"deliveryBody\": \"string\",\n\"paymentRequestNumber\": 0,\n\"agreementNumber\": \"string\",\n\"contractNumber\":\"string\",\n \"value\": 0,\n\"dueDate\":\"string\",\n\"invoiceLines\":[\n{\n\"value\":0,\n\"currency\":\"string\",\n\"schemeCode\":\"string\",\n\"description\": \"string\",\n\"fundCode\": \"string\"\n}\n],\n\"appendixReferences\":{\n\"claimReferenceNumber\":\"string\"\n}\n}\n],\n\"status\":\"string\",\n\"reference\":\"string\",\n\"created\":\"2023-04-03T07:18:19.457Z\",\n\"updated\":\"2023-04-03T07:18:19.457Z\",\n\"createdBy\":\"string\",\n\"updatedBy\":\"string\"\n}\n],\n\"schemeType\":\"AP\"\n}";
            var response = new Mock<Response<SendReceipt>>();
            _mockqueueClient.Setup(x => x.SendMessageAsync(It.IsAny<string>())) // Push the message onto the queue.
            .ReturnsAsync(response.Object);

            await eventQueueService.CreateMessage(status, action, message, data);

            _mockqueueClient.Verify(x => x.SendMessageAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
