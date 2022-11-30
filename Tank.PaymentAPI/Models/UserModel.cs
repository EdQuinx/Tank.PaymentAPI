using System;
using System.Collections.Generic;

#nullable disable

namespace Tank.PaymentAPI.Models
{
    public partial class UserModel
    {
        public UserModel()
        {
            LogCards = new HashSet<LogCardModel>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Money { get; set; }
        public string PhoneNumber { get; set; }
        public int VipLevel { get; set; }
        public int VipExp { get; set; }
        public DateTime CreateAt { get; set; }
        public bool IsExist { get; set; }

        public virtual ICollection<LogCardModel> LogCards { get; set; }
    }
}
