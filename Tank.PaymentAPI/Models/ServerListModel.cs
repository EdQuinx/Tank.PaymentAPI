using System;
using System.Collections.Generic;

#nullable disable

namespace Tank.PaymentAPI.Models
{
    public partial class ServerListModel
    {
        public string Name { get; set; }
        public string DataSource { get; set; }
        public int? Port { get; set; }
        public string Catalog { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string KeyRequest { get; set; }
        public string RequestUrl { get; set; }
        public string FlashUrl { get; set; }
        public string ConfigUrl { get; set; }
        public int? Status { get; set; }
        public string LinkCenter { get; set; }
        public int Id { get; set; }
    }
}
