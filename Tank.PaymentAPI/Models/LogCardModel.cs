using System;
using System.Collections.Generic;

#nullable disable

namespace Tank.PaymentAPI.Models
{
    public partial class LogCardModel
    {
        public int Id { get; set; }
        public string CardType { get; set; }
        public string CardName { get; set; }
        public string CardCode { get; set; }
        public string CardSeri { get; set; }
        public DateTime CreateAt { get; set; }
        public int Status { get; set; }
        public string Note { get; set; }
        public int UserId { get; set; }

        public virtual UserModel User { get; set; }
    }
}
