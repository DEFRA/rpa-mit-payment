using System.Text;
using Newtonsoft.Json;

namespace EST.MIT.Payment.Function.Util;

public static class MessageSize
{
    public static double GetMessageSize(object message)
    {
        var json = JsonConvert.SerializeObject(message);
        var bytes = Encoding.UTF8.GetBytes(json);
        return bytes.Length / 1024.0;
    }
}
