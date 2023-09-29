using System.Text;
using EST.MIT.Payment.Function.Util;
using Newtonsoft.Json;

namespace EST.MIT.Payment.Function.Test.Util;

public class MessageSizeTests
{
    [Fact]
    public void GetMessageSize_ReturnsCorrectSizeInKilobytes()
    {
        var testObject = new { Name = "Test", Value = 123 };
        var serializedObject = JsonConvert.SerializeObject(testObject);
        var expectedSize = Encoding.UTF8.GetByteCount(serializedObject) / 1024.0;
        var actualSize = MessageSize.GetMessageSize(testObject);

        Assert.Equal(expectedSize, actualSize);
    }
}
