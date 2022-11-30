namespace Tank.PaymentAPI.Services
{
    public enum eDatabaseType
    {
        SUCCESS = 0,
        SERVER_NOTFOUND = 1,
        PAYMENT_CODE_NOTFOUND = 2,
        PAYMENT_CODE_EXPIRED = 3,
        AMOUNT_VALUE_NOTFOUND = 4,
        USER_NOTFOUND = 5,
        TRANSACTION_NOTFOUND = 6,
        SYSTEM_ERROR = 10,
    }
}
