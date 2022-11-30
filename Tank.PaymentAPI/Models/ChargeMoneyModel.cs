using System;
using System.Collections.Generic;

namespace Tank.PaymentAPI.Models
{
    public partial class ChargeMoneyModel
    {
        public string ChargeId { get; set; }
        public string UserName { get; set; }
        public int Money { get; set; }
        public DateTime Date { get; set; }
        public bool CanUse { get; set; }
        public string PayWay { get; set; }
        public decimal NeedMoney { get; set; }
        public string Ip { get; set; }
        public string NickName { get; set; }
    }
}
