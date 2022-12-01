namespace Tank.PaymentAPI.Services
{
    public class AppSetting
    {
        //jwt
        public string SecretKey { get; set; }
        
        //charge to game
        public string ChargeMoney_Url { get; set; }
        
        //mbbank
        public string APIMBBank_Url { get; set; }
        public string APIMBBank_AccountID { get; set; }
        public string APIMBBank_Pwd { get; set; }
        public string APIMBBank_Token { get; set; }
    
        //momo
        public string APIMomo_Url { get; set; }
        public string APIMomo_Token { get; set; }
    }
}
