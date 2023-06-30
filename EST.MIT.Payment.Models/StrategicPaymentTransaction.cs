using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EST.MIT.Payment.Models;

public class StrategicPaymentTransaction
{
    public StrategicPaymentInstruction paymentInstruction { get; set; } = default!;
    public bool Accepted { get; set; }
}
