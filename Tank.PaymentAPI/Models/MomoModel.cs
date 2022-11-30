using System;

namespace Tank.PaymentAPI.Models
{
    public class MomoModel
    {
        public long TranID { get; set; }
        public string UserID { get; set; }
        public string PartnerID { get; set; }
        public string PartnerName { get; set; }
        public int Amount { get; set; }
        public string Comment { get; set; }
        public DateTime PayTime { get; set; }
        public bool Checked { get; set; }
    }
}
