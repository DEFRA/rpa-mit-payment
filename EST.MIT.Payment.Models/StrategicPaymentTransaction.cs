namespace EST.MIT.Payment.Models;

public class StrategicPaymentTransaction
{
    public StrategicPaymentInstruction paymentInstruction { get; set; } = default!;
    public bool Accepted { get; set; }
}
