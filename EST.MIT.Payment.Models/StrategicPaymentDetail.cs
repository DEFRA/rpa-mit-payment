namespace EST.MIT.Payment.Models;
public class StrategicPaymentDetail
{
    public string StandardCode { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Value { get; set; }
    public string SchemeCode { get; set; } = default!;
    public string FundCode { get; set; } = default!;
}
