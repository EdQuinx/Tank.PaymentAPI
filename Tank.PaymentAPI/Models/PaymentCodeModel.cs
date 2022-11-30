using System;
using System.ComponentModel;

namespace Tank.PaymentAPI.Models
{
    public class PaymentCodeModel
    {
        public string Code { get; set; }
        public int Amount { get; set; }
        public DateTime EndTime { get; set; }
    }
}
