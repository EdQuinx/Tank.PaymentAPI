using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tank.PaymentAPI.Models
{
    public class MBBankModel
    {
        public string TransactionID { get; set; }//ma giao dich
        public int Amount { get; set; }//so tien        
        public string Description { get; set; }//noi dung
        public DateTime TransactionDate { get; set; }//thoi gian giao dich
        public string Type { get; set; }//giao dich ra hay vao {IN, OUT}
        public bool? Checked { get; set; }//da kiem tra
    }
}
