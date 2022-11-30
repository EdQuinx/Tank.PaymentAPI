using System;
using System.Dynamic;

namespace Tank.PaymentAPI.Interfaces.IModels
{
    public class IMBBankModel
    {
        public string TransactionID { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool? Checked { get; set; }
    }
}
