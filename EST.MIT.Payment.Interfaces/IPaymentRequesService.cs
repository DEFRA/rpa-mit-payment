using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EST.MIT.Payment.Models;

namespace EST.MIT.Payment.Interfaces;

public interface IPaymentRequesService
{
    bool ValidatePaymentRequest(StrategicPaymentInstruction paymentRequest);
}
