using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tank.PaymentAPI.Models
{
    public class ChargeValueModel
    {
        public int ID { get; set; }
        public int RealAmount { get; set; }
        public int GameAmount { get; set; }
        public double BonusRate { get; set; }
    }
}
