namespace Tank.PaymentAPI.Models
{
    public class MomoModel
    {
        public string User { get; set; }
        public long TranID { get; set; }
        public string PartnerID { get; set; }
        public string PartnerName { get; set; }
        public int Amount { get; set; }
        public string Comment { get; set; }
        public string Desc { get; set; }
    }
}
